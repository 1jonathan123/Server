using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    //the first int in a packet from a client
    enum FirstInt { Update, Quit, Message }

    //the second int in a packet from a client, after update
    enum SecondInt { KeyDown, KeyUp }

    static class Constants
    {
        public const int BlockSize = 75;
        public const int Dimentions = 2; //this game is 2d

        public const int WeaponsPerHand = 2;
        public const int Arms = 2;
        public const int Arm1 = 0;
        public const int Arm2 = 1;

        public const int Port = 55555;
        public const int MaxPlayers = 10;
        public const int NamesMaxLength = 16;
        public const int GameSpeedDelay = 20;
        public const int JoinDelay = 10;
        public const int CheckAliveDelay = 500;

        public const char North = 'W';
        public const char South = 'S';
        public const char West = 'A';
        public const char East = 'D';

        public const string ModelsDirectory = "data/models";
        public const string MapsDirectory = "data/maps";
        public const string BlocksDirectory = "data/blocks";

        public const double Epsilon1 = 0.00000001; //a small enough number
        public const double Epsilon2 = 0.00001;

        public static readonly Tangible.Vector ScreenSize = new Tangible.Vector(1920, 1080);
    }
}
