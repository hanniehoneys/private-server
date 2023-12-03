﻿using PointBlank.Core.Network;
using System;

namespace PointBlank.Game.Network.ServerPacket
{
    public class PROTOCOL_INVENTORY_LEAVE_ACK : SendPacket
    {
        private int erro;

        public PROTOCOL_INVENTORY_LEAVE_ACK(int erro)
        {
            this.erro = erro;
        }

        public override void write()
        {
            writeH(3332);
            writeH(0);
            writeD(0);
        }
    }
}