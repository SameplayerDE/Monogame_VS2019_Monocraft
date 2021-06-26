using Monocraft.Commands;
using System;

namespace Monocraft
{
    class Program
    {

        static bool running = false;
        static CommandHandler commandHandler = new CommandHandler();

        static void Main(string[] args)
        {
            string raw;
            string[] prepared;

            commandHandler.Register("help", new HelpCommand());
            commandHandler.Register("close", new CloseCommand());
            commandHandler.Register("and", new AndCommand());
            commandHandler.Register("or", new OrCommand());
            commandHandler.Register("varint", new VarIntCommand());
            commandHandler.Register("position", new PositionCommand());

            running = true;

            while (running)
            {
                Console.Write("#");
                raw = Console.ReadLine();
                if (string.IsNullOrEmpty(raw) || string.IsNullOrWhiteSpace(raw))
                {
                    continue;
                }

                commandHandler.PrepareCommand(raw, out prepared);
                int result = commandHandler.ProcessCommand(prepared);

                if (result > 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"!Befehlsfehler #{ result }");
                    Console.ForegroundColor = ConsoleColor.Gray;
                }

            }

        }
    }
}
