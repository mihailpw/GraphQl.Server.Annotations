using System;
using System.Collections.Generic;
using GraphQL;
using GraphQl.Server.Annotations.Common.Types;
using GraphQL.Types;

namespace GraphQl.Server.Annotations.Common
{
    internal class GraphTypeRegistry : IGraphTypeRegistry
    {
        private readonly Dictionary<Type, Type> _typeToGraphTypeMap;


        public GraphTypeRegistry()
        {
            _typeToGraphTypeMap = new Dictionary<Type, Type>
            {
                [typeof(bool)] = typeof(BooleanGraphType),
                [typeof(byte)] = typeof(ByteGraphType),
                [typeof(DateTime)] = typeof(DateTimeGraphType),
                [typeof(DateTimeOffset)] = typeof(DateTimeOffsetGraphType),
                [typeof(decimal)] = typeof(DecimalGraphType),
                [typeof(float)] = typeof(FloatGraphType),
                [typeof(Guid)] = typeof(GuidGraphType),
                [typeof(long)] = typeof(LongGraphType),
                [typeof(int)] = typeof(IntGraphType),
                [typeof(sbyte)] = typeof(SByteGraphType),
                [typeof(short)] = typeof(ShortGraphType),
                [typeof(string)] = typeof(StringGraphType),
                [typeof(TimeSpan)] = typeof(TimeSpanMillisecondsGraphType),
                [typeof(uint)] = typeof(UIntGraphType),
                [typeof(ulong)] = typeof(ULongGraphType),
                [typeof(ushort)] = typeof(UShortGraphType),
                [typeof(Uri)] = typeof(UriGraphType),
            };
        }


        public bool IsRegistered(Type type)
        {
            return _typeToGraphTypeMap.ContainsKey(type);
        }

        public Type Resolve(Type type)
        {
            if (TryResolve(type, out var graphQlType))
                return graphQlType;
            else
                throw new InvalidOperationException($"Type {type.Name} is not registered.");
        }

        public bool TryResolve(Type type, out Type graphType)
        {
            return _typeToGraphTypeMap.TryGetValue(type, out graphType);
        }

        public IEnumerable<Type> ResolveAdditional(Type type)
        {
            var graphQlType = Resolve(type);
            foreach (var typeToGraphQlType in _typeToGraphTypeMap)
            {
                if (typeToGraphQlType.Value == graphQlType
                    && typeToGraphQlType.Key != type)
                {
                    yield return typeToGraphQlType.Key;
                }
            }
        }

        public IEnumerable<Type> ResolveAll()
        {
            return _typeToGraphTypeMap.Values;
        }

        public Type RegisterInputObject(Type type)
        {
            return Register(type, typeof(AutoInputObjectGraphType<>).MakeGenericType(type));
        }

        public Type RegisterObject(Type type)
        {
            return Register(type, typeof(AutoObjectGraphType<>).MakeGenericType(type));
        }

        public Type RegisterInterface(Type type)
        {
            return Register(type, typeof(AutoInterfaceGraphType<>).MakeGenericType(type));
        }

        public void DirectRegister(Type type, Type graphType)
        {
            if (!graphType.IsGraphType())
            {
                throw new ArgumentException($"Invalid GraphQL type provided (graphType={graphType.Name}.", nameof(graphType));
            }
            Register(type, graphType);
        }


        private Type Register(Type key, Type value)
        {
            if (_typeToGraphTypeMap.TryGetValue(key, out var existingValue))
            {
                if (value == existingValue)
                    return existingValue;

                throw new ArgumentException($"Type {key.Name} already registered.");
            }

            _typeToGraphTypeMap.Add(key, value);

            return value;
        }
    }
}