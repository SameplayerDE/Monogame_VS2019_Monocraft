/*using System;
using System.Collections.Generic;
using System.Text.Json;
using Monocraft_Protocol.Packets.Serverbound.Handshake;
using Monocraft_Protocol.Packets.Serverbound.Status;
using Monocraft_Protocol.Packets.Serverbound.Login;
using Monocraft_Protocol.Packets.Clientbound.Status;
using System.Threading;
using Monocraft_Protocol;
using Monocraft_Protocol.Packets;
using Monocraft_Protocol.Packets.Clientbound.Login;
using Monocraft_Protocol.Packets.Serverbound.Play;
using Monocraft_Protocol.Packets.Clientbound.Play;
using System.Threading.Tasks;

#if DEBUG
#endif

namespace Monocraft_Test
{
    class ServerPing
    {

        private const string _host = "localhost";
        private const ushort _port = 25566;

        private static Dictionary<int, byte[]> _packets = new Dictionary<int, byte[]>();

        private static long _start = DateTimeOffset.Now.ToUnixTimeMilliseconds();

        private static Client _client = new Client();

        private static void Main(string[] args)
        {

            if (Login(_host, _port, "Username0006"))
            {

                while (_client.IsConnected)
                {
                    if (_client.NetworkStream.DataAvailable)
                    {

                        int length = _client.Read();
                        Packet packet = _client.ResolvePacket();

                    }
                }
                Console.WriteLine("Press ENTER to exit");
                Console.ReadLine();
            }
            Console.WriteLine("Press ENTER to exit");
            Console.ReadLine();
        }

        private static long GetUnixMilliseconds()
        {
            return DateTimeOffset.Now.ToUnixTimeMilliseconds();
        }

        private static void Chat(Client c, string message)
        {
            Monocraft_Protocol.Packets.Serverbound.Play.ChatMessage chatMessage = new Monocraft_Protocol.Packets.Serverbound.Play.ChatMessage(message);
            c.SendPacket(chatMessage);
        }

        private static bool Login(string host, ushort port, string username)
        {
            _client.Connect(host, port);

            Handshake handshake = new Handshake(754, host, port, State.LOGIN);
            _client.SendPacket(handshake);

            PacketResolver.ServerState = ServerState.LOGIN;

            LoginStart loginStart = new LoginStart(username);
            _client.SendPacket(loginStart);

            _client.Read();
            Packet packet = _client.ResolvePacket();

            if (packet is LoginSuccess)
            {
                PacketResolver.ServerState = ServerState.PLAY;
                LoginSuccess loginSuccess = packet as LoginSuccess;
                Console.WriteLine($"{loginSuccess.UUID}\n{loginSuccess.Username}");
                return true;
            }
            return false;
        }

        private static void Move(Client c, double x, double z)
        {
            PlayerPosition playerPosition = new PlayerPosition(x, 63, z, false);
            c.SendPacket(playerPosition);
        }

        private static void SwingArm(Client c, Hand hand)
        {
            Animation animation = new Animation(hand);
            c.SendPacket(animation);
        }

        

        private static void Ping(string host, ushort port)
        {
            Client c = _client;
            c.Connect(host, port);

            Handshake handshake = new Handshake(754, host, port, State.STATUS);
            c.SendPacket(handshake);

            PacketResolver.ServerState = ServerState.STATUS;

            Request request = new Request();
            c.SendPacket(request);

            c.Read();

            Packet packet = c.ResolvePacket();

            if (packet is Response)
            {
                Response response = packet as Response;

                Ping ping = new Ping(_start);
                c.SendPacket(ping);

                c.Read();

                packet = c.ResolvePacket();

                if (packet is Pong)
                {
                    Pong pong = packet as Pong;
                    if (pong.PacketID == ping.PacketID)
                    {
                        Console.WriteLine(PrettyJson(response.Json));
                    }
                }

                
            }

            Console.WriteLine("Press ENTER to exit");
            Console.ReadLine();
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
}*/