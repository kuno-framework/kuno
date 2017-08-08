/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System;

namespace Kuno.Configuration
{
    /// <summary>
    /// Indicates that the class should be loaded with any loading routines.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class AutoLoadAttribute : Attribute
    {
    }
}