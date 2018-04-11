using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace ApiClient
{
    public sealed class InheritanceSerializationBinder : DefaultSerializationBinder
    {
        public override Type BindToType(string assemblyName, string typeName)
        {
            var matchedType = GetTypesWithNgtsTypeAttribute(typeName).FirstOrDefault();


            if (matchedType != null)
            {
                return matchedType;
            }
            else
            {
                return base.BindToType(string.Empty, string.Empty);
            }
        }

        public override void BindToName(Type serializedType, out string assemblyName, out string typeName)
        {
            var t = serializedType.GetCustomAttributes<NgtsTypeAttribute>().SingleOrDefault();

            if (t != null)
            {
                assemblyName = t.NgtsType.Substring(0, t.NgtsType.LastIndexOf("."));
                typeName = t.NgtsType;
            }
            else
            {
                assemblyName = string.Empty;
                typeName = string.Empty;
            }

        }

        private IEnumerable<Type> GetTypesWithNgtsTypeAttribute(string typeName)
        {
            foreach (var t in Assembly.GetExecutingAssembly().GetTypes())
            {
                var na = t.GetCustomAttributes().OfType<NgtsTypeAttribute>().FirstOrDefault();

                if (na != null && string.Equals(na.NgtsType, typeName))
                {
                    yield return t;
                }
            }
        }
    }
}

