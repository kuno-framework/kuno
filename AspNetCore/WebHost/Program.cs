using Kuno;
using Kuno.AspNetCore;

namespace WebHost
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using (var stack = new Stack())
            {
                stack.RunWebHost();
            }
        }
    }
}
