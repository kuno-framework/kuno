/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

namespace Kuno.Services.Services
{
    /// <summary>
    /// Checks the health of the application and returns no content if no issues are found.  An exception is raised when there is an issue present.
    /// </summary>
    [EndPoint("_system/health", Method = "GET")]
    public class CheckHealth : Function
    {
        /// <inheritdoc />
        public override void Receive()
        {
        }
    }
}