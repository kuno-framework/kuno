/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using Autofac;
using Kuno.Services.Inventory;

namespace Kuno.Services
{
    /// <summary>
    /// EndPoint extention methods for Stacks.
    /// </summary>
    public static class ServiceExtensions
    {
        /// <summary>
        /// Gets the local service inventory.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>Kuno.Services.Inventory.ServiceInventory.</returns>
        public static ServiceInventory GetServices(this Stack instance)
        {
            return instance.Container.Resolve<ServiceInventory>();
        }
    }
}