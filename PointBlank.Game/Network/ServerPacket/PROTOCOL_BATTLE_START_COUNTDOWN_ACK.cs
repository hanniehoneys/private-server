﻿using PointBlank.Core.Models.Enums;
using PointBlank.Core.Network;

namespace PointBlank.Game.Network.ServerPacket
{
    public class PROTOCOL_BATTLE_START_COUNTDOWN_ACK : SendPacket
    {
        private CountDownEnum type;

        public PROTOCOL_BATTLE_START_COUNTDOWN_ACK(CountDownEnum timer)
        {
            type = timer;
        }

        public override void write()
        {
            writeH(4102);
            writeC((byte)type);
        }
    }
}