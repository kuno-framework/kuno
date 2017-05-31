using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Kuno.Reflection;
using Kuno.Text;
using Kuno.Validation;

namespace Kuno.Services.OpenApi
{
    /// <summary>
    /// A collection of OpenAPI schema defintinions.
    /// </summary>
    public class SchemaCollection : SortedDictionary<string, Schema>
    {
        /// <summary>
        /// Gets or adds a shema for the specified type.
        /// </summary>
        /// <param name="type">The type to use for the schema.</param>
        /// <param name="description">The description for the schema.</param>
        /// <returns>
        /// Returns the existing or added schema.
        /// </returns>
        public Schema GetOrAdd(Type type, string description = null)
        {
            if (type == null)
            {
                return null;
            }

            if (type.IsNullable())
            {
                type = Nullable.GetUnderlyingType(type);
            }

            if (type.IsPrimitive() || type == typeof(object))
            {
                return this.CreatePrimitiveSchema(type);
            }

            if (type.IsDictionary())
            {
                return this.CreateDictionarySchema(type, description);
            }

            if (typeof(IEnumerable).IsAssignableFrom(type) || type.IsArray)
            {
                return this.CreateArraySchema(type, description);
            }

            var key = GetKey(type);

            if (!this.ContainsKey(key))
            {
                this.Add(key, null);
                this[key] = this.CreateSchema(type, description);
            }

            return this[key];
        }

        internal Schema CreatePrimitiveSchema(Type type, string description = null)
        {
            if (type == typeof(bool))
            {
                return new Schema { Type = "boolean", Description = description };
            }
            if (type == typeof(Guid))
            {
                return new Schema { Type = "string", Format = "uuid", Description = description };
            }
            if (type == typeof(DateTime))
            {
                return new Schema { Type = "string", Format = "date-time", Description = description };
            }
            if (type == typeof(DateTimeOffset))
            {
                return new Schema { Type = "string", Format = "date-time", Description = description };
            }
            if (type == typeof(TimeSpan))
            {
                return new Schema { Type = "string", Example = TimeSpan.FromSeconds(32), Description = description };
            }
            if (type == typeof(char))
            {
                return new Schema { Type = "string", Description = description };
            }
            if (type == typeof(int) || type == typeof(uint) || type == typeof(short) || type == typeof(ushort))
            {
                return new Schema { Type = "integer", Format = "int32", Description = description };
            }
            if (type == typeof(long) || type == typeof(ulong))
            {
                return new Schema { Type = "integer", Format = "int64", Description = description };
            }
            if (type == typeof(float))
            {
                return new Schema { Type = "number", Format = "float", Description = description };
            }
            if (type == typeof(double) || type == typeof(decimal))
            {
                return new Schema { Type = "number", Format = "double", Description = description };
            }
            if (type.IsNullable())
            {
                return this.CreatePrimitiveSchema(Nullable.GetUnderlyingType(type), description);
            }
            return new Schema { Type = "string", Description = description };
        }

        /// <summary>
        /// Gets the schema key for the specified type.
        /// </summary>
        /// <param name="type">The schema key for the specified type.</param>
        /// <returns></returns>
        internal static string GetKey(Type type)
        {
            return type.FullName.ToCamelCase();
        }

        internal Schema GetReferenceSchema(Type type, string description)
        {
            if (type.IsNullable())
            {
                type = Nullable.GetUnderlyingType(type);
            }

            if (type.IsPrimitive() || type == typeof(object))
            {
                return this.CreatePrimitiveSchema(type);
            }
            if (type.IsDictionary())
            {
                return this.CreateDictionarySchema(type, description);
            }
            if (type.IsArray)
            {
                return this.CreateArraySchema(type.GetElementType(), description);
            }
            if (typeof(IEnumerable).IsAssignableFrom(type))
            {
                if (type.GetTypeInfo().IsInterface)
                {
                    return this.CreateArraySchema(type.GetGenericArguments()[0], description);
                }
                var target = type.GetInterfaces().First(e => e.GetTypeInfo().IsGenericType && e.GetGenericTypeDefinition() == typeof(IEnumerable<>));
                return this.CreateArraySchema(target.GetGenericArguments()[0], description);
            }
            this.GetOrAdd(type);
            return this.CreateReferenceSchema(type, description);
        }

        private Schema CreateArraySchema(Type type, string description)
        {
            if (type.IsArray)
            {
                return this.CreateArraySchema(type.GetElementType(), description);
            }
            if (typeof(IEnumerable).IsAssignableFrom(type) && type != typeof(string))
            {
                if (type.GetTypeInfo().IsInterface)
                {
                    return this.CreateArraySchema(type.GetGenericArguments()[0], description);
                }
                var target = type.GetInterfaces().First(e => e.GetTypeInfo().IsGenericType && e.GetGenericTypeDefinition() == typeof(IEnumerable<>));
                return this.CreateArraySchema(target.GetGenericArguments()[0], description);
            }

            this.GetOrAdd(type);

            return new Schema
            {
                Type = "array",
                Items = type.IsPrimitive() || type == typeof(object) ? this.CreatePrimitiveSchema(type) : this.CreateReferenceSchema(type),
                Description = description
            };
        }

        private Schema CreateDictionarySchema(Type type, string description = null)
        {
            var valueType = type.GetInterfaces().FirstOrDefault(e => e.GetTypeInfo().IsGenericType && e.GetGenericTypeDefinition() == typeof(IDictionary<,>))?.GetGenericArguments().ElementAt(1);
            this.GetOrAdd(valueType);
            if (valueType != null)
            {
                return new Schema
                {
                    Type = "object",
                    AdditionalProperties = valueType.IsPrimitive() || valueType == typeof(object) ? this.CreatePrimitiveSchema(valueType) : this.CreateReferenceSchema(valueType, description),
                    Description = description
                };
            }
            return new Schema
            {
                Type = "object",
                Description = description
            };
        }

        private Schema CreateEnumSchema(Type type, string description = null)
        {
            return new Schema
            {
                Type = "string",
                Enum = Enum.GetNames(type).Select(name => name.ToCamelCase()).ToArray(),
                Description = description ?? type.GetComments()?.Summary
            };
        }

        private Schema CreateObjectSchema(Type type, string description = null)
        {
            var schema = new Schema
            {
                Type = "object",
                Description = description ?? type.GetComments()?.Summary
            };

            var required = new List<string>();
            foreach (var property in type.GetProperties())
            {
                schema.Properties.Add(property.Name, this.GetReferenceSchema(property.PropertyType, description));
                if (property.GetCustomAttributes<ValidationAttribute>(true).Any())
                {
                    required.Add(property.Name.ToCamelCase());
                }
            }
            if (required.Any())
            {
                schema.Required = required;
            }
            return schema;
        }

        private Schema CreateReferenceSchema(Type type, string description = null)
        {
            return new Schema
            {
                Ref = $"#/definitions/{GetKey(type)}",
                Description = description
            };
        }

        private Schema CreateSchema(Type type, string description = null)
        {
            if (type.IsNullable())
            {
                return this.CreateSchema(Nullable.GetUnderlyingType(type), description);
            }
            if (type.GetTypeInfo().IsEnum)
            {
                return this.CreateEnumSchema(type, description);
            }
            if (type.IsPrimitive())
            {
                return this.CreatePrimitiveSchema(type, description);
            }
            if (type.IsDictionary())
            {
                return this.CreateDictionarySchema(type, description);
            }
            if (type.IsArray || typeof(IEnumerable).IsAssignableFrom(type))
            {
                return this.CreateArraySchema(type, description);
            }
            return this.CreateObjectSchema(type, description);
        }
    }
}