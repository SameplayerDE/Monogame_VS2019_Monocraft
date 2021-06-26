using Ionic.Zlib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monocraft_Client_NET.Rendering;
using Monocraft_Client_NET.Rendering.Renderer;
using Monocraft_Protocol;
using Monocraft_Protocol.Data.Enums;
using Monocraft_Protocol.Packets;
using Monocraft_Protocol.Packets.Clientbound.Play;
using Monocraft_Protocol.Packets.Serverbound.Handshake;
using Monocraft_Protocol.Packets.Serverbound.Login;
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

namespace Monocraft_Client_NET
{

    public class Client
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

        public CubeMesh Cube = new CubeMesh();
        public BlockRenderer BlockRenderer;
        public Camera Camera;

        private Dictionary<Vector3, Material> _blocks = new Dictionary<Vector3, Material>();

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

        public void Login(string username)
        {
            SendMessage(new Handshake(754, Address, Port, State.LOGIN));
            SendMessage(new LoginStart(username));
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
                            OnMessageReceived(memoryStream.ToArray(), packetLength);
                        }

                    }
                    packetLength = 0;
                    bytesRead = 0;
                    Count++;
                }
            }
        }

        protected virtual void OnMessageReceived(byte[] data, int packetLength)
        {
            MemoryStream stream = new MemoryStream(data);
            PacketStream packetStream = new PacketStream(stream);
            int packetID = 0x00;

            if (CompressionThreshold != -1)
            {
                int compressionLength;
                int isCompressed = packetStream.ReadVarInt(out compressionLength);

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
                        Vector3 vector3 = new Vector3(position[0], position[1], position[2]);
                        if (_blocks.ContainsKey(vector3))
                        {
                            _blocks[vector3] = (Material)blockID;
                        }
                        else 
                        {
                            _blocks.Add(vector3, (Material)blockID);
                        }
                        Console.WriteLine($"x: {position[0]}, y: {position[1]}, z: {position[2]}\nBlockID: 0x{blockID:x2}\nMaterial: {(Material)blockID}");
                        return;
                    }
                    if (packetID == 0x34)
                    {
                        double x = packetStream.ReadDouble(); // x
                        double y = packetStream.ReadDouble(); // y
                        double z = packetStream.ReadDouble(); // z
                        float yaw = packetStream.ReadFloat(); // yaw
                        float pitch = packetStream.ReadFloat(); // pitch
                        byte flags = packetStream.ReadUnsignedByte(); // flags

                        double dirX = 0;
                        double dirY = 0;
                        double dirZ = 1;

                        Vector3 position = new Vector3((float)x, (float)y + 1.6f, (float)z);
                        Vector3 direction = new Vector3((float)dirX, (float)dirY, (float)dirZ);

                        Camera.RotateX(pitch);
                        Camera.RotateY(-yaw);
                        Camera.Teleport(position);

                        Console.WriteLine($"{x} {y} {z} | {Camera.RotationX} {Camera.RotationY}");

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
                else
                {
                    byte[] compressedData = packetStream.Read(packetLength - compressionLength);
                    using (PacketStream decompressedPacketStream = new PacketStream())
                    {
                        using (ZlibStream outZStream = new ZlibStream(decompressedPacketStream, CompressionMode.Decompress, CompressionLevel.Default, true))
                        {
                            outZStream.Write(compressedData, 0, compressedData.Length);
                        }

                        decompressedPacketStream.Seek(0, SeekOrigin.Begin);

                        int l;
                        packetID = decompressedPacketStream.ReadVarInt(out l);

                        if (packetID == 0x20)
                        {
                            ChunkData chunkData = new ChunkData();
                            chunkData.Decode(decompressedPacketStream);

                            Console.WriteLine($"{chunkData.ChunkX},{chunkData.ChunkY}\nfull:{chunkData.FullChunk}");

                            return;
                        }


                    }

                    
                }

            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            for (int key = _blocks.Count - 1; key >= 0; key--)
            {
                Vector3 position = _blocks.Keys.ToArray()[key];
                Material material = _blocks[position];
                if (material != Material.Air)
                {
                    BlockRenderer.DrawModel(Cube, position, Camera.View, Camera.Projection, gameTime);
                }
            }
           
        }

        public void Update(GameTime gameTime)
        {

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
            Console.WriteLine(message);
        }

        private void KeepAlive(long id)
        {
            PacketStream packetStream = new PacketStream();
            packetStream.WritePacket(new Monocraft_Protocol.Packets.Serverbound.Play.KeepAlive(id), 256);
            SendMessage(packetStream.ToPacket());
        }

    }
}
