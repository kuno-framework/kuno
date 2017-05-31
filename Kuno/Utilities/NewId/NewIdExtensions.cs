using System;

namespace Kuno.Utilities.NewId
{
    internal static class NewIdExtensions
    {
        public static NewId ToNewId(this Guid guid)
        {
            return new NewId(guid.ToByteArray());
        }
    }
}