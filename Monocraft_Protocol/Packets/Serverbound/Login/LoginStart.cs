using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monocraft_Protocol.Packets.Serverbound.Login
{
    public class LoginStart : ServerboundPacket
    {

        public readonly string Username;

        public LoginStart(string username) : base(0x00)
        {
            Username = username;
        }

        public override byte[] ToBytes(int compressionThreshold)
        {
            PacketStream packetStream = new PacketStream();
            packetStream.WriteVarInt(PacketID);
            packetStream.WriteString(Username);
            return packetStream.ToPacket();
        }
    }
}
