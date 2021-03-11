using Mapster;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace Managix.Infrastructure
{
    /// <summary>
    /// Mapper
    /// </summary>
    public class MapperService : IMapperService
    {
        private static TypeAdapterConfig TopTypeAdapterConfig = TypeAdapterConfig.GlobalSettings.Clone();

        static MapperService()
        {
            TopTypeAdapterConfig.Default.IgnoreMember((model, side) =>
            {
                if (model.Type.IsValueType || model.Type == typeof(string))
                {
                    return false;
                }
                return true;
            });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public TDestination Map<TDestination>(object source)
        {
            return source.Adapt<TDestination>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public TDestination Map<TSource, TDestination>(TSource source)
        {
            if (source == null)
            {
                return default;
            }
            return source.Adapt<TSource, TDestination>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public List<TDestination> MapList<TSource, TDestination>(IEnumerable<TSource> source)
        {
            if (source == null)
            {
                return null;
            }

            var dest = new List<TDestination>();
            foreach (var s in source)
            {
                dest.Add(Map<TDestination>(s));
            }
            return dest;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            if (source == null)
            {
                return destination;
            }
            if (destination == null)
            {
                return default;
            }

            return source.Adapt(destination);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        public TDestination MapTop<TSource, TDestination>(TSource source, TDestination destination)
        {
            if (source == null)
            {
                return destination;
            }
            if (destination == null)
            {
                return default;
            }
            return source.Adapt(destination, TopTypeAdapterConfig);
        }
    }

  

}
