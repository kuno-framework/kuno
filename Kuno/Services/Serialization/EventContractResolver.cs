/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Kuno.Serialization;
using Kuno.Services.Messaging;

namespace Kuno.Services.Serialization
{
    /// <summary>
    /// A JSON Contract Resolver for <see cref="EventMessage" /> instances.
    /// </summary>
    public class EventContractResolver : BaseContractResolver
    {
        /// <summary>
        /// Creates a <see cref="T:Newtonsoft.Json.Serialization.JsonProperty" /> for the given <see cref="T:System.Reflection.MemberInfo" />.
        /// </summary>
        /// <param name="member">The member to create a <see cref="T:Newtonsoft.Json.Serialization.JsonProperty" /> for.</param>
        /// <param name="memberSerialization">The member's parent <see cref="T:Newtonsoft.Json.MemberSerialization" />.</param>
        /// <returns>A created <see cref="T:Newtonsoft.Json.Serialization.JsonProperty" /> for the given <see cref="T:System.Reflection.MemberInfo" />.</returns>
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var prop = base.CreateProperty(member, memberSerialization);
            if ((member as PropertyInfo).GetCustomAttributes<IgnoreAttribute>().Any())
            {
                prop.Ignored = true;
                return prop;
            }
            var declaringType = (member as PropertyInfo)?.DeclaringType;
            if (declaringType == typeof(EventMessage))
            {
                prop.Ignored = true;
                return prop;
            }
            return base.CreateProperty(member, memberSerialization);
        }
    }
}