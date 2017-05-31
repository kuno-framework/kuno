/* 
 * Copyright (c) Stacks Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

namespace Kuno.Domain
{
    /// <summary>
    /// Represents an Aggregate Root.
    /// </summary>
    /// <seealso cref="Kuno.Domain.Entity" />
    /// <seealso cref="Kuno.Domain.IAggregateRoot" />
    /// <seealso href="https://lostechies.com/jimmybogard/2008/05/21/entities-value-objects-aggregates-and-roots/">Entities, Value Objects, Aggregates and Roots</seealso>
    public abstract class AggregateRoot : Entity, IAggregateRoot
    {
    }
}