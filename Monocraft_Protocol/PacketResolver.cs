using Monocraft_Protocol.Packets;
using Monocraft_Protocol.Packets.Clientbound.Login;
using Monocraft_Protocol.Packets.Clientbound.Play;
using Monocraft_Protocol.Packets.Clientbound.Status;
using Monocraft_Protocol.Packets.Serverbound.Handshake;
using Monocraft_Protocol.Packets.Serverbound.Login;
using Monocraft_Protocol.Packets.Serverbound.Play;
using Monocraft_Protocol.Packets.Serverbound.Status;
using System;

namespace Monocraft_Protocol
{

    public enum ServerState
    {
        HANDSHAKE = 0,
        STATUS = 1,
        LOGIN = 2,
        PLAY = 3
    }

    public static class PacketResolver
    {

        /*public static ServerState ServerState = ServerState.HANDSHAKE;

        public static Packet ResolveServerbound(DataReader reader)
        {
            return ResolveServerbound(reader.Data);
        }

        public static Packet ResolveServerbound(byte[] data)
        {

            for (int i = 0; i < data.Length; i++)
            {
                //Console.WriteLine(data[i].ToString("X2"));
            }

            DataReader reader = new DataReader(data);
            int length = reader.ReadVarInt(); //Length
            int packetID = reader.ReadVarInt(); //ID
            //Console.WriteLine($"Length: {length:X2}, ID: {packetID:X2}");
            //Console.ReadLine();
            switch (packetID)
            {
                case 0x00:
                    {
                        switch (ServerState)
                        {
                            case ServerState.HANDSHAKE:
                                {
                                    Console.WriteLine("Handshake");
                                    return new Handshake(reader);
                                }
                            case ServerState.STATUS:
                                {
                                    Console.WriteLine("Request");
                                    return new Request(reader);
                                }
                            case ServerState.LOGIN:
                                {
                                    Console.WriteLine("Login Start");
                                    return new LoginStart(reader);
                                }
                            case ServerState.PLAY:
                                {
                                    Console.WriteLine("Teleport Confirm");
                                    return new TeleportConfirm(reader);
                                }
                        }
                        break;
                    }
                        
            }

            return null;
        }

        public static Packet ResolveClientbound(DataReader reader)
        {
            return ResolveClientbound(reader.Data);
        }

        public static Packet ResolveClientbound(byte[] data)
        {
            DataReader reader = new DataReader(data);
            int length = reader.ReadVarInt(); //Length
            int packetID = reader.ReadVarInt(); //ID
            switch (packetID)
            {
                case 0x00:
                    {
                        switch (ServerState)
                        {
                            case ServerState.HANDSHAKE:
                                {
                                    return null;
                                }
                            case ServerState.STATUS:
                                {
                                    Console.WriteLine("Response");
                                    return new Response(reader);
                                }
                            case ServerState.LOGIN:
                                {
                                    Console.WriteLine("Login Success");
                                    return new LoginSuccess(reader);
                                }
                            case ServerState.PLAY:
                                {
                                    Console.WriteLine("Spawn Entity");
                                    return new SpawnEntity(reader);
                                }
                        }
                        break;
                    }
                case 0x01:
                    {
                        switch (ServerState)
                        {
                            case ServerState.HANDSHAKE:
                                {
                                    return null;
                                }
                            case ServerState.STATUS:
                                {
                                    Console.WriteLine("Pong");
                                    return new Pong(reader);
                                }
                            case ServerState.LOGIN:
                                {
                                    Console.WriteLine("Encryption Request");
                                    return new EncryptionRequest(reader);
                                }
                            case ServerState.PLAY:
                                {
                                    Console.WriteLine("Spawn Experience Orb");
                                    return null;
                                }
                        }
                        break;
                    }
                case 0x02:
                    {
                        switch (ServerState)
                        {
                            case ServerState.HANDSHAKE:
                                {
                                    return null;
                                }
                            case ServerState.STATUS:
                                {
                                    return null;
                                }
                            case ServerState.LOGIN:
                                {
                                    Console.WriteLine("Login Success");
                                    return new LoginSuccess(reader);
                                }
                            case ServerState.PLAY:
                                {
                                    Console.WriteLine("Spawn Living Entity");
                                    return null;
                                }
                        }
                        break;
                    }
                case 0x1F:
                    {
                        Console.WriteLine("Keep Alive");
                        return new Packets.Clientbound.Play.KeepAlive(reader);
                    }
                case 0x4E:
                    {
                        Console.WriteLine("Time Update");
                        return new Packets.Clientbound.Play.TimeUpdate(reader);
                    }
                case 0x19:
                    {
                        switch (ServerState)
                        {
                            case ServerState.PLAY:
                                {
                                    Console.WriteLine("Disconnect");
                                    return new Packets.Clientbound.Play.Disconnect(reader);
                                }
                        }
                        break;
                    }
                case 0x34:
                    {
                        Console.WriteLine("Player Position And Look");
                        return new PlayerPositionAndLook(reader);
                        //return null;
                    }
                case 0x0E:
                    {
                        switch (ServerState)
                        {
                            case ServerState.PLAY:
                                {
                                    Console.WriteLine("Chat Message");
                                    return new Packets.Clientbound.Play.ChatMessage(reader);
                                }
                        }
                        break;
                    }
                case 0x32:
                    {
                        switch (ServerState)
                        {
                            case ServerState.PLAY:
                                {
                                    Console.WriteLine("Player Info");
                                    return null;
                                }
                        }
                        break;
                    }
                case 0x44:
                    {
                        switch (ServerState)
                        {
                            case ServerState.PLAY:
                                {
                                    Console.WriteLine("Entity Metadata");
                                    return null;
                                }
                        }
                        break;
                    }
                case 0x40:
                    {
                        switch (ServerState)
                        {
                            case ServerState.PLAY:
                                {
                                    Console.WriteLine("Update View Position");
                                    return null;
                                }
                        }
                        break;
                    }
                case 0x23:
                    {
                        switch (ServerState)
                        {
                            case ServerState.PLAY:
                                {
                                    Console.WriteLine("Update Light");
                                    return null;
                                }
                        }
                        break;
                    }
            }

            return null;
        }*/

    }
}
