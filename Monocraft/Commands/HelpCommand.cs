using Monocraft_API.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monocraft.Commands
{
    public class HelpCommand : Command
    {

        public static new string Usage = "help";
        public static new string Description = "zeigt Befehlsauflistung mit jeweiliger Beschreibung";

        public override int Invoke(string[] arguments)
        {
            if (arguments.Length == 0)
            {
                Console.WriteLine($"{ Usage }: { Description }");
                Console.WriteLine($"{ CloseCommand.Usage }: { CloseCommand.Description }");
                return 0;
            }
            
            return 1;
        }
    }
}
