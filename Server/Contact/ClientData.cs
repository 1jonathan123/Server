using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Contact
{
    class ClientData
    {
        public string name;
        public int socketID;
        public bool goodbye;
        public int fromAliveCheck;

        public ClientData(string name, int socketID)
        {
            this.name = name;
            this.socketID = socketID;
            goodbye = false;
            fromAliveCheck = 0;
        }
    }
}
