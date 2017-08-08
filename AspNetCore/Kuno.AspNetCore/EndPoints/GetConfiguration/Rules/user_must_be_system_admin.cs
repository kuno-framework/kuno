/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System;
using Kuno.Services.Validation;
using Kuno.Validation;

namespace Kuno.AspNetCore.EndPoints.GetConfiguration.Rules
{
    /// <summary>
    /// Validates that the current user is a system administrator.
    /// </summary>
    public class user_must_be_system_admin : SecurityRule<GetConfigurationRequest>
    {
        /// <inheritdoc />
        public override ValidationError Validate(GetConfigurationRequest instance)
        {
            if (!this.Request.User.IsInRole("System Administrator"))
            {
                return "You must be a system administrator to veiw the configuration.";
            }
            return null;
        }
    }
}