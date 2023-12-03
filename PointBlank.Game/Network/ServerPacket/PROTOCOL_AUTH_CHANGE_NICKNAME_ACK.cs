﻿using PointBlank.Core.Network;
using System;

namespace PointBlank.Game.Network.ServerPacket
{
    public class PROTOCOL_AUTH_CHANGE_NICKNAME_ACK : SendPacket
    {
        private string name;

        public PROTOCOL_AUTH_CHANGE_NICKNAME_ACK(string name)
        {
            this.name = name;
        }

        public override void write()
        {
            writeH(812);
            writeC((byte)name.Length);
            writeUnicode(name, name.Length * 2);
            Console.WriteLine("Change Nick To : "+ name);
        }
    }
}