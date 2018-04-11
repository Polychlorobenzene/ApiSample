using System;
using System.Collections.Generic;
using System.Text;

namespace ApiClient
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class NgtsTypeAttribute : Attribute
    {
        public NgtsTypeAttribute(string Name)
        {
            NgtsType = Name;
        }

        public string NgtsType { get; private set; }
    }
}
