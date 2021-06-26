using System;
using System.IO;
using System.Text;

namespace Monocraft_Client_NET
{
    public static class FileConsole
    {

        private static readonly string DIRECTORY = $"Logs/{DateTime.Now:dd MMMM yyyy}";
        private static readonly string FILE = $"{DateTime.Now:HH mm ss}.txt";
        private static readonly string PATH = $"./{DIRECTORY}/{FILE}";
        private static string Time { get { return $"{DateTime.Now:HH mm ss}"; } }
        private static bool HasDirectory = false;
        private static bool HasFile = false;

        private static void CreateDirectory()
        {
            while (!Directory.Exists(DIRECTORY))
            {
                Directory.CreateDirectory(DIRECTORY);
                HasDirectory = true;
            }
        }

        public static void WriteLine()
        {
            CreateDirectory();
            if (File.Exists(PATH))
                using (StreamWriter sw = File.AppendText(PATH))
                {
                    sw.WriteLine(Time);
                }
            else
            {
                using (StreamWriter sw = File.CreateText(PATH))
                {
                    sw.WriteLine(Time);
                }
            }
        }

        public static void WriteLine(string x)
        {
            CreateDirectory();
            if (File.Exists(PATH))
                using (StreamWriter sw = File.AppendText(PATH))
                {
                    sw.WriteLine($"{Time, -10}{x}");
                }
            else
            {
                using (StreamWriter sw = File.CreateText(PATH))
                {
                    sw.WriteLine($"{Time,-10}{x}");
                }
            }
        }

        public static void WriteLine(int x)
        {
            CreateDirectory();
            if (File.Exists(PATH))
                using (StreamWriter sw = File.AppendText(PATH))
                {
                    sw.WriteLine($"{Time,-10}{x}");
                }
            else
            {
                using (StreamWriter sw = File.CreateText(PATH))
                {
                    sw.WriteLine($"{Time,-10}{x}");
                }
            }
        }

        public static void Write(string x)
        {
            CreateDirectory();
            if (File.Exists(PATH))
                using (StreamWriter sw = File.AppendText(PATH))
                {
                    sw.Write($"{x}");
                }
            else
            {
                using (StreamWriter sw = File.CreateText(PATH))
                {
                    sw.Write($"{x}");
                }
            }
        }

        public static void Write(int x)
        {
            CreateDirectory();
            if (File.Exists(PATH))
                using (StreamWriter sw = File.AppendText(PATH))
                {
                    sw.Write($"{x}");
                }
            else
            {
                using (StreamWriter sw = File.CreateText(PATH))
                {
                    sw.Write($"{x}");
                }
            }
        }

        public static void Write(char x)
        {
            CreateDirectory();
            if (File.Exists(PATH))
                using (StreamWriter sw = File.AppendText(PATH))
                {
                    sw.Write($"{x}");
                }
            else
            {
                using (StreamWriter sw = File.CreateText(PATH))
                {
                    sw.Write($"{x}");
                }
            }
        }

        public static void Write(char[] x)
        {
            CreateDirectory();
            if (File.Exists(PATH))
                using (StreamWriter sw = File.AppendText(PATH))
                {
                    sw.Write($"{new string(x)}");
                }
            else
            {
                using (StreamWriter sw = File.CreateText(PATH))
                {
                    sw.Write($"{new string(x)}");
                }
            }
        }

        public static void XWrite(char[] x)
        {
            CreateDirectory();
            if (File.Exists(PATH))
                using (StreamWriter sw = File.AppendText(PATH))
                {
                    string dump = "";
                    foreach (char c in x)
                    {
                        string hex = Convert.ToByte(c).ToString("x2");
                        dump += "," + hex;
                    }
                    sw.Write($"{dump.Replace(',', ' ')}");
                }
            else
            {
                using (StreamWriter sw = File.CreateText(PATH))
                {
                    string dump = "";
                    foreach (char c in x)
                    {
                        string hex = Convert.ToByte(c).ToString("x2");
                        dump += "," + hex;
                    }
                    sw.Write($"{dump.Replace(',', ' ')}");
                }
            }
        }

        public static void Write(byte[] x)
        {
            CreateDirectory();
            if (File.Exists(PATH))
                using (StreamWriter sw = File.AppendText(PATH))
                {
                    sw.BaseStream.Write(x, 0, x.Length);
                }
            else
            {
                using (StreamWriter sw = File.CreateText(PATH))
                {
                    sw.BaseStream.Write(x, 0, x.Length);
                }
            }
        }

        public static void Write(byte x)
        {
            CreateDirectory();
            if (File.Exists(PATH))
                using (StreamWriter sw = File.AppendText(PATH))
                {
                    sw.BaseStream.WriteByte(x);
                }
            else
            {
                using (StreamWriter sw = File.CreateText(PATH))
                {
                    sw.BaseStream.WriteByte(x);
                }
            }
        }

        public static void WriteLine(bool x)
        {
            CreateDirectory();
            if (File.Exists(PATH))
                using (StreamWriter sw = File.AppendText(PATH))
                {
                    sw.WriteLine($"{Time,-10}{x}");
                }
            else
            {
                using (StreamWriter sw = File.CreateText(PATH))
                {
                    sw.WriteLine($"{Time,-10}{x}");
                }
            }
        }

    }
}
