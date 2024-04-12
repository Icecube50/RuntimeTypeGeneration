using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuntimeTypeCreator.DynamicType.Attributes
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = false)]
    public sealed class SizeAttribute : Attribute
    {
        public SizeAttribute(params int[] size)
        {
            Size = size;
        }

        public readonly int[] Size;
    }
}
