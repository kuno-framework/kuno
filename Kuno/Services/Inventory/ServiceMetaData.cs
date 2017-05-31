/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using Kuno.Reflection;

namespace Kuno.Services.Inventory
{
    /// <summary>
    /// A service in the service registry.
    /// </summary>
    public class ServiceMetaData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceMetaData" /> class.
        /// </summary>
        /// <param name="service">The service type.</param>
        /// <param name="rootPath">The root path.</param>
        public ServiceMetaData(Type service, string rootPath)
        {
            this.Path = service.GetPath();
            this.RootPath = rootPath;
            this.EndPoints = EndPointMetaData.Create(service).ToList();
            this.ServiceType = service;
            this.Name = service.Name;
            var attribute = service.GetAllAttributes<EndPointAttribute>().FirstOrDefault();
            if (attribute != null)
            {
                this.Name = attribute.Name ?? this.Name;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceMetaData" /> class.
        /// </summary>
        public ServiceMetaData()
        {
        }

        /// <summary>
        /// Gets or sets the end points.
        /// </summary>
        /// <value>The end points.</value>
        public List<EndPointMetaData> EndPoints { get; set; } = new List<EndPointMetaData>();

        /// <summary>
        /// Gets or sets the service name.
        /// </summary>
        /// <value>The service name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        /// <value>The path.</value>
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets the root path.
        /// </summary>
        /// <value>The root path.</value>
        public string RootPath { get; set; }

        /// <summary>
        /// Gets or sets the service type.
        /// </summary>
        /// <value>The service type.</value>
        public Type ServiceType { get; set; }
    }
}