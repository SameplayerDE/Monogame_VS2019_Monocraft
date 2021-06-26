using Monocraft_API.Abstracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monocraft.Commands
{
    public class CloseCommand : Command
    {

        public static new string Usage = "close";
        public static new string Description = "Schließt die Konsole";

        public override int Invoke(string[] arguments)
        {
            switch(arguments.Length)
            {
                case 0:
                    {
                        Process.GetCurrentProcess().Kill();
                        return 0;
                    }
            }
            return 1;
        }
    }
}
