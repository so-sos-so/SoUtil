using System;
using System.Collections.Generic;
using System.Reflection;
using Sirenix.Utilities;
using Tool;

namespace Framework
{
    public class ManagerAttribute : Attribute
    {
        public int IntTag { get; private set; } = -1;
        
        public ManagerAttribute(int intTag)
        {
            this.IntTag = intTag;
        }

        public virtual string IndexName { get; } = nameof(IntTag);
    }
    
    public class ManagerBase<T,V,I> : IManager  where T : IManager, new() 
        where V : ManagerAttribute
    {
        
        private static T ins;

        public static T Ins
        {
            get
            {
                if (ins == null)
                {
                    ins = new T();
                }

                return ins;
            }
        }

        protected Dictionary<I, ClassData> ClassDataMap { get; } = new Dictionary<I, ClassData>();
        
        public virtual void Init()
        {
            
        }

        public virtual void Start()
        {
            
        }

        private PropertyInfo _indexProperty;
        
        public virtual void CheckType(Type type)
        {
            var attr = type.GetCustomAttribute<V>();
            
            if (attr != null)
            {
                if (_indexProperty == null)
                {
                    _indexProperty = typeof(V).GetProperty(attr.IndexName);
                }
                ClassDataMap[(I)_indexProperty.GetValue(attr)] = new ClassData {Attribute = attr, Type = type};
            }
        }
        
        public ClassData GetClassData(I tag)
        {
            this.ClassDataMap.TryGetValue(tag, out var classData);
            return classData;
        }
        
        public ClassData GetClassData<TN>()
        {
            return GetClassData(typeof(TN));
        }
        
        public ClassData GetClassData(Type type)
        {
            var classDatas = GetAllClassDatas();
            foreach (var value in classDatas)
            {
                if (value.Type == type)
                {
                    return value;
                }
            }
            return null;
        }
        
        public IEnumerable<ClassData> GetAllClassDatas()
        {
            return ClassDataMap.Values;
        }

        protected T CreateInstance<T>(I tag, params object[] args) where T : class
        {
            var cd = GetClassData(tag);
            if (cd == null)
            {
                Log.Error("没有找到:", tag, " -");
                return null;
            }
            return ReflectionHelper.CreateInstance<T>(cd.Type, args);
        }
    }
}