using System;
using System.Linq;
using UnityEngine;

namespace SodaGameLibrary.Utils
{
    public class EnumUtil
    {
        public static T[] GetEnums<T>() where T : Enum
        {
            return Enum.GetNames(typeof(T))
                .Select(x => Parsing<T>(x))
                .ToArray();
        }
        public static T Parsing<T>(string value) where T : Enum
        {
            return (T)Enum.Parse(typeof(T), value);
        }
    }
}
