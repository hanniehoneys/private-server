﻿using PointBlank.Core.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointBlank.Auth.Network.ServerPacket
{
    public class PROTOCOL_LOBBY_QUICKJOIN_ROOM_ACK : SendPacket
    {
        public PROTOCOL_LOBBY_QUICKJOIN_ROOM_ACK()
        {

        }

        public override void write()
        {
            writeH(5378);
        }
    }
}
