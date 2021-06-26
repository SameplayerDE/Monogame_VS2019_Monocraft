using Monocraft_Protocol;
using Monocraft_Protocol.Data.Enums;
using Monocraft_Protocol.Packets;
using Monocraft_Protocol.Packets.Serverbound.Play;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Monocraft_Test
{

    public class MinecraftClient
    {

        private Thread _receivingThread;
        private Thread _sendingThread;

        public static int CompressionThreshold = -1;
        public static bool CheckedCompression = false;
        public TcpClient TcpClient;
        public string Address;
        public ushort Port;

        public Queue<byte[]> PacketQueue = new Queue<byte[]>();
        public int Count = 0;
        public ClientConnectionState ClientConnectionState = ClientConnectionState.Disconnected;

        public Dictionary<int, string> Players = new Dictionary<int, string>();

        public void Connect(string address, ushort port)
        {

            Console.Write("Connecting to server...");

            Address = address;
            Port = port;
            TcpClient = new TcpClient();
            Task connectionTask = TcpClient.ConnectAsync(Address, Port);

            while (!connectionTask.IsCompleted)
            {
                Console.Write(".");
                Thread.Sleep(500);
            }

            if (connectionTask.IsFaulted)
            {
                Console.WriteLine("\nCould not connect to server.");
                return;
            }
            else
            {
                Console.WriteLine("\nConnected to server.");
                ClientConnectionState = ClientConnectionState.Connected;
            }

            TcpClient.ReceiveBufferSize = 256;
            TcpClient.SendBufferSize = 256;
            ClientConnectionState = ClientConnectionState.Connected;


            _receivingThread = new Thread(ReceivingMethod);
            _receivingThread.IsBackground = true;
            _receivingThread.Start();

            _sendingThread = new Thread(SendingMethod);
            _sendingThread.IsBackground = true;
            _sendingThread.Start();
        }

        private void SendingMethod(object obj)
        {
            while (ClientConnectionState == ClientConnectionState.Connected)
            {
                if (PacketQueue.Count > 0)
                {
                    byte[] packet = PacketQueue.Dequeue();

                    try
                    {
                        TcpClient.GetStream().Write(packet, 0, packet.Length);
                    }
                    catch
                    {
                        Disconnect();
                    }
                }

                Thread.Sleep(50);
            }
        }

        private void Disconnect()
        {
            PacketQueue.Clear();
            try
            {
                //SendMessage();
            }
            catch { }
            Thread.Sleep(1000);
            ClientConnectionState = ClientConnectionState.Disconnected;
            TcpClient.Client.Disconnect(false);
            TcpClient.Close();
        }

        public void SendMessage(byte[] data)
        {
            foreach (byte @byte in data)
            {
                //Console.Write($"{@byte:x2} ");
            }
            //Console.WriteLine();
            PacketQueue.Enqueue(data);
        }

        public void SendMessage(Packet packet)
        {
            SendMessage(packet.ToBytes(CompressionThreshold));
        }

        private void ReceivingMethod(object obj)
        {
            PacketStream stream = new PacketStream(TcpClient.GetStream());
            int packetLength = 0;
            int bytesRead = 0;
            int packetID = -1;
            while (ClientConnectionState == ClientConnectionState.Connected)
            {

                if (TcpClient.Available > 0)
                {
                    Console.Title = $"Available: {TcpClient.Available}, Count: {Count}";
                    if (packetLength == 0)
                    {
                        packetLength = stream.ReadVarInt();
                        //Console.WriteLine($"{packetLength:x2}");

                    }
                    if (Count == 0)
                    {
                        packetID = stream.ReadVarInt();
                        bytesRead++;
                        if (packetID == 0x03)
                        {
                            CompressionThreshold = stream.ReadVarInt();
                            bytesRead = packetLength;
                        }
                        CheckedCompression = true;
                    }
                    else
                    {
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            while (packetLength > bytesRead)
                            {
                                byte @byte = stream.ReadUnsignedByte();
                                bytesRead++;
                                //Console.Write($"{@byte:x2} ");
                                memoryStream.WriteByte(@byte);
                            }
                            //Console.WriteLine();
                            OnMessageReceived(memoryStream.ToArray());
                        }

                    }
                    packetLength = 0;
                    bytesRead = 0;
                    Count++;
                }
            }
        }

        protected virtual void OnMessageReceived(byte[] data)
        {
            MemoryStream stream = new MemoryStream(data);
            PacketStream packetStream = new PacketStream(stream);
            int packetID = 0x00;

            if (CompressionThreshold != -1)
            {

                int isCompressed = packetStream.ReadVarInt();

                if (isCompressed == 0x00)
                {
                    packetID = packetStream.ReadByte();

                    if (packetID == 0x1f)
                    {
                        KeepAlive(packetStream.ReadLong());
                        Console.WriteLine("+");
                        return;
                    }
                    if (packetID == 0x0e)
                    {
                        DisplayChat(packetStream.ReadString());
                        return;
                    }
                    if (packetID == 0x0b)
                    {
                        int[] position = packetStream.ReadPosition();
                        int blockID = packetStream.ReadVarInt();
                        Console.WriteLine($"x: {position[0]}, y: {position[1]}, z: {position[2]}\nBlockID: 0x{blockID:x2}\nMaterial: {(Material)blockID}");
                        return;
                    }
                    if (packetID == 0x34)
                    {
                        packetStream.ReadDouble(); // x
                        packetStream.ReadDouble(); // y
                        packetStream.ReadDouble(); // z
                        packetStream.ReadFloat(); // yaw
                        packetStream.ReadFloat(); // pitch
                        packetStream.ReadByte(); // flags
                        ConfirmTeleport(packetStream.ReadVarInt()); // TeleportID
                        return;
                    }
                    if (packetID == 0x49)
                    {
                        float health = packetStream.ReadFloat();
                        int food = packetStream.ReadVarInt();
                        float foodSaturation = packetStream.ReadFloat();
                        Console.WriteLine($"{health:0} | {food}");
                    }
                    if (packetID == 0x44)
                    {

                        int entityID = packetStream.ReadVarInt();
                        Console.WriteLine($"{entityID}");
                        /*int index = 0x00;

                        do
                        {
                            index = packetStream.ReadUnsignedByte();
                            Console.WriteLine("index: " + index);
                            if (index != 0xff)
                            {
                                int type = packetStream.ReadVarInt();
                                Console.WriteLine(type);
                            }
                        } while (index != 0xff);*/
                        return;
                    }
                    if (packetID == 0x32)
                    {
                        int action = packetStream.ReadVarInt();
                        int numberOfPlayers = packetStream.ReadVarInt();

                        for (int i = 0; i < numberOfPlayers; i++)
                        {
                            Guid uuid = packetStream.ReadUUID();
                            if (action == 2) // ping
                            {
                                int ping = packetStream.ReadVarInt();

                                string pingIcon = "";
                                if (ping < 150)
                                {
                                    pingIcon = "|||||";
                                }
                                else if (ping < 300)
                                {
                                    pingIcon = "||||";
                                }
                                else if (ping < 600)
                                {
                                    pingIcon = "|||";
                                }
                                else if (ping < 1000)
                                {
                                    pingIcon = "||";
                                }
                                else if (ping >= 1000)
                                {
                                    pingIcon = "|";
                                }

                                Console.WriteLine($"{uuid} has a ping of {pingIcon} ({ping}ms)");
                            }
                        }

                        return;
                    }
                }

            }
        }

        private void ConfirmTeleport(int id)
        {
            Console.WriteLine(id);
            MemoryStream stream = new MemoryStream();
            PacketStream packetStream = new PacketStream(stream);
            packetStream.WriteByte(0x00);
            packetStream.WriteByte(0x00);
            packetStream.WriteVarInt(id);
            SendMessage(packetStream.ToPacket());
        }

        private void DisplayChat(string message)
        {
            Console.WriteLine(Program.PrettyJson(message));
        }

        private void KeepAlive(long id)
        {
            PacketStream packetStream = new PacketStream();
            packetStream.WritePacket(new KeepAlive(id), 256);
            SendMessage(packetStream.ToPacket());
        }

    }
}
