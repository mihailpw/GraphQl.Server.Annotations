using System;
using System.Collections.Generic;

namespace GraphQl.Server.Annotations.Common
{
    internal interface IGraphTypeRegistry
    {
        bool IsRegistered(Type type);
        Type Resolve(Type type);
        bool TryResolve(Type type, out Type graphType);
        IEnumerable<Type> ResolveAdditional(Type type);
        IEnumerable<Type> ResolveAll();

        Type RegisterInputObject(Type type);
        Type RegisterObject(Type type);
        Type RegisterInterface(Type type);
        void DirectRegister(Type type, Type graphType);
    }
}