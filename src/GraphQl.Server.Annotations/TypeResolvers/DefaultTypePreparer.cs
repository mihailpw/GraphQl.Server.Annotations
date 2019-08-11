using System;
using GraphQl.Server.Annotations.Common.Helpers;

namespace GraphQl.Server.Annotations.TypeResolvers
{
    public class DefaultTypePreparer : ITypePreparer
    {
        public Type PrepareType(Type type)
        {
            var currentType = type;
            while (true)
            {
                if (TypeUtils.Task.IsInType(currentType))
                    currentType = TypeUtils.Task.UnwrapType(currentType);
                else if (TypeUtils.Nullable.IsInType(currentType))
                    currentType = TypeUtils.Nullable.UnwrapType(currentType);
                else
                    return currentType;
            }
        }
    }
}