﻿using PointBlank.Core.Network;
using PointBlank.Game.Data.Model;

namespace PointBlank.Game.Network.ServerPacket
{
    public class PROTOCOL_BATTLE_MISSION_TUTORIAL_ROUND_END_ACK : SendPacket
    {
        private Room Room;

        public PROTOCOL_BATTLE_MISSION_TUTORIAL_ROUND_END_ACK(Room Room)
        {
            this.Room = Room;
        }

        public override void write()
        {
            writeH(4165);
            writeC(3);
            writeH((short)((Room.getTimeByMask() * 60) - Room.getInBattleTime()));
        }
    }
}