using System.Linq;
using System.Reflection;
using GraphQl.Server.Annotations.Attributes;
using GraphQl.Server.Annotations.Providers;

namespace GraphQl.Server.Annotations.Common.Helpers
{
    internal static class CustomAttributeProviderExtensions
    {
        public static string GetNameOrDefault(this ICustomAttributeProvider attributeProvider, string defaultName)
        {
            return attributeProvider.FindInAttributes<INameProvider>()?.Name ?? defaultName;
        }

        public static bool IsGraphQlMember(this ICustomAttributeProvider attributeProvider)
        {
            return attributeProvider.FindInAttributes<GraphQlAttribute>() != null;
        }

        public static T FindInAttributes<T>(this ICustomAttributeProvider attributeProvider, bool inherit = false)
        {
            return attributeProvider.GetCustomAttributes(inherit).OfType<T>().FirstOrDefault();
        }
    }
}