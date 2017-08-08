/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using Kuno.Domain;

namespace Kuno.Tests
{
    /// <summary>
    /// A scenario is a set of data and conditions that are used fora  test.  It uses an in-memory entity context.
    /// </summary>
    public class Scenario
    {
        /// <summary>
        /// Gets or sets the entity context.
        /// </summary>
        /// <value>
        /// The entity context.
        /// </value>
        public InMemoryEntityContext EntityContext { get; set; } = new InMemoryEntityContext();

        /// <summary>
        /// Adds data to the in-memory entity context.
        /// </summary>
        /// <param name="items">The items to add.</param>
        /// <returns>Returns this instance for method chaining.</returns>
        public Scenario WithData(params IAggregateRoot[] items)
        {
            this.EntityContext.Add(items).Wait();

            return this;
        }
    }
}