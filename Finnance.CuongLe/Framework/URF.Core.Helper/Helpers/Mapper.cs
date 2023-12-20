using System;
using System.Linq;
using System.Reflection;
using URF.Core.Helper.Extensions;

namespace URF.Core.Helper
{
    public class Mapper
    {
        public static T Map<T>(object sourceObj)
        {
            if (sourceObj == null)
                return default;

            Type T1 = sourceObj.GetType();
            var targetObj = Activator.CreateInstance(typeof(T));
            var sourceProprties = T1.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            var targetProprties = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);

            foreach (var sourceProp in sourceProprties)
            {
                int entIndex = Array.IndexOf(targetProprties, sourceProp);
                if (entIndex >= 0)
                {
                    object osourceVal = sourceProp.GetValue(sourceObj, null);
                    var targetProp = targetProprties[entIndex];
                    if (sourceProp.PropertyType == targetProp.PropertyType)
                        targetProp.SetValue(targetObj, osourceVal);
                }
                else
                {
                    var targetProp = targetProprties.FirstOrDefault(c => c.Name.EqualsEx(sourceProp.Name));
                    if (targetProp != null)
                    {
                        object osourceVal = sourceProp.GetValue(sourceObj, null);
                        var sourcePropertyType = sourceProp.PropertyType;
                        if (sourcePropertyType.IsGenericType && sourcePropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                            sourcePropertyType = sourcePropertyType.GetGenericArguments()[0];

                        var targetPropertyType = targetProp.PropertyType;
                        if (targetPropertyType.IsGenericType && targetPropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                            targetPropertyType = targetPropertyType.GetGenericArguments()[0];

                        if (sourcePropertyType == targetPropertyType)
                            targetProp.SetValue(targetObj, osourceVal);
                    }
                }
            }

            foreach (var property in targetProprties)
            {
                if (property.Name == "IsActive")
                {
                    var valueProperty = property.GetValue(targetObj);
                    if (valueProperty == null)
                        property.SetValue(targetObj, true);
                }
                if (property.Name == "IsDelete")
                {
                    var valueProperty = property.GetValue(targetObj);
                    if (valueProperty == null)
                        property.SetValue(targetObj, false);
                }
            }
            return (T)targetObj;
        }

        public static T MapMeey<T>(object sourceObj)
        {
            if (sourceObj == null)
                return default;

            Type T1 = sourceObj.GetType();
            var targetObj = Activator.CreateInstance(typeof(T));
            var sourceProprties = T1.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            var targetProprties = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);

            foreach (var sourceProp in sourceProprties)
            {
                int entIndex = Array.IndexOf(targetProprties, sourceProp);
                if (entIndex >= 0)
                {
                    object osourceVal = sourceProp.GetValue(sourceObj, null);
                    var targetProp = targetProprties[entIndex];
                    if (sourceProp.PropertyType == targetProp.PropertyType)
                        targetProp.SetValue(targetObj, osourceVal);
                }
                else
                {
                    var targetProp = targetProprties.FirstOrDefault(c => c.Name.EqualsEx(sourceProp.Name));
                    if (targetProp != null)
                    {
                        object osourceVal = sourceProp.GetValue(sourceObj, null);
                        var sourcePropertyType = sourceProp.PropertyType;
                        if (sourcePropertyType.IsGenericType && sourcePropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                            sourcePropertyType = sourcePropertyType.GetGenericArguments()[0];

                        var targetPropertyType = targetProp.PropertyType;
                        if (targetPropertyType.IsGenericType && targetPropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                            targetPropertyType = targetPropertyType.GetGenericArguments()[0];

                        if (sourcePropertyType == targetPropertyType)
                            targetProp.SetValue(targetObj, osourceVal);
                    }
                    else
                    {
                        if (sourceProp.Name == "_id")
                        {
                            targetProp = targetProprties.FirstOrDefault(c => c.Name.EqualsEx("MeeyId"));
                            if (targetProp != null)
                            {
                                object osourceVal = sourceProp.GetValue(sourceObj, null);
                                targetProp.SetValue(targetObj, osourceVal);
                            }
                        }
                    }
                }
            }
            return (T)targetObj;
        }


        public static T MapTo<T>(object sourceObj, object targetObj)
        {
            if (sourceObj == null)
                return default;

            Type T1 = sourceObj.GetType();
            var sourceProprties = T1.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            var targetProprties = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);
            var ignoreProperties = new string[] { "IsActive", "IsDelete", "CreatedBy", "UpdatedBy", "CreatedDate", "UpdatedDate" };

            foreach (var sourceProp in sourceProprties.Where(c => !ignoreProperties.Contains(c.Name)))
            {
                int entIndex = Array.IndexOf(targetProprties, sourceProp);
                if (entIndex >= 0)
                {
                    object osourceVal = sourceProp.GetValue(sourceObj, null);
                    var targetProp = targetProprties[entIndex];
                    if (sourceProp.PropertyType == targetProp.PropertyType)
                        targetProp.SetValue(targetObj, osourceVal);
                }
                else
                {
                    var targetProp = targetProprties.FirstOrDefault(c => c.Name.EqualsEx(sourceProp.Name));
                    if (targetProp != null)
                    {
                        object osourceVal = sourceProp.GetValue(sourceObj, null);
                        var sourcePropertyType = sourceProp.PropertyType;
                        if (sourcePropertyType.IsGenericType && sourcePropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                            sourcePropertyType = sourcePropertyType.GetGenericArguments()[0];

                        var targetPropertyType = targetProp.PropertyType;
                        if (targetPropertyType.IsGenericType && targetPropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                            targetPropertyType = targetPropertyType.GetGenericArguments()[0];

                        if (sourcePropertyType == targetPropertyType)
                            targetProp.SetValue(targetObj, osourceVal);
                    }
                }
            }

            foreach (var property in targetProprties)
            {
                if (property.Name == "IsActive")
                {
                    var valueProperty = property.GetValue(targetObj);
                    if (valueProperty == null)
                        property.SetValue(targetObj, true);
                }
                if (property.Name == "IsDelete")
                {
                    var valueProperty = property.GetValue(targetObj);
                    if (valueProperty == null)
                        property.SetValue(targetObj, false);
                }
            }
            return (T)targetObj;
        }
    }
}
