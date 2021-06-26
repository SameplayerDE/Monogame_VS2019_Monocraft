using Monocraft_API.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monocraft.Commands
{
    public class VarIntCommand : Command
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

                if (arguments[0] == "r")
                {
                    int numRead = 0;
                    int result = 0;
                    byte read;
                    int i = 1;

                    string hex;
                    int dec;

                    do
                    {
                        try
                        {
                            hex = arguments[i++];
                        }catch (IndexOutOfRangeException)
                        {
                            return 1;
                        }
                        foreach (char c in hex)
                        {
                            if (!Uri.IsHexDigit(c))
                            {
                                return 1;
                            }
                        }
                        dec = int.Parse(hex, System.Globalization.NumberStyles.HexNumber);
                        read = Convert.ToByte(dec);
                        int value = (read & 0b01111111);
                        result |= (value << (7 * numRead));

                        numRead++;
                        if (numRead > 5)
                        {
                            throw new Exception("VarInt is too big");
                        }
                    } while ((read & 0b10000000) != 0);
                    Console.WriteLine(result);
                }
                else if (arguments[0] == "w")
                {

                    List<byte> buffer = new List<byte>();
                    int a = int.Parse(arguments[1]);

                    do
                    {
                        byte temp = (byte)(a & 0b01111111);
                        a >>= 7;
                        if (a != 0)
                        {
                            temp |= 0b10000000;
                        }
                        buffer.Add(temp);
                    } while (a != 0);

                    foreach (byte b in buffer)
                    {
                        Console.WriteLine(b);
                    }

                }

                return 0;
            }
            else if (arguments.Length >= 2)
            {

                if (arguments[0] == "r")
                {
                    int numRead = 0;
                    int result = 0;
                    byte read;
                    int i = 1;

                    string hex;
                    int dec;

                    do
                    {
                        try
                        {
                            hex = arguments[i++];
                        }
                        catch (IndexOutOfRangeException)
                        {
                            return 1;
                        }
                        foreach (char c in hex)
                        {
                            if (!Uri.IsHexDigit(c))
                            {
                                return 1;
                            }
                        }
                        dec = int.Parse(hex, System.Globalization.NumberStyles.HexNumber);
                        read = Convert.ToByte(dec);
                        int value = (read & 0b01111111);
                        result |= (value << (7 * numRead));

                        numRead++;
                        if (numRead > 5)
                        {
                            throw new Exception("VarInt is too big");
                        }
                    } while ((read & 0b10000000) != 0);
                    Console.WriteLine(result);
                }
                
                return 0;
            }
            return 1;
        }
    }
}
