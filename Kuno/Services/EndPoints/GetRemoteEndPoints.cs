/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System.Linq;
using Kuno.Services.Inventory;
using Kuno.Validation;

namespace Kuno.Services.EndPoints
{
    /// <summary>
    /// Gets all connected remote endpoints.
    /// </summary>
    [EndPoint("_system/endpoints/remote", Method = "GET", Public = false)]
    public class GetRemoteEndPoints : EndPoint
    {
        private readonly RemoteServiceInventory _inventory;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetRemoteEndPoints" /> class.
        /// </summary>
        /// <param name="inventory">The current inventory.</param>
        public GetRemoteEndPoints(RemoteServiceInventory inventory)
        {
            Argument.NotNull(inventory, nameof(inventory));

            _inventory = inventory;
        }

        /// <inheritdoc />
        public override void Receive()
        {
            this.Respond(_inventory.Services.Select(e => new
            {
                e.Path,
                e.EndPoints
            }));
        }
    }
}