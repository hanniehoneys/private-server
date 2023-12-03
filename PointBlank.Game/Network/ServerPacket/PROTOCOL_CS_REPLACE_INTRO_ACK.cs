﻿using PointBlank.Core.Network;

namespace PointBlank.Game.Network.ServerPacket
{
    public class PROTOCOL_CS_REPLACE_INTRO_ACK : SendPacket
    {
        private uint _erro;

        public PROTOCOL_CS_REPLACE_INTRO_ACK(uint erro)
        {
            _erro = erro;
        }

        public override void write()
        {
            writeH(1885);
            writeD(_erro);
        }
    }
}