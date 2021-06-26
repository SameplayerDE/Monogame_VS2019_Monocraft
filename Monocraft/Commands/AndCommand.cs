using Monocraft_API.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monocraft.Commands
{
    public class AndCommand : Command
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
                int a = 0, b = 0;
                string arg0 = arguments[0];
                string arg1 = arguments[1];
                if (arg0.StartsWith("0b"))
                {
                    a = Convert.ToInt32(arg0.Remove(0, 2), 2);
                }
                else
                {
                    a = int.Parse(arg0);
                }
                if (arg1.StartsWith("0b"))
                {
                    b = Convert.ToInt32(arg1.Remove(0, 2), 2);
                }
                else
                {
                    b = int.Parse(arg1);
                }
                Console.WriteLine(a & b);
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
