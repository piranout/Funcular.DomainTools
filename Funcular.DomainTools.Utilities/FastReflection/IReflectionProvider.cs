using System;
using System.Collections.Generic;
using System.Reflection;

namespace Funcular.DomainTools.Utilities.FastReflection
{
    interface IReflectionProvider
    {
        T GetSingleAttributeOrDefault<T>(MemberInfo memberInfo) where T : Attribute, new();
        IEnumerable<MemberInfo> GetSerializableMembers(Type type);
        object Instantiate(Type type);
        object GetValue(MemberInfo member, object instance);
        void SetValue(MemberInfo member, object instance, object value);
        MethodHandler GetDelegate(MethodBase method);
    }
}
