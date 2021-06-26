using Monocraft_Protocol.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Monocraft_Protocol
{
    public class DataReader
    {

        private byte[] _data;
        private int _index = 0;

        public byte[] Data { get { return _data; } }
        public int DataLength { get { return _data.Length; } }
        public int CurrentIndex { get { return _index; } }
        public int UnreadBytes { get { return DataLength - CurrentIndex; } }

        public DataReader() { }
        public DataReader(byte[] data)
        {
            _data = data;
        }

        public void SetData(byte[] data)
        {
            _data = data;
            _index = 0;
        }

        public Guid ReadUUID()
        {
            int length = 16;
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                builder.Append(_data[_index + i].ToString("X2"));
            }
            _index += length;
            return new Guid(builder.ToString());
        }

        public Position ReadPosition()
        {
            byte[] data = ReadUBytes(8);
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                builder.Append($"{data[i]:x2}");
            }

            ulong var = Convert.ToUInt64(builder.ToString(), 16);

            ulong x = var & 0xFFFFFFC000000000;
            ulong y = var & 0x3FFFFFF000;
            ulong z = var & 0xFFF;

            int o_x;
            int o_y;
            int o_z;

            x = x >> 38;
            if ((x & 0x2000000) > 0)
            {
                x = x ^ 0x3FFFFFF;
                o_x = -Convert.ToInt32(x);
            }
            else
            {
                o_x = Convert.ToInt32(x);
            }


            y = y >> 12;
            if ((y & 0x2000000) > 0)
            {
                y = y ^ 0x3FFFFFF;
                o_y = -Convert.ToInt32(y);
            }
            else
            {
                o_y = Convert.ToInt32(y);
            }

            if ((z & 0x800) > 0)
            {
                z = z ^ 0xFFF;
                o_z = -Convert.ToInt32(z);
            }
            else
            {
                o_z = Convert.ToInt32(z);
            }

            Console.WriteLine($"x: {o_x}/ y: {o_z} / z: {o_y}");
            return new Position(o_x, o_z, o_y);
        }

        public int SetData(NetworkStream stream)
        {
            byte[] data = new byte[short.MaxValue];
            int length = stream.Read(data, 0, data.Length);
            if (length > 0)
            {
                Array.Resize(ref data, length);
                SetData(data);
            }
            return length;
        }

        public bool ReadBoolean()
        {
            return Convert.ToBoolean(_data[_index++]);
        }

        public sbyte ReadByte()
        {
            return Convert.ToSByte(_data[_index++]);
        }

        public sbyte[] ReadBytes(int count)
        {
            sbyte[] data = new sbyte[count];
            for (int i = _index; i < count + _index; i++)
            {
                data[i] = Convert.ToSByte(_data[i]);
            }
            _index += count;
            return data;
        }

        public sbyte ReadAngle()
        {
            return Convert.ToSByte(_data[_index++]);
        }

        public double ReadDouble()
        {
            double value = BitConverter.ToDouble(_data, _index);
            _index += 8;
            return value;
        }

        public float ReadFloat()
        {
            float value = BitConverter.ToSingle(_data, _index);
            _index += 4;
            return value;
        }

        public byte ReadUByte()
        {
            return _data[_index++];
        }

        public byte[] ReadUBytes(int count)
        {
            byte[] data = _data.Skip(_index).Take(count).ToArray();
            _index += count;
            return data;
        }

        public long ReadAlexLong()
        {
            var l = ReadUBytes(8);
            return IPAddress.NetworkToHostOrder(BitConverter.ToInt64(l, 0));
        }

        public short ReadShort()
        {
            byte[] data = new byte[] { _data[_index++], _data[_index++] };
            Array.Reverse(data);
            short value = BitConverter.ToInt16(data, 0);
            return value;
        }

        public ushort ReadUShort()
        {
            byte[] data = new byte[] { _data[_index++], _data[_index++] };
            Array.Reverse(data);
            ushort value = BitConverter.ToUInt16(data, 0);
            return value;
        }

        public int ReadInt()
        {
            int value = BitConverter.ToInt32(_data, _index);
            _index += 4;
            return value;
        }

        public long ReadLong()
        {
            long value = BitConverter.ToInt64(_data, _index);
            _index += 8;
            return value;
        }

        public ulong ReadULong()
        {
            ulong value = BitConverter.ToUInt64(_data, _index);
            Console.WriteLine(value);
            _index += 8;
            return value;
        }

        public int ReadVarInt()
        {
            int numRead = 0;
            int result = 0;
            byte read;
            do
            {
                read = ReadUByte();
                int value = (read & 0b01111111);
                result |= (value << (7 * numRead));

                numRead++;
                if (numRead > 5)
                {
                    throw new Exception("VarInt is too big");
                }
            } while ((read & 0b10000000) != 0);
            return result;
        }

        public int ReadVarLong()
        {
            int value = BitConverter.ToInt32(_data, _index);
            _index += 4;
            return value;
        }

        public string ReadString()
        {
            int length = ReadVarInt();
            byte[] chars = new byte[length];
            for (int i = 0; i < length; i++)
            {
                chars[i] = _data[_index + i];
            }
            _index += length;
            return Encoding.Default.GetString(chars);
        }

        #region statics

        //public static ushort ReadShortReverse(byte )

        public static int GetMSB(short shortValue)
        {
            return (shortValue & 0xFF00);
        }

        public static int GetLSB(short shortValue)
        {
            return (shortValue & 0x00FF);
        }

        public static int GetMSB(ushort shortValue)
        {
            return (shortValue & 0xFF00);
        }

        public static int GetLSB(ushort shortValue)
        {
            return (shortValue & 0x00FF);
        }

        public static uint SwapBytes(uint x)
        {
            x = (x >> 16) | (x << 16);
            return ((x & 0xFF00FF00) >> 8) | ((x & 0x00FF00FF) << 8);
        }

        #endregion

    }
}
