﻿using PointBlank.Core.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointBlank.Auth.Network.ServerPacket
{
    public class PROTOCOL_BASE_BOOSTEVENT_INFO_ACK : SendPacket
    {
        public PROTOCOL_BASE_BOOSTEVENT_INFO_ACK()
        {

        }

        public override void write()
        {
            writeH(676);
            writeQ(1);
            writeC(0);
        }
    }
}
