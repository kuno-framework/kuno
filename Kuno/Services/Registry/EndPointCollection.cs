/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Kuno.Services.Registry
{
    /// <summary>
    /// A list of endpoints with methods to find internal functions.
    /// </summary>
    public class EndPointCollection : List<EndPoint>
    {
        /// <summary>
        /// Finds the registered endpoint based on the specified path or message if no path is specified.
        /// </summary>
        /// <param name="path">The endpoint path.</param>
        /// <param name="message">The message being sent.</param>
        /// <returns>
        /// Returns the registered endpoint based on the specified path or message if no path is specified.
        /// </returns>
        public EndPoint Find(string path, object message)
        {
            var target = this.Find(path);
            if (target != null)
            {
                return target;
            }
            return this.FirstOrDefault<EndPoint>(e => e.Function.RequestType == message?.GetType());
        }

        /// <summary>
        /// Finds the registered endpoint based on the specified path.
        /// </summary>
        /// <param name="path">The endpoint path.</param>
        /// <returns>Returns the registered endpoint based on the specified path.</returns>
        public EndPoint Find(string path)
        {
            if (path != null)
            {
                if (Regex.IsMatch(path, "v\\d*\\/", RegexOptions.Compiled))
                {
                    var target = this.FirstOrDefault<EndPoint>(e => e.VersionedPath == path);
                    if (target != null)
                    {
                        return target;
                    }
                }
                else
                {
                    var target = this.Where<EndPoint>(e => e.Path == path).OrderBy(e => e.Version).LastOrDefault();
                    if (target != null)
                    {
                        return target;
                    }
                }
            }
            return null;
        }
    }
}