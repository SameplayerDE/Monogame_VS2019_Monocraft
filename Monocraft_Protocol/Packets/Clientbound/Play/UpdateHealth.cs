using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monocraft_Protocol.Packets.Clientbound.Play
{
    public class UpdateHealth : ClientboundPacket
    {
        public UpdateHealth() : base(0x49) { }

        public override byte[] ToBytes(int compressionThreshold)
        {
            return null;
        }
    }
}
