using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

using Server.Universe;

namespace Server.Contact
{
    class Server
    {
        World world;
        WSListener listener;
        Thread live;
        Thread start;
        int stuck;
        bool running;
        ClientData[] clients;

        public Server(string data)
        {
            switch (data)
            {
                case "debug":
                case "simpleWorld":
                    world = new Universe.Mods.SimpleWorld(data);
                    break;

                case "apocalypse":
                    world = new Universe.Mods.Apocalypse(data);
                    break;
            }

            running = false;
            clients = new ClientData[Constants.MaxPlayers];
            stuck = 0;
        }

        public void Start()
        {
            running = true;
            start = new Thread(TStart);
            start.Start();
            live = new Thread(Live);
            live.Start();
        }

        void TStart()
        {
            listener = new WSListener(Constants.Port, Constants.MaxPlayers);
            for (int i = 0; true; i = (i + 1) % Constants.MaxPlayers)
            {
                if (clients[i] == null)
                {
                    try
                    {
                        clients[i] = new ClientData("", listener.Accept());
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        running = false;
                        return;
                    }
                }
                Thread.Sleep(Constants.JoinDelay);
            }
        }

        void Live()
        {
            while (running)
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();

                lock (world)
                {
                    world.Live();
                    CheckClients();
                }

                sw.Stop();
                int t = Convert.ToInt32(sw.ElapsedMilliseconds);
                if (t < Constants.GameSpeedDelay)
                    Thread.Sleep(Constants.GameSpeedDelay - t);
                else
                    ++stuck;
            }
        }

        public void Stop()
        {
            running = false;
            listener.Stop();
        }

        void CheckClients()
        {
            lock (clients)
            {
                for (int i = 0; i < clients.Length; ++i)
                {
                    if (clients[i] == null)
                        continue;

                    if (!listener.HasSent(clients[i].socketID))
                        if (clients[i].fromAliveCheck < Constants.CheckAliveDelay / Constants.GameSpeedDelay)
                        {
                            ++clients[i].fromAliveCheck;
                            continue;
                        }

                    clients[i].fromAliveCheck = 0;

                    if (!Handle(clients[i]))
                    {
                        Remove(i);
                        continue;
                    }

                    try
                    {
                        Bytes packet = Bytes.IntToBytes(1);

                        lock (world)
                            world.GetBytes(clients[i].name).GetBytes(packet);

                        listener.Send(clients[i].socketID, packet);
                    }
                    catch(Exception e)
                    {
                        Remove(i);
                    }
                }
            }
        }

        void Remove(int index)
        {
            if (clients[index].name.Length <= Constants.NamesMaxLength && clients[index].name.Length > 0)
                world.RemovePlayer(clients[index].name);

            listener.Close(index);
            clients[index] = null;
        }

        bool Handle(ClientData cd)
        {
            Bytes b;
            try
            {
                if (cd.name == "") //new player
                {
                    b = listener.Read(cd.socketID, 1);
                    string name = b.NextString();

                    if (name.Length > Constants.MaxPlayers || name.Length == 0)
                    {
                        cd.goodbye = true;
                        listener.Close(cd.socketID);
                        return false;
                    }

                    lock (world)
                    {
                        cd.name = world.AddPlayer(name);
                        Bytes t = Bytes.IntToBytes(0);
                        t.Add(Tangible.Model.GetModelsBytes());
                        listener.Send(cd.socketID, t);
                    }

                    Console.WriteLine(cd.name + " has joined the game.");
                }

                if (listener.HasSent(cd.socketID))
                {
                    b = listener.Read(cd.socketID);

                    switch ((FirstInt)b.NextInt())
                    {
                        case FirstInt.Update: //update
                            lock (world)
                                world.UpdatePlayer(cd.name, b);
                            break;

                        case FirstInt.Quit: //leave the game       

                            return false;

                        case FirstInt.Message: //send message
                            lock (world)
                                world.SendMessage(cd.name, b.NextString());
                            break;
                        default:
                            Console.WriteLine();
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }

        public bool Running { get { return running; } }

        public int Stuck { get { return stuck; } }
    }
}
