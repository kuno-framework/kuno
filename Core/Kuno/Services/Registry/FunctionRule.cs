/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System;
using System.Reflection;
using Kuno.Reflection;
using Kuno.Services.Validation;
using Kuno.Text;
using Kuno.Validation;

namespace Kuno.Services.Registry
{
    /// <summary>
    /// Contains information about a function rule.
    /// </summary>
    public class FunctionRule
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FunctionRule" /> class.
        /// </summary>
        /// <param name="type">The function rule type.</param>
        public FunctionRule(Type type)
        {
            this.Name = type.Name.ToSentence();
            var baseType = type.GetTypeInfo().BaseType?.GetGenericTypeDefinition();
            if (baseType == typeof(BusinessRule<>))
            {
                this.RuleType = ValidationType.Business;
            }
            if (baseType == typeof(SecurityRule<>))
            {
                this.RuleType = ValidationType.Security;
            }
            if (baseType == typeof(InputRule<>))
            {
                this.RuleType = ValidationType.Input;
            }
            this.Comments = type.GetComments();
        }

        /// <summary>
        /// Gets or sets the comments.
        /// </summary>
        /// <value>The comments.</value>
        public Comments Comments { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the type of the rule.
        /// </summary>
        /// <value>The type of the rule.</value>
        public ValidationType RuleType { get; set; }
    }
}