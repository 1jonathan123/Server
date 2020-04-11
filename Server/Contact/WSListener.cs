using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Server.Contact
{
    class WSListener
    {
        TcpListener listener;
        Socket[] sockets;
        int counter;

        public WSListener(int port, int max)
        {
            listener = new TcpListener(port);
            listener.Start();
            sockets = new Socket[max];
            counter = 0;
        }

        public int Accept()
        {
            if (counter >= sockets.Length)
            {
                while (sockets[counter % sockets.Length] != null)
                {
                    ++counter;
                    Thread.Sleep(100);
                }

                counter = counter % sockets.Length;
            }

            sockets[counter] = listener.AcceptSocket();

            sockets[counter].SendTimeout = sockets[counter].ReceiveTimeout = 1000;

            for (int i = 0; i < 1000 && sockets[counter].Available == 0; ++i)
                Thread.Sleep(100);

            byte[] buffer = new byte[sockets[counter].Available];
            sockets[counter].Receive(buffer);

            string data = Encoding.UTF8.GetString(buffer);

            const string eol = "\r\n";

            byte[] response = Encoding.UTF8.GetBytes("HTTP/1.1 101 Switching Protocols" + eol
                + "Connection: Upgrade" + eol
                + "Upgrade: websocket" + eol
                + "Sec-WebSocket-Accept: " + Convert.ToBase64String(
                    System.Security.Cryptography.SHA1.Create().ComputeHash(
                        Encoding.UTF8.GetBytes(
                            new System.Text.RegularExpressions.Regex("Sec-WebSocket-Key: (.*)").Match(data).Groups[1].Value.Trim() + "258EAFA5-E914-47DA-95CA-C5AB0DC85B11"
                        )
                    )
                ) + eol
                + eol);

            sockets[counter].Send(response);

            ++counter;
            return counter - 1;
        }

        public Bytes Read(int index, int sleepTime = 0)
        {
            while (sockets[index].Available == 0)
            {
                if (!sockets[index].Connected)
                    throw new Exception();
                Thread.Sleep(sleepTime);
            }

            byte[] buffer = new byte[sockets[index].Available];
            sockets[index].Receive(buffer);

            byte[] key = buffer.Skip(2).Take(4).ToArray();
            byte[] msg = buffer.Skip(6).ToArray();
            byte[] final = new byte[msg.Length];

            for (int i = 0; i < msg.Length; i++)
                final[i] = (byte)(msg[i] ^ key[i % 4]);

            return new Bytes(final.ToList());
        }

        public void Send(int index, Bytes bytes)
        {
            if (!sockets[index].Connected)
                throw new Exception();

            byte[] first = new byte[0];

            if (bytes.list.Count <= 125)
                first = new byte[] { 130, (byte)bytes.list.Count };
            else
            {
                if (bytes.list.Count <= Math.Pow(2, 16))
                {
                    byte[] length = BitConverter.GetBytes(Convert.ToUInt16(bytes.list.Count));
                    first = new byte[] { 130, 126, length[1], length[0] };
                }
                else
                {
                    byte[] length = BitConverter.GetBytes(Convert.ToUInt64(bytes.list.Count));
                    Array.Reverse(length);
                    first = new byte[] { 130, 127, length[0], length[1], length[2], length[3], length[4], length[5], length[6], length[7] };
                }
            }

            List<byte> final = new List<byte>();
            final.AddRange(first);
            final.AddRange(bytes.list);
            sockets[index].Send(final.ToArray());
        }

        public void Stop()
        {
            listener.Stop();
        }

        public void Close(int index)
        {
            if (sockets[index].Connected)
                sockets[index].Close();
            sockets[index] = null;
        }

        public bool HasSent(int index)
        {
            return sockets[index].Available != 0;
        }
    }
}
