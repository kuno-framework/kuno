/* 
 * Copyright (c) Stacks Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System;

namespace Kuno.Serialization
{
    /// <summary>
    /// Indicates that a property should be handled securely when logged.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class SecureAttribute : Attribute
    {
        /// <summary>
        /// The default display text.
        /// </summary>
        public const string DefaultDisplayText = "[SECURE]";
    }
}