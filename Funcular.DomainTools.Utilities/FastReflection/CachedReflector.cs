using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Funcular.DomainTools.Utilities.FastReflection
{
    public class CachedReflector : BaseReflector
    {
        private readonly Dictionary<PointerPair, Attribute> _attributeCache = new Dictionary<PointerPair, Attribute>();
        private readonly Dictionary<IntPtr, PropertyInfo[]> _propertyCache = new Dictionary<IntPtr, PropertyInfo[]>();
        private readonly Dictionary<IntPtr, MemberInfo[]> _memberCache = new Dictionary<IntPtr, MemberInfo[]>();
        private readonly Dictionary<IntPtr, Func<object>> _constructorCache = new Dictionary<IntPtr, Func<object>>();
        private readonly Dictionary<IntPtr, Func<object, object>> _getterCache = new Dictionary<IntPtr, Func<object, object>>();
        private readonly Dictionary<IntPtr, Action<object, object>> _setterCache = new Dictionary<IntPtr, Action<object, object>>();

        private T GetSingleAttributeOrDefault<T>(PropertyInfo propertyInfo) where T : Attribute, new()
        {
            Type attributeType = typeof(T);
            Attribute attribute;
            var key = new PointerPair(propertyInfo.GetGetMethod().MethodHandle.Value, attributeType.TypeHandle.Value);
            if (!_attributeCache.TryGetValue(key, out attribute))
                _attributeCache.Add(key, attribute = base.GetSingleAttributeOrDefault<T>(propertyInfo));
            return attribute as T;
        }

        private T GetSingleAttributeOrDefault<T>(FieldInfo fieldInfo) where T : Attribute, new()
        {
            Type attributeType = typeof(T);
            Attribute attribute;
            var key = new PointerPair(fieldInfo.FieldHandle.Value, attributeType.TypeHandle.Value);
            if (!_attributeCache.TryGetValue(key, out attribute))
                _attributeCache.Add(key, attribute = base.GetSingleAttributeOrDefault<T>(fieldInfo));
            return attribute as T;
        }

        public override T GetSingleAttributeOrDefault<T>(MemberInfo memberInfo)
        {
            return memberInfo is PropertyInfo ?
                GetSingleAttributeOrDefault<T>((PropertyInfo) memberInfo) :
                GetSingleAttributeOrDefault<T>(memberInfo as FieldInfo);
        }

        public override IEnumerable<MemberInfo> GetSerializableMembers(Type type)
        {
            MemberInfo[] properties;
            if (!_memberCache.TryGetValue(type.TypeHandle.Value, out properties))
                _memberCache.Add(type.TypeHandle.Value, properties = base.GetSerializableMembers(type).ToArray());
            return properties;
        }

        /// <summary>
        /// Not implemented, DO NOT USE.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public override object Instantiate(Type type)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Not implemented, DO NOT USE.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public override object GetValue(MemberInfo member, object instance)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Not implemented, DO NOT USE.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <summary>
        /// Not implemented, DO NOT USE.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public override void SetValue(MemberInfo member, object instance, object value)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Not implemented, DO NOT USE.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public override MethodHandler GetDelegate(MethodBase method)
        {
            throw new NotImplementedException();
        }

        #region New methods

        public TInstance Instantiate<TInstance>()
        {
            Func<object> constructor;
            var intPtr = typeof(TInstance).TypeHandle.Value;
            if (!_constructorCache.TryGetValue(intPtr, out constructor))
                _constructorCache[intPtr] = EmitHelper.CreateParameterlessConstructorHandler(typeof(TInstance));
            return (TInstance) constructor();
        }

        public TMember GetPropertyValue<TInstance, TMember>(PropertyInfo propertyInfo, TInstance instance)
        {
            if (propertyInfo != null)
            {
                var key = propertyInfo.GetGetMethod().MethodHandle.Value;
                Func<object, object> getter;
                if (!_getterCache.TryGetValue(key, out getter))
                    {
                        getter = EmitHelper.CreatePropertyGetterHandler(propertyInfo);
                        _getterCache[key] = getter;
                    }

                if (getter == null)
                    return default(TMember);
                return (TMember)getter(instance);
            }
            throw new NotImplementedException();
        }

        public TMember GetFieldValue<TInstance, TMember>(MemberInfo memberInfo, TInstance instance)
        {
            var info = memberInfo as FieldInfo;
            if (info != null)
            {
                var fieldInfo = info;
                Func<object, object> getter;
                if (!_getterCache.TryGetValue(fieldInfo.FieldHandle.Value, out getter))
                    _getterCache[fieldInfo.FieldHandle.Value] = EmitHelper.CreateFieldGetterHandler(fieldInfo);
                return (TMember) getter(instance);
            }
            else
                throw new NotImplementedException();
        }

        public TMember SetProperty<TInstance, TMember>(PropertyInfo propertyInfo, TInstance instance, object value)
        {
            if (propertyInfo == null)
                throw new ArgumentNullException("propertyInfo", "propertyInfo cannot be null");
                //throw new ArgumentNullException(nameof(propertyInfo), $"{nameof(propertyInfo)} cannot be null");
            var key = propertyInfo.GetSetMethod().MethodHandle.Value;
            Action<object, object> setter;
            if (!_setterCache.TryGetValue(key, out setter))
            {
                setter = EmitHelper.CreatePropertySetterHandler(propertyInfo);
                _setterCache[key] = setter;
            }

            if (setter == null)
                return (TMember) value;
            if (value is DBNull)
            {
                // If you are getting exceptions here, someone probably used a 
                // non-nullable property for a nullable column. Change the property
                // in question to a nullable type and see if that doesn't clear
                // it up for you:
                setter(instance, (default(TMember)));
                return default(TMember);
            }
            else
            {
                setter(instance, (TMember)value);
                return (TMember)value;
            }
        }

        public TMember SetField<TInstance, TMember>(MemberInfo memberInfo, TInstance instance, TMember value)
        {
            var fieldInfo = memberInfo as FieldInfo;
            if (fieldInfo != null)
            {
                Action<object, object> setter;
                if (!_setterCache.TryGetValue(fieldInfo.FieldHandle.Value, out setter))
                    _setterCache[fieldInfo.FieldHandle.Value] = EmitHelper.CreateFieldSetterHandler(fieldInfo);
                setter(instance, value);
                return value;
            }
            throw new NotImplementedException();
        }

        #endregion

/*        public override MethodHandler GetDelegate(MethodBase method)
        {
            MethodHandler handler;
            var key = method.MethodHandle.Value;
            if (!_methodCache.TryGetValue(key, out handler))
                _methodCache.Add(key, handler = EmitHelper.CreateMethodHandler(method));
            return handler;
        }*/
    }
}
