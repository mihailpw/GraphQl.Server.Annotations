﻿using System;

namespace GraphQl.Server.Annotations.Common.Helpers
{
    internal static class TypeExtensions
    {
        public static bool IsGenericTypeDefinition(this Type type, Type genericType)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == genericType;
        }
    }
}