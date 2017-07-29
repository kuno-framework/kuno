using System;

namespace Kuno.Tests
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TestSubjectAttribute : Attribute
    {
        public Type Type { get; private set; }

        public TestSubjectAttribute(Type type)
        {
            this.Type = type;
        }
    }
}
