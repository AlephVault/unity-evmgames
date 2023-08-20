﻿using System;
using System.Reflection;

namespace AlephVault.Unity.EVMGames.Nethereum.ABI.FunctionEncoding.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class FunctionOutputAttribute : Attribute
    {
        public static FunctionOutputAttribute GetAttribute<T>()
        {
            var type = typeof(T);
            return type.GetTypeInfo().GetCustomAttribute<FunctionOutputAttribute>(true);
        }

        public static bool IsFunctionType<T>()
        {
            return GetAttribute<T>() != null;
        }
    }
}