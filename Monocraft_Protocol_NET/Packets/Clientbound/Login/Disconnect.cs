using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monocraft_Protocol_NET.Packets.Clientbound.Login
{
    public class Disconnect : ClientboundPacket
    {
        public Disconnect() : base(0x00) { }

        public override byte[] ToBytes(int compressionThreshold)
        {
            DataWriter writer = new DataWriter();
            return writer.Data;
        }
    }
}
