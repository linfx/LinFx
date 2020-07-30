using EmitMapper;
using EmitMapper.MappingConfiguration;
using EmitMapper.MappingConfiguration.MappingOperations;
using System;

namespace LinFx.Extensions.ObjectMapping
{
    public class MapBuilder<TSource>
    {
        private DefaultMapConfig _config = DefaultMapConfig.Instance;
        private readonly TSource source;

        public MapBuilder(TSource source)
        {
            this.source = source;
        }

        public TDestination To<TDestination>()
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<TSource, TDestination>(_config).Map(source);
        }

        /// <summary>
        /// Define custom type converter
        /// </summary>
        /// <typeparam name="From">Source type</typeparam>
        /// <typeparam name="To">Destination type</typeparam>
        /// <param name="converter">Function which converts an inctance of the source type to an instance of the destination type</param>
        /// <returns></returns>
        public MapBuilder<TSource> ConvertUsing<From, To>(Func<From, To> converter)
        {
            _config = _config.ConvertUsing(converter);
            return this;
        }

        /// <summary>
        /// Define members which should be ingored
        /// </summary>
        /// <param name="typeFrom">Source type for which ignore members are defining</param>
        /// <param name="typeTo">Destination type for which ignore members are defining</param>
        /// <param name="ignoreNames">Array of member names which should be ingored</param>
        /// <returns></returns>
        public MapBuilder<TSource> IgnoreMembers(Type typeFrom, Type typeTo, string[] ignoreNames)
        {
            _config = _config.IgnoreMembers(typeFrom, typeTo, ignoreNames);
            return this;
        }

        /// <summary>
        /// Define members which should be ingored
        /// </summary>
        /// <typeparam name="TFrom">Source type for which ignore members are defining</typeparam>
        /// <typeparam name="TTo">Destination type for which ignore members are defining</typeparam>
        /// <param name="ignoreNames">Array of member names which should be ingored</param>
        /// <returns></returns>
        public MapBuilder<TSource> IgnoreMembers<TFrom, TTo>(string[] ignoreNames)
        {
            _config = _config.IgnoreMembers<TFrom, TTo>(ignoreNames);
            return this;
        }

        public MapBuilder<TSource> PostProcess<T>(ValuesPostProcessor<T> postProcessor)
        {
            //var vpp = new ValuesPostProcessor<T>((t1, t2) =>
            //{
            //    return postProcessor(t1);
            //});

            _config = _config.PostProcess(postProcessor);
            return this;
        }
    }
}