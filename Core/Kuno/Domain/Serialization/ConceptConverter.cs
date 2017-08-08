/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System;
using System.Reflection;
using Newtonsoft.Json;

// ReSharper disable PossibleNullReferenceException

namespace Kuno.Domain.Serialization
{
    /// <summary>
    /// A <see cref="ConceptAs{TValue}" /> JSON converter that flattens and unflattens values.
    /// </summary>
    /// <seealso cref="Newtonsoft.Json.JsonConverter" />
    public class ConceptConverter : JsonConverter
    {
        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">EndPointType of the object.</param>
        /// <returns><c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.</returns>
        public override bool CanConvert(Type objectType)
        {
            var typeInfo = objectType.GetTypeInfo();

            return typeInfo.IsGenericType
                   && typeInfo.GetGenericTypeDefinition() == typeof(ConceptAs<>);
        }

        /// <summary>
        /// Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader">The <see cref="T:Newtonsoft.Json.JsonReader" /> to read from.</param>
        /// <param name="objectType">EndPointType of the object.</param>
        /// <param name="existingValue">The existing value of object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>The object value.</returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var valueType = objectType.GetTypeInfo().BaseType.GetGenericArguments()[0];

            var value = serializer.Deserialize(reader, valueType);

            var instance = Activator.CreateInstance(objectType);

            objectType.GetProperty("Value").SetValue(instance, value);

            return instance;
        }

        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter" /> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var inner = value.GetType().GetProperty("Value").GetValue(value);

            serializer.Serialize(writer, inner);
        }
    }
}