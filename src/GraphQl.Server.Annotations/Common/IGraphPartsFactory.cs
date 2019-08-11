using System;
using System.Reflection;
using GraphQL.Resolvers;
using GraphQL.Types;

namespace GraphQl.Server.Annotations.Common
{
    internal interface IGraphPartsFactory
    {
        FieldType CreateFieldType(PropertyInfo propertyInfo, IFieldResolver fieldResolver = null);
        FieldType CreateFieldType(MethodInfo methodInfo, IFieldResolver fieldResolver = null);
        Type CreateInterfaceType(Type interfaceType);
        Func<object, bool> CreateIsTypeOfFunc(Type type);
    }
}