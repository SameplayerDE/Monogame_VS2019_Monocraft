using System;

namespace Monocraft_Client_NET
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new MonocraftGame())
                game.Run();
        }
    }
}
