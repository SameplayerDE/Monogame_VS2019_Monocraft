namespace Monocraft_Protocol.Packets
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