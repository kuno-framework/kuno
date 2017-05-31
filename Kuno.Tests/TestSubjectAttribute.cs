using System;

namespace Kuno.Tests
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TestSubjectAttribute : Attribute
    {
        public Type Type { get; }

        public TestSubjectAttribute(Type type)
        {
            this.Type = type;
        }
    }
}
