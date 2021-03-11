using System;
using System.Collections.Generic;
using System.Text;

namespace Managix.Infrastructure
{
    /// <summary>
    /// Defines a simple interface to automatically map objects.
    /// </summary>
    public interface IMapperService
    {
        ///// <summary>
        ///// Gets the underlying <see cref="IAutoObjectMappingProvider"/> object that is used for auto object mapping.
        ///// </summary>
        //IAutoObjectMappingProvider AutoObjectMappingProvider { get; }

        /// <summary>
        /// Converts an object to another. Creates a new object of TDestination.
        /// </summary>
        /// <typeparam name="TDestination">Type of the destination object</typeparam>
        /// <param name="source">Source object</param>
        TDestination Map<TDestination>(object source);

        /// <summary>
        /// Converts an object to another. Creates a new object of TDestination.
        /// </summary>
        /// <typeparam name="TDestination">Type of the destination object</typeparam>
        /// <typeparam name="TSource">Type of the source object</typeparam>
        /// <param name="source">Source object</param>
        TDestination Map<TSource, TDestination>(TSource source);

        /// <summary>
        /// Converts an object list to another. Creates a new object list of TDestination.
        /// </summary>
        /// <typeparam name="TDestination">Type of the destination object</typeparam>
        /// <typeparam name="TSource">Type of the source object</typeparam>
        /// <param name="source">Source object llist</param>
        List<TDestination> MapList<TSource, TDestination>(IEnumerable<TSource> source);

        /// <summary>
        /// Execute a mapping from the source object to the existing destination object
        /// </summary>
        /// <typeparam name="TSource">Source type</typeparam>
        /// <typeparam name="TDestination">Destination type</typeparam>
        /// <param name="source">Source object</param>
        /// <param name="destination">Destination object</param>
        /// <returns>Returns the same destination object after mapping operation</returns>
        TDestination Map<TSource, TDestination>(TSource source, TDestination destination);

        /// <summary>
        /// Execute a mapping from the source object preserve reference to the existing destination object
        /// </summary>
        /// <typeparam name="TSource">Source type</typeparam>
        /// <typeparam name="TDestination">Destination type</typeparam>
        /// <param name="source">Source object</param>
        /// <param name="destination">Destination object</param>
        /// <returns>Returns the same destination object after mapping operation</returns>
        TDestination MapTop<TSource, TDestination>(TSource source, TDestination destination);
    }
}
