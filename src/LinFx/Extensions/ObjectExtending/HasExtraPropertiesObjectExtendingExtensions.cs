using JetBrains.Annotations;
using Volo.Abp.ObjectExtending;

namespace LinFx.Extensions.ObjectExtending
{
    public static class HasExtraPropertiesObjectExtendingExtensions
    {
        /// <summary>
        /// Copies extra properties from the <paramref name="source"/> object
        /// to the <paramref name="destination"/> object.
        /// 
        /// Checks property definitions (over the <see cref="ObjectExtensionManager"/>)
        /// based on the <paramref name="definitionChecks"/> preference.
        /// </summary>
        /// <typeparam name="TSource">Source class type</typeparam>
        /// <typeparam name="TDestination">Destination class type</typeparam>
        /// <param name="source">The source object</param>
        /// <param name="destination">The destination object</param>
        /// <param name="definitionChecks">
        ///     Controls which properties to map.
        /// </param>
        /// <param name="ignoredProperties">Used to ignore some properties</param>
        public static void MapExtraPropertiesTo<TSource, TDestination>(
            [NotNull] this TSource source,
            [NotNull] TDestination destination,
            MappingPropertyDefinitionChecks? definitionChecks = null,
            string[] ignoredProperties = null)
            where TSource : IHasExtraProperties
            where TDestination : IHasExtraProperties
        {
            ExtensibleObjectMapper.MapExtraPropertiesTo(
                source,
                destination,
                definitionChecks,
                ignoredProperties
            );
        }
    }
}