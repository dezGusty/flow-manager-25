using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FlowManager.Infrastructure.Utils
{
    public static class PatchHelper
    {
        public static void PatchFrom<TDto, TEntity>(this TEntity entity, TDto dto)
        {
            var entityType = entity.GetType();
            var dtoType = dto.GetType();

            var dtoProperties = dtoType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in dtoProperties)
            {
                if (!property.CanRead)
                    continue;

                var entityProperty = entityType.GetProperty(property.Name, BindingFlags.Public | BindingFlags.Instance);

                if (entityProperty == null || !entityProperty.CanWrite)
                    continue;

                var dtoValue = property.GetValue(dto);
                if (!ShouldPatchValue(dtoValue, property.GetType()))
                    continue;

                if (dtoValue != null)
                {
                    var targetType = entityProperty.PropertyType;

                    if (IsDictionaryType(targetType))
                    {
                        PatchDictionary(entity, entityProperty, dtoValue);
                    }
                    else
                    {
                        if (IsNullableType(targetType))
                        {
                            targetType = Nullable.GetUnderlyingType(targetType);
                        }

                        var convertedValue = Convert.ChangeType(dtoValue, targetType);
                        entityProperty.SetValue(entity, convertedValue);
                    }
                }
            }
        }

        private static void PatchDictionary(object entity, PropertyInfo entityProperty, object dtoValue)
        {
            var entityDictionary = entityProperty?.GetValue(entity) as IDictionary;
            var dtoDictionary = dtoValue as IDictionary;

            if (dtoDictionary == null)
                return;

            if (entityDictionary == null)
            {
                var dictionaryType = entityProperty.PropertyType;
                if (dictionaryType.IsInterface)
                {
                    var genericArgs = dictionaryType.GetGenericArguments();
                    var concreteType = typeof(Dictionary<,>).MakeGenericType(genericArgs);
                    entityDictionary = Activator.CreateInstance(concreteType) as IDictionary;
                }
                else
                {
                    entityDictionary = Activator.CreateInstance(dictionaryType) as IDictionary;
                }
                entityProperty.SetValue(entity, entityDictionary);
            }

            foreach (DictionaryEntry entry in dtoDictionary)
            {
                var key = entry.Key;
                var newValue = entry.Value;

                if (!entityDictionary.Contains(key) ||
                    !AreValuesEqual(entityDictionary[key], newValue))
                {
                    entityDictionary[key] = newValue;
                }
            }
        }

        private static bool AreValuesEqual(object existingValue, object newValue)
        {
            if (existingValue == null && newValue == null)
                return true;

            if (existingValue == null || newValue == null)
                return false;

            return existingValue.Equals(newValue);
        }

        private static bool IsDictionaryType(Type type)
        {
            return typeof(IDictionary).IsAssignableFrom(type) ||
                   (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>)) ||
                   (type.IsGenericType && type.GetInterfaces().Any(x =>
                       x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IDictionary<,>)));
        }

        private static bool ShouldPatchValue(object value, Type propertyType)
        {
            if (propertyType == typeof(bool) || propertyType == typeof(bool?))
                return true;

            if (value == null)
                return false;

            if (value is string str && string.IsNullOrWhiteSpace(str))
                return false;

            if (propertyType.IsValueType && !IsNullableType(propertyType))
            {
                var defaultValue = Activator.CreateInstance(propertyType);
                if (value.Equals(defaultValue))
                    return false;
            }

            return true;
        }

        private static bool IsNullableType(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }
    }
}
