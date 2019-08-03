using System;
using System.Reflection;
using GraphQL.Types;

namespace GraphQl.Server.Annotations.Providers
{
    public interface IFieldTypeInfoProvider
    {
        void Provide(FieldType fieldType, PropertyInfo propertyInfo, IServiceProvider serviceProvider);
        void Provide(FieldType fieldType, MethodInfo methodInfo, IServiceProvider serviceProvider);
    }
}