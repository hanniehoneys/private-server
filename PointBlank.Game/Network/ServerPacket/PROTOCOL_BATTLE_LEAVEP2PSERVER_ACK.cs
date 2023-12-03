﻿using PointBlank.Core.Network;
using PointBlank.Game.Data.Model;

namespace PointBlank.Game.Network.ServerPacket
{
    public class PROTOCOL_BATTLE_LEAVEP2PSERVER_ACK : SendPacket
    {
        private Room _r;

        public PROTOCOL_BATTLE_LEAVEP2PSERVER_ACK(Room room)
        {
            _r = room;
        }

        public override void write()
        {
            writeH(4125);
            writeD(_r._leader);
        }
    }
}