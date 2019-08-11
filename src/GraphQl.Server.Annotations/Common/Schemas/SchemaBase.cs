using System.Linq;
using GraphQL.Types;

namespace GraphQl.Server.Annotations.Common.Schemas
{
    internal abstract class SchemaBase : Schema
    {
        private readonly IGraphTypeRegistry _typeRegistry;


        protected SchemaBase(IGraphTypeRegistry typeRegistry)
        {
            _typeRegistry = typeRegistry;
        }


        protected void PopulateAdditionalTypes()
        {
            var registerTypeMethodInfo = GetType().GetMethods().First(mi => mi.Name == nameof(RegisterType) && mi.IsGenericMethod);
            foreach (var graphQlType in _typeRegistry.ResolveAll())
            {
                if (!AllTypes.Any(it => graphQlType.IsInstanceOfType(it)))
                {
                    registerTypeMethodInfo.MakeGenericMethod(graphQlType).Invoke(this, new object[0]);
                }
            }
        }
    }
}