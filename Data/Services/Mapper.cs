using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Data.Services
{
    public class Mapper
    {
        /// <summary>
        /// Map from source to destination
        /// <para>Returns destination</para>
        /// </summary>
        public static object Map(object source, object destination)
        {
            var sourceProperties = GetProperties(source, p => IsSourceProperty(p));
            var destinationProperties = GetProperties(destination, IsDestinationProperty);

            Map(source, destination, sourceProperties, destinationProperties);

            return destination;
        }

        protected static void Map(object source, object destination, IEnumerable<PropertyInfo> sourceProperties, IList<PropertyInfo> destinationProperties)
        {
            foreach (var sourceProperty in sourceProperties)
            {
                var destinationProperty = GetMatching(sourceProperty, destinationProperties);
                if (destinationProperty == null) continue;
                destinationProperty.SetValue(destination, sourceProperty.GetValue(source));
            }
        }

        protected static Func<PropertyInfo, bool> IsSourceProperty => (p) => p.CanRead && IsMappable(p.PropertyType);
        protected static Func<PropertyInfo, bool> IsDestinationProperty => (p) => p.CanWrite;

        private static bool IsMappable(Type type)
        {
            while (true)
            {
                if (!type.IsGenericType || type.GetGenericTypeDefinition() != Types.Nullable)
                    return type == Types.String || type == Types.ValueType;

                type = type.GetGenericArguments().First();
            }
        }

        protected static IList<PropertyInfo> GetProperties(object item, Func<PropertyInfo, bool> filter)
            => item.GetType().GetProperties().Where(filter).ToList();

        private static PropertyInfo GetMatching(PropertyInfo desiredProperty, IEnumerable<PropertyInfo> properties)
            => properties.FirstOrDefault(p => p.Name == desiredProperty.Name && desiredProperty.PropertyType.IsAssignableFrom(p.PropertyType));
    }

    public class Mapper<TType> : Mapper
    {
        private readonly PropertyInfo[] _baseProperties;

        public Mapper()
        {
            _baseProperties = typeof(TType).GetProperties().ToArray();
        }

        /// <summary>
        /// Map from source to destination
        /// <para>Returns destination</para>
        /// </summary>
        public TType Map(object source, TType destination)
        {
            Map(source, destination, GetProperties(source, IsSourceProperty), _baseProperties.Where(IsDestinationProperty).ToList());
            return destination;
        }

        /// <summary>
        /// Map from source to destination
        /// <para>Returns destination</para>
        /// </summary>
        public object Map(TType source, object destination)
        {
            Map(source, destination, _baseProperties.Where(IsSourceProperty), GetProperties(destination, IsDestinationProperty));
            return destination;
        }

        /// <summary>
        /// Map from source to destination
        /// <para>Returns destination</para>
        /// </summary>
        public TType Map(TType source, TType destination)
        {
            Map(source, destination, _baseProperties.Where(IsSourceProperty), _baseProperties.Where(IsDestinationProperty).ToList());
            return destination;
        }
    }
}