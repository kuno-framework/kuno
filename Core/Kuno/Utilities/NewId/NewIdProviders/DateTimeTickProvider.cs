using System;

namespace Kuno.Utilities.NewId.NewIdProviders
{
    internal class DateTimeTickProvider :
        ITickProvider
    {
        public long Ticks
        {
            get { return DateTime.UtcNow.Ticks; }
        }
    }
}