using System;

namespace GraphQl.Server.Annotations.TypeResolvers
{
    public interface ITypePreparer
    {
        Type PrepareType(Type type);
    }
}