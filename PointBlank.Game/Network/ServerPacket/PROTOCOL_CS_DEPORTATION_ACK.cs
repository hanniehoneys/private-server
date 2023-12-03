﻿using PointBlank.Core.Network;

namespace PointBlank.Game.Network.ServerPacket
{
    public class PROTOCOL_CS_DEPORTATION_ACK : SendPacket
    {
        private uint result;

        public PROTOCOL_CS_DEPORTATION_ACK(uint result)
        {
            this.result = result;
        }

        public override void write()
        {
            writeH(1855);
            writeD(result);
        }
    }
}