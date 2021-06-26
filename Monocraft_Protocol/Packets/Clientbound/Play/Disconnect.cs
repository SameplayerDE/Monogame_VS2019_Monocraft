using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monocraft_Protocol.Packets.Clientbound.Play
{
    public class Disconnect : ClientboundPacket
    {

        public readonly string Reason;

        public Disconnect() : base(0x19) { }
        public Disconnect(DataReader reader) : base(0x19)
        {
            Reason = reader.ReadString();
        }

        public override byte[] ToBytes(int compressionThreshold)
        {
            throw new NotImplementedException();
        }
    }
}
