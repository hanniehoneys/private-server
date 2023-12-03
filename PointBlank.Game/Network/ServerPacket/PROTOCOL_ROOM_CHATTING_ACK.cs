﻿using PointBlank.Core.Network;

namespace PointBlank.Game.Network.ServerPacket
{
    public class PROTOCOL_ROOM_CHATTING_ACK : SendPacket
    {
        private string msg;
        private int type, slotId;
        private bool GMColor;

        public PROTOCOL_ROOM_CHATTING_ACK(int chat_type, int slotId, bool GM, string message)
        {
            type = chat_type;
            this.slotId = slotId;
            GMColor = GM;
            msg = message;
        }

        public override void write()
        {

            if(type <= 0)
            {
                writeH(3862);
                writeH((short)1);
                writeD(slotId);
                writeC(GMColor);
                writeD(msg.Length + 1);
                writeUnicode(msg, true);
            }
            writeH(3862);
            writeH((short)type);
            writeD(slotId);
            writeC(GMColor);
            writeD(msg.Length + 1);
            writeUnicode(msg, true);
        }
    }
}