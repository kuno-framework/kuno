using System;
using System.Text.RegularExpressions;

namespace Kuno.Services.Inventory
{
    public class EndPointPath : IComparable
    {
        public bool IsSystemPath => this.Path?.StartsWith("_") == true;

        public string Path { get; set; }

        public int Version { get; set; }

        public bool IsVersioned { get; set; }

        public string Value { get; set; }

        private static Regex Regex = new Regex("(^\\/?v(\\d*)\\/)(.*)");

        public EndPointPath(string value)
        {
            this.Value = this.Path = value?.Trim().Trim('/');
            if (!String.IsNullOrWhiteSpace(this.Value))
            {
                if (Regex.IsMatch(value))
                {
                    var match = Regex.Match(this.Value);
                    this.Version = Convert.ToInt32(match.Groups[2].Value);
                    this.Path = match.Groups[3].Value;
                    this.IsVersioned = true;
                }
            }
        }

        public int CompareTo(object obj)
        {
            var source = obj as EndPointPath;
            if (source == null)
            {
                return -1;
            }
            if (this.IsSystemPath && !source.IsSystemPath)
            {
                return 1;
            }
            if (source.IsSystemPath && !this.IsSystemPath)
            {
                return -1;
            }
            if (this.IsVersioned && !source.IsVersioned)
            {
                return 1;
            }
            if (source.IsVersioned && !this.IsVersioned)
            {
                return -1;
            }
            if (this.Path == source.Path)
            {
                return this.Version.CompareTo(source.Version);
            }
            return String.CompareOrdinal(this.Value, source.Value);
        }
    }
}