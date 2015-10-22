namespace EA.Iws.TestHelpers.Helpers
{
    using System;
    using System.Collections.Generic;
    using Prsd.Core.Mapper;

    public class TestMapper : IMapper
    {
        private readonly Dictionary<Type, object> mappers;

        public TestMapper()
        {
            mappers = new Dictionary<Type, object>();
        }

        public TResult Map<TResult>(object source)
        {
            var sourceType = source.GetType();

            if (sourceType.Namespace.Contains("DomainFakes"))
            {
                sourceType = sourceType.BaseType;
            }

            var mappingInterface = MakeMappingInterface<TResult>(sourceType);

            object mapper;
            if (mappers.TryGetValue(mappingInterface, out mapper))
            {
                return InvokeMapper<TResult>(mapper, source);
            }

            throw new InvalidOperationException(string.Format(
                "No matching mapper for interface IMap<{0}, {1}> found in TestMap dictionary",
                mappingInterface.GenericTypeArguments[0].Name,
                mappingInterface.GenericTypeArguments[1].Name));
        }

        public TResult Map<TResult>(object source, object parameter)
        {
            object mapper;
            var sourceType = source.GetType();
            var parameterType = parameter.GetType();

            if (sourceType.Namespace.Contains("DomainFakes"))
            {
                sourceType = sourceType.BaseType;
            }

            if (parameterType.Namespace.Contains("DomainFakes"))
            {
                parameterType = parameterType.BaseType;
            }

            var mappingInterface = MakeMappingInterface<TResult>(sourceType, parameterType);

            if (mappers.TryGetValue(mappingInterface, out mapper))
            {
                return InvokeMapper<TResult>(mapper, source, parameter);
            }

            throw new InvalidOperationException(string.Format(
                "No matching mapper for interface IMapWithParameter<{0}, {1}, {2}> found in TestMap dictionary",
                mappingInterface.GenericTypeArguments[0].Name,
                mappingInterface.GenericTypeArguments[1].Name,
                mappingInterface.GenericTypeArguments[2].Name));
        }

        public TResult Map<TSource, TResult>(TSource source)
        {
            object result;
            if (mappers.TryGetValue(typeof(IMap<TSource, TResult>), out result))
            {
                var mapper = result as IMap<TSource, TResult>;

                return mapper.Map(source);
            }

            throw new InvalidOperationException(string.Format(
                "No matching mapper for interface IMap<{0}, {1}> found in TestMap dictionary",
                typeof(TSource).Name, typeof(TResult).Name));
        }

        public TResult Map<TSource, TParameter, TResult>(TSource source, TParameter parameter)
        {
            object result;
            if (mappers.TryGetValue(typeof(IMapWithParameter<TSource, TParameter, TResult>), out result))
            {
                var mapper = result as IMapWithParameter<TSource, TParameter, TResult>;

                return mapper.Map(source, parameter);
            }

            throw new InvalidOperationException(string.Format(
                "No matching mapper found for interface IMapWithParameter<{0}, {1}, {2}> found in TestMap dictionary",
                typeof(TSource).Name, typeof(TParameter).Name, typeof(TResult).Name));
        }

        public void AddMapper<TSource, TTarget>(IMap<TSource, TTarget> mapper)
        {
            mappers.Add(MakeMappingInterface<TTarget>(typeof(TSource)), mapper);
        }

        public void AddMapper<TSource, TParameter, TTarget>(IMapWithParameter<TSource, TParameter, TTarget> mapper)
        {
            mappers.Add(MakeMappingInterface<TTarget>(typeof(TSource), typeof(TParameter)), mapper);
        }

        private static Type MakeMappingInterface<TResult>(Type sourceType)
        {
            return typeof(IMap<,>).MakeGenericType(sourceType, typeof(TResult));
        }

        private static Type MakeMappingInterface<TResult>(Type sourceType, Type parameterType)
        {
            return typeof(IMapWithParameter<,,>).MakeGenericType(sourceType, parameterType, typeof(TResult));
        }

        private static TResult InvokeMapper<TResult>(object mapper, object source)
        {
            return (TResult)mapper
                .GetType()
                .GetMethod("Map", new[] { source.GetType() })
                .Invoke(mapper, new[] { source });
        }

        private static TResult InvokeMapper<TResult>(object mapper, object source, object parameter)
        {
            return (TResult)mapper
                .GetType()
                .GetMethod("Map", new[] { source.GetType(), parameter.GetType() })
                .Invoke(mapper, new[] { source, parameter });
        }
    }
}
