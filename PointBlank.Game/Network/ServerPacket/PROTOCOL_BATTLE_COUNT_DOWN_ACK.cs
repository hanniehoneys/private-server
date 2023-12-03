﻿using PointBlank.Core.Network;

namespace PointBlank.Game.Network.ServerPacket
{
    public class PROTOCOL_BATTLE_COUNT_DOWN_ACK : SendPacket
    {
        private int Seconds;
        public PROTOCOL_BATTLE_COUNT_DOWN_ACK(int Seconds)
        {
            this.Seconds = Seconds;
        }

        public override void write()
        {
			return;
            writeH(4251);
            writeC((byte)Seconds);
        }
    }
}