﻿using PointBlank.Core.Network;
using PointBlank.Game.Data.Model;

namespace PointBlank.Game.Network.ServerPacket
{
    public class PROTOCOL_BATTLE_MISSION_TOUCHDOWN_COUNT_ACK : SendPacket
    {
        private Room _r;

        public PROTOCOL_BATTLE_MISSION_TOUCHDOWN_COUNT_ACK(Room r)
        {
            _r = r;
        }

        public override void write()
        {
            writeH(4159);
            writeH((ushort)_r.red_dino);
            writeH((ushort)_r.blue_dino);
        }
    }
}