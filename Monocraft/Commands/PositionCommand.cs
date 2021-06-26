using Monocraft_API.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monocraft.Commands
{
    public class PositionCommand : Command
    {
        public PositionCommand()
        {
        }

        public override int Invoke(string[] arguments)
        {
            if (arguments.Length == 0)
            {
                return 0;
            }
            else if (arguments.Length == 1)
            {
                ulong var = 3272159489155520;

                ulong x = var & 0xFFFFFFC000000000;
                ulong y = var & 0x3FFFFFF000;
                ulong z = var & 0xFFF;

                int o_x;
                int o_y;
                int o_z;

                x = x >> 38;
                if ((x & 0x2000000) > 0)
                {
                    x = x ^ 0x3FFFFFF;
                    o_x = -Convert.ToInt32(x);
                }
                else
                {
                    o_x = Convert.ToInt32(x);
                }


                y = y >> 12;
                if ((y & 0x2000000) > 0)
                {
                    y = y ^ 0x3FFFFFF;
                    o_y = -Convert.ToInt32(y);
                }
                else
                {
                    o_y = Convert.ToInt32(y);
                }

                if ((z & 0x800) > 0)
                {
                    z = z ^ 0xFFF;
                    o_z = -Convert.ToInt32(z);
                }
                else
                {
                    o_z = Convert.ToInt32(z);
                }

                Console.WriteLine($"x: {o_x}/ y: {o_z} / z: {o_y}");
            }
            return 1;
        }
    }
}
