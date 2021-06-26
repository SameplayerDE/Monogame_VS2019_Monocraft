using Monocraft_Client_NET;
using Monocraft_Protocol;
using Monocraft_Protocol.Packets.Serverbound.Handshake;
using Monocraft_Protocol.Packets.Serverbound.Login;
using Monocraft_Protocol.Packets.Serverbound.Play;
using System;
using System.Text.Json;
using System.Threading;

namespace Monocraft_Test
{
    public class Program
    {

        static string host = "localhost";
        static ushort port = 25566;

        public static void Main(string[] args)
        {
            MinecraftClient _client = new MinecraftClient();
            _client.Connect(host, port);

            _client.SendMessage(new Handshake(754, host, port, State.LOGIN));
            _client.SendMessage(new LoginStart("SameplayerDE"));

            Console.ReadKey();

        }

        public static string PrettyJson(string unPrettyJson)
        {
            var options = new JsonSerializerOptions()
            {
                WriteIndented = true
            };

            var jsonElement = JsonSerializer.Deserialize<JsonElement>(unPrettyJson);

            return JsonSerializer.Serialize(jsonElement, options);
        }
    }
}