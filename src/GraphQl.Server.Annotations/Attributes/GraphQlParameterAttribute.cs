using System;
using System.Reflection;
using GraphQl.Server.Annotations.Providers;
using GraphQL.Types;

namespace GraphQl.Server.Annotations.Attributes
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class GraphQlParameterAttribute : GraphQlAttribute, INameProvider, IQueryArgumentInfoProvider
    {
        public string Name { get; }
        public string Description { get; set; }
        public object DefaultValue { get; set; }


        public GraphQlParameterAttribute(string name = null)
        {
            Name = name;
        }


        public void Provide(QueryArgument queryArgument, ParameterInfo parameterInfo, IServiceProvider serviceProvider)
        {
            queryArgument.Name = Name ?? parameterInfo.Name;
            queryArgument.Description = Description;
            if (DefaultValue != null)
            {
                queryArgument.DefaultValue = DefaultValue;
            }
        }
    }
}