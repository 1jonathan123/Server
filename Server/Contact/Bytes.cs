using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.Contact
{
    class Bytes
    {
        public LinkedList<byte> list;

        public Bytes()
        {
            list = new LinkedList<byte>();
        }

        public void Reset()
        {
            list = new LinkedList<byte>();
        }

        public Bytes Cut(int after)
        {
            Bytes b = new Bytes(list.Take(after).ToList());

            list = new LinkedList<byte>(list.Skip(after));

            return b;
        }

        public Bytes(LinkedList<byte> list)
        {
            this.list = new LinkedList<byte>(list);
        }

        public Bytes(List<byte> list)
        {
            this.list = new LinkedList<byte>(list);
        }

        public void Add(int a)
        {
            while ((a & -128) != 0)
            {
                list.AddLast((byte)(a & 127 | 128));
                a = (int)(((uint)a) >> 7);
            }

            list.AddLast((byte)a);
        }

        public void Add(string a)
        {
            byte[] value = Encoding.UTF8.GetBytes(a);
            Add(value.Length);

            for (int i = 0; i < value.Length; ++i)
                list.AddLast(value[i]);
        }

        public void Add(double a)
        {
            Add(Convert.ToInt32(a * 1000));
        }

        public void Add(Tangible.Vector v)
        {
            Add(v.x);
            Add(v.y);
        }

        public void Add(Bytes b)
        {
            byte[] t = b.list.ToArray();

            for (int i = 0; i < t.Length; ++i)
                list.AddLast(t[i]);
        }

        public static bool operator ==(Bytes a, Bytes b)
        {
            if (a is null && b is null)
                return true;

            if (a is null || b is null)
                return false;

            return a.list.SequenceEqual(b.list);
        }

        public static bool operator !=(Bytes a, Bytes b)
        {
            if (a is null && b is null)
                return false;

            if (a is null || b is null)
                return true;

            return !a.list.SequenceEqual(b.list);
        }

        public Tangible.Vector NextVector()
        {
            return new Tangible.Vector(NextDouble(), NextDouble());
        }

        public string NextString()
        {
            return ReadString(list);
        }

        public int NextInt()
        {
            return ReadVarInt(list);
        }

        public double NextDouble()
        {
            return NextInt() / 1000.0;
        }

        public static Bytes StringToBytes(string str)
        {
            return new Bytes(GetString(str).ToList());
        }

        public static Bytes IntToBytes(int a)
        {
            return new Bytes(GetVarInt(a).ToList());
        }

        public static Bytes DoubleToBytes(double a)
        {
            return new Bytes(GetVarInt(Convert.ToInt32(1000 * a)).ToList());
        }

        public static Bytes operator +(Bytes a, Bytes b)
        {
            return new Bytes(Concat(a.list.ToArray(), b.list.ToArray()).ToList());
        }

        public int Count { get { return list.Count; } }

        static byte[] Concat(params byte[][] bytes)
        {
            List<byte> result = new List<byte>();
            foreach (byte[] array in bytes)
                result.AddRange(array);

            return result.ToArray();
        }

        static string ReadString(LinkedList<byte> cache)
        {
            int length = ReadVarInt(cache);
            if (length > 0)
                return Encoding.UTF8.GetString(ReadData(length, cache));
            else return "";
        }

        static byte[] ReadData(int offset, LinkedList<byte> cache)
        {
            if (cache.Count < offset)
                throw new Exception();

            byte[] result = cache.Take(offset).ToArray();

            for (int i = 0; i < offset; ++i)
                cache.RemoveFirst();
            return result;
        }

        static int ReadVarInt(LinkedList<byte> cache)
        {
            int i = 0;
            int j = 0;
            int k = 0;
            do
            {
                k = ReadNextByte(cache);
                i |= (k & 0x7F) << j++ * 7;
            } while ((k & 0x80) != 0);
            return i;
        }

        static byte[] ReadByteArray(LinkedList<byte> cache)
        {
            int len = ReadVarInt(cache);
            return ReadData(len, cache);
        }

        static byte ReadNextByte(LinkedList<byte> cache)
        {
            byte result = cache.First.Value;
            cache.RemoveFirst();
            return result;
        }

        static byte[] GetVarInt(int paramInt)
        {
            List<byte> bytes = new List<byte>();

            while ((paramInt & -128) != 0)
            {
                bytes.Add((byte)(paramInt & 127 | 128));
                paramInt = (int)(((uint)paramInt) >> 7);
            }

            bytes.Add((byte)paramInt);
            return bytes.ToArray();
        }

        static byte[] GetString(string str)
        {
            byte[] value = Encoding.UTF8.GetBytes(str);
            return Concat(GetVarInt(value.Length), value);
        }
    }
}
