/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System;

namespace Kuno.Tests
{
    /// <summary>
    /// Indicates the scenario that the test should use.
    /// </summary>
    public class GivenAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GivenAttribute"/> class.
        /// </summary>
        /// <param name="scenario">The scenario type.</param>
        public GivenAttribute(Type scenario)
        {
            this.Scenario = scenario;
        }

        /// <summary>
        /// Gets the scenario type.
        /// </summary>
        /// <value>
        /// The scenario type.
        /// </value>
        public Type Scenario { get; private set; }
    }
}