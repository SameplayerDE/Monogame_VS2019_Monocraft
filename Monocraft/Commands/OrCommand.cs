using Monocraft_API.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monocraft.Commands
{
    public class OrCommand : Command
    {
        public override int Invoke(string[] arguments)
        {
            if (arguments.Length == 0)
            {
                return 0;
            }
            else if (arguments.Length == 1)
            {
                return 0;
            }
            else if (arguments.Length == 2)
            {
                int a = int.Parse(arguments[0]);
                int b = int.Parse(arguments[1]);
                Console.WriteLine(a | b);
                return 0;
            }
            else
            {
                return 0;
            }
            return 1;
        }
    }
}
