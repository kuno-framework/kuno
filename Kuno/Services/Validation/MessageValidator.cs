/* 
 * Copyright (c) Stacks Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Kuno.Services.Messaging;
using Kuno.Validation;

namespace Kuno.Services.Validation
{
    /// <summary>
    /// Validates a message using input, security and business rules.
    /// </summary>
    public class MessageValidator<TCommand> : IMessageValidator where TCommand : class
    {
        private readonly IEnumerable<IValidate<TCommand>> _rules;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageValidator{TCommand}" /> class.
        /// </summary>
        /// <param name="rules">The rules for the message.</param>
        public MessageValidator(IEnumerable<IValidate<TCommand>> rules)
        {
            Argument.NotNull(rules, nameof(rules));

            _rules = rules;
        }

        /// <summary>
        /// Validates the specified message.
        /// </summary>
        /// <param name="command">The message to validate.</param>
        /// <param name="context">The current context.</param>
        /// <returns>The <see cref="ValidationError">messages</see> returned from validation routines.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="command" /> argument is null.</exception>
        public Task<IEnumerable<ValidationError>> Validate(object command, ExecutionContext context)
        {
            Argument.NotNull(command, nameof(command));

            var instance = command as TCommand;
            if (instance == null)
            {
                instance = (TCommand) JsonConvert.DeserializeObject(JsonConvert.SerializeObject(command), typeof(TCommand));
            }

            if (context.EndPoint.Secure && !(context.Request.User?.Identity?.IsAuthenticated ?? false))
            {
                return Task.FromResult(new[] {new ValidationError("Unauthorized", "The call to \"" + context.Request.Path + "\" requires authentication.", ValidationType.Security)}.AsEnumerable());
            }


            var input = this.CheckInputRules(instance).ToList();
            if (input.Any())
            {
                return Task.FromResult(input.WithType(ValidationType.Input));
            }

            var security = this.CheckSecurityRules(instance, context).ToList();
            if (security.Any())
            {
                return Task.FromResult(security.WithType(ValidationType.Security));
            }

            var business = this.CheckBusinessRules(instance, context).ToList();
            if (business.Any())
            {
                return Task.FromResult(business.WithType(ValidationType.Business));
            }

            return Task.FromResult(Enumerable.Empty<ValidationError>());
        }

        /// <summary>
        /// Checks all discovered business rules for validation errors.
        /// </summary>
        /// <param name="command">The message to validate.</param>
        /// <param name="context">The context.</param>
        /// <returns>A task for asynchronous programming.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="command" /> argument is null.</exception>
        protected virtual IEnumerable<ValidationError> CheckBusinessRules(TCommand command, ExecutionContext context)
        {
            foreach (var rule in _rules.OfType<IBusinessRule<TCommand>>())
            {
                if (rule is IUseExecutionContext)
                {
                    ((IUseExecutionContext) rule).UseContext(context);
                }
                var result = rule.Validate(command).ToList();
                if (result.Any())
                {
                    return result;
                }
            }
            return Enumerable.Empty<ValidationError>();
        }

        /// <summary>
        /// Checks all discovered input rules for validation errors.
        /// </summary>
        /// <param name="command">The message to validate.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="command" /> argument is null.</exception>
        /// <returns>A task for asynchronous programming.</returns>
        protected virtual IEnumerable<ValidationError> CheckInputRules(TCommand command)
        {
            var target = new List<ValidationError>();
            foreach (var property in command.GetType().GetProperties())
            {
                target.AddRange(this.CheckRules(property, () => property.GetValue(command)));
            }
            if (!target.Any())
            {
                foreach (var rule in _rules.OfType<IInputRule<TCommand>>())
                {
                    target.AddRange(rule.Validate(command));
                }
            }
            return target.AsEnumerable();
        }

        /// <summary>
        /// Checks all discovered security rules for validation errors.
        /// </summary>
        /// <param name="command">The message to validate.</param>
        /// <param name="context">The current context.</param>
        /// <returns>A task for asynchronous programming.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="command" /> argument is null.</exception>
        protected virtual IEnumerable<ValidationError> CheckSecurityRules(TCommand command, ExecutionContext context)
        {
            foreach (var rule in _rules.OfType<ISecurityRule<TCommand>>())
            {
                if (rule is IUseExecutionContext)
                {
                    ((IUseExecutionContext) rule).UseContext(context);
                }
                var result = rule.Validate(command).ToList();
                if (result.Any())
                {
                    return result;
                }
            }
            return Enumerable.Empty<ValidationError>();
        }

        private IEnumerable<ValidationError> CheckRules(PropertyInfo property, Func<object> value, string prefix = null)
        {
            if (prefix?.Count(e => e == '.') > 2)
            {
                yield break;
            }

            foreach (var attribute in property.GetCustomAttributes<ValidationAttribute>())
            {
                if (!attribute.IsValid(value()))
                {
                    if (attribute.Code == null)
                    {
                        attribute.Code = $"{prefix}{property.DeclaringType.Name}.{property.Name}.{attribute.GetType().Name.Replace("Attribute", "")}";
                    }
                    yield return attribute.GetValidationError(property);
                }
            }

            foreach (var item in property.PropertyType.GetProperties())
            {
                if (item.DeclaringType == property.DeclaringType)
                {
                    continue;
                }
                foreach (var error in this.CheckRules(item, () =>
                {
                    var target = value();
                    return target == null ? null : item.GetValue(target);
                }, $"{prefix}{property.DeclaringType.Name}."))
                {
                    yield return error;
                }
            }
        }
    }
}