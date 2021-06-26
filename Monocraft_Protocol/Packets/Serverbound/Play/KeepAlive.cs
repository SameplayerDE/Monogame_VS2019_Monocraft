using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monocraft_Protocol.Packets.Serverbound.Play
{


    public class KeepAlive : ServerboundPacket
    {
        public readonly long KeepAliveID;

        public KeepAlive(long keepAliveID) : base(0x10) 
        {
            KeepAliveID = keepAliveID;
        }

        public override byte[] ToBytes(int compressionThreshold)
        {
            PacketStream stream = new PacketStream();
            if (compressionThreshold != -1 && compressionThreshold > 9)
            {
                stream.WriteByte(0x00); // No Compression
            }
            stream.WriteVarInt(PacketID);
            stream.WriteLong(KeepAliveID);
            return stream.ToArray();
        }
    }
}
