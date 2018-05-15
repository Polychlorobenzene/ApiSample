using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace WebApi.Utility
{
    public static class ExtensionMethods
    {
        public static string GetPrettyName(this Type T)
        {
            StringBuilder sb = new StringBuilder();
            if (T.Name == "String")
                return Utility.GetTypeName(T);

            else if (typeof(IEnumerable).IsAssignableFrom(T))
            {
                bool isDict = T.IsGenericType && T.GetGenericTypeDefinition() == typeof(Dictionary<,>);
                if (!isDict)
                {
                    sb.Append("IList");
                    foreach (var arg in T.GetGenericArguments())
                    {
                        sb.Append("<");
                        sb.Append(Utility.GetTypeName(arg));
                        sb.Append(">");
                    }
                }
                else if (typeof(IDictionary).IsAssignableFrom(T))
                {
                    sb.Append("IDictionary");
                    sb.Append("<");
                    sb.Append(Utility.GetTypeName(T.GetGenericArguments()[0]));
                    sb.Append(",");
                    sb.Append(Utility.GetTypeName(T.GetGenericArguments()[1]));
                    sb.Append(">");
                }
            }

            else if (T.IsClass)
            {
                return Utility.GetTypeName(T);
            }
            else
            {
                if (T.IsGenericType && T.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    Type nullable = Nullable.GetUnderlyingType(T);
                    sb.Append(Utility.GetTypeName(nullable));
                    sb.Append("?");
                }
                else sb.Append(Utility.GetTypeName(T));
            }
            return sb.ToString();
        }
    }

    public static class Utility
    {
        internal static string GetTypeName(Type T)
        {
            return Aliases.ContainsKey(T) ? Aliases[T] : T.Name;
        }
        private static readonly Dictionary<Type, string> Aliases =
        new Dictionary<Type, string>()
        {
        { typeof(byte), "byte" },
        { typeof(sbyte), "sbyte" },
        { typeof(short), "short" },
        { typeof(ushort), "ushort" },
        { typeof(int), "int" },
        { typeof(uint), "uint" },
        { typeof(long), "long" },
        { typeof(ulong), "ulong" },
        { typeof(float), "float" },
        { typeof(double), "double" },
        { typeof(decimal), "decimal" },
        { typeof(object), "object" },
        { typeof(bool), "bool" },
        { typeof(char), "char" },
        { typeof(string), "string" },
        { typeof(void), "void" }
        };
    }

}