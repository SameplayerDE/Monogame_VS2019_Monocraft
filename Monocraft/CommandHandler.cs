using Monocraft_API.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monocraft
{
    /// <summary>
    /// 
    /// </summary>
    public class CommandHandler
    {
        /// <summary>
        /// Contains defined commands
        /// </summary>
        Dictionary<string, Command> _commands = new Dictionary<string, Command>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="command"></param>
        public void Register(string key, Command command)
        {
            key = key.ToLower();
            _commands.Add(key, command);
        }
        /// <summary>
        /// splits raw input into an string array
        /// </summary>
        /// <param name="raw">raw input string</param>
        /// <param name="prepared">prepared string array</param>
        public void PrepareCommand(string raw, out string[] prepared)
        {
            prepared = raw.Split(null);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="prepared">command with args as string array</param>
        public int ProcessCommand(string[] prepared)
        {
            string command = prepared[0].ToLower();
            if (_commands.ContainsKey(command))
            {
                prepared = CutPrepared(prepared);
                int result = _commands[command].Invoke(prepared);
                if (result != 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"!Befehlsausführungsfehler #{ result }");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    return 2;
                }
                return 0;
            }
            return 1;
        }
        /// <summary>
        /// removes command and returns args of the command
        /// </summary>
        /// <param name="prepared"></param>
        /// <returns>the args of the command</returns>
        private string[] CutPrepared(string[] prepared)
        {
            return prepared.Skip(1).ToArray();
        }

    }
}
