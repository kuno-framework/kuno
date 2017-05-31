/* 
 * Copyright (c) Stacks Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System;
using System.Collections;
using System.Reflection;
using System.Security.Claims;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Kuno.Serialization
{
    /// <summary>
    /// Overrides the default behavior of skipping over private members.
    /// </summary>
    /// <seealso cref="Newtonsoft.Json.Serialization.DefaultContractResolver" />
    public class DefaultContractResolver : BaseContractResolver
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
            var property = member as PropertyInfo;
            if (property != null)
            {
                if (!prop.Writable)
                {
                    var hasPrivateSetter = property.GetSetMethod(true) != null;
                    prop.Writable = hasPrivateSetter;
                }
                if (prop.PropertyType == typeof(ClaimsPrincipal))
                {
                    prop.Converter = new ClaimsPrincipalConverter();
                }
                var isDefaultValueIgnored = ((prop.DefaultValueHandling ?? DefaultValueHandling.Ignore) & DefaultValueHandling.Ignore) != 0;
                if (isDefaultValueIgnored)
                {
                    Predicate<object> newShouldSerialize = obj =>
                    {
                        var value = prop.ValueProvider.GetValue(obj);
                        if (value == null)
                        {
                            return false;
                        }
                        if (!typeof(string).IsAssignableFrom(property.PropertyType) && typeof(IEnumerable).IsAssignableFrom(property.PropertyType))
                        {
                            var collection = value as ICollection;
                            return collection == null || collection.Count != 0;
                        }
                        return true;
                    };

                    var oldShouldSerialize = prop.ShouldSerialize;
                    prop.ShouldSerialize = oldShouldSerialize != null ? o => oldShouldSerialize(o) && newShouldSerialize(o) : newShouldSerialize;
                }
            }

            return prop;
        }
    }
}