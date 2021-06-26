namespace Monocraft_Protocol_NET.Packets
{
    public abstract class Packet
    {

        public readonly byte PacketID;

        public Packet(byte packetID)
        {
            PacketID = packetID;
        }

        public abstract byte[] ToBytes(int compressionThreshold);

    }
}