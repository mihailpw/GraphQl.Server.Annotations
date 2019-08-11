using System;

namespace GraphQl.Server.Annotations.Common.Helpers
{
    internal static class GenericExtensions
    {
        public static T VerifyNotNull<T>(this T target, string argumentName) where T : class
        {
            return target ?? throw new ArgumentNullException(argumentName);
        }
    }
}