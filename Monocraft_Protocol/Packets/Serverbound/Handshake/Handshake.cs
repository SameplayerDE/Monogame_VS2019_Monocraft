using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monocraft_Protocol.Packets.Serverbound.Handshake
{

    public enum State
    {
        STATUS = 0x01,
        LOGIN = 0x02
    }

    public class Handshake : ServerboundPacket
    {

        public readonly int Version;
        public readonly string Host;
        public readonly ushort Port;
        public readonly State State;

        public Handshake() : base(0x00) { }
        public Handshake(DataReader reader) : base(0x00)
        {
            Version = reader.ReadVarInt();
            Host = reader.ReadString();
            Port = reader.ReadUShort();
            State = (State)reader.ReadVarInt();
        }
        public Handshake(byte[] data) : base(0x00)
        {
            DataReader reader = new DataReader(data);
            Version = reader.ReadVarInt();
            Host = reader.ReadString();
            Port = reader.ReadUShort();
            State = (State)reader.ReadVarInt();
        }
        public Handshake(int version, string host, ushort port, State state) : base(0x00)
        {
            Version = version;
            Host = host;
            Port = port;
            State = state;
        }
        public Handshake(int version, string host, ushort port, int state) : base(0x00)
        {
            Version = version;
            Host = host;
            Port = port;
            State = (State)state;
        }

        public override byte[] ToBytes(int compressionThreshold)
        {
            PacketStream packetStream = new PacketStream();
            packetStream.WriteVarInt(PacketID);
            packetStream.WriteVarInt(Version);
            packetStream.WriteString(Host);
            packetStream.WriteUnsignedShort(Port);
            packetStream.WriteVarInt((int)State);
            return packetStream.ToPacket();
        }
    }
}
