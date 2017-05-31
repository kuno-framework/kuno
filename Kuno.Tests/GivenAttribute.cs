using System;

namespace Kuno.Tests
{
    public class GivenAttribute : Attribute
    {
        public Type Name { get; }

        public GivenAttribute(Type name)
        {
            this.Name = name;
        }
    }
}