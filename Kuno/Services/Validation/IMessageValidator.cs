using System.Collections.Generic;
using System.Threading.Tasks;
using Kuno.Services.Messaging;
using Kuno.Validation;

namespace Kuno.Services.Validation
{
    /// <summary>
    /// Validates messages using input, security and business rules.
    /// </summary>
    public interface IMessageValidator
    {
        /// <summary>
        /// Validates the specified message.
        /// </summary>
        /// <param name="message">The message to validate.</param>
        /// <param name="context">The current context.</param>
        /// <returns>The <see cref="ValidationError">messages</see> returned from validation routines.</returns>
        Task<IEnumerable<ValidationError>> Validate(IMessage message, ExecutionContext context);
    }
}