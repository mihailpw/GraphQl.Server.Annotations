using System;
using GraphQl.Server.Annotations.Providers;
using GraphQL.Types;

namespace GraphQl.Server.Annotations.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public class GraphQlTypeAttribute : GraphQlAttribute, IGraphTypeInfoProvider
    {
        public string Name { get; }
        public string Description { get; set; }
        public string DeprecationReason { get; set; }


        public GraphQlTypeAttribute(string name = null)
        {
            Name = name;
        }


        public void Provide(GraphType graphType, Type type, IServiceProvider serviceProvider)
        {
            graphType.Name = Name ?? type.Name;
            graphType.Description = Description;
            graphType.DeprecationReason = DeprecationReason;
        }
    }
}