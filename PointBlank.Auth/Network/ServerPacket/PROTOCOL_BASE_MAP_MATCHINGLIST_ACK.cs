﻿using PointBlank.Core.Models.Map;
using PointBlank.Core.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointBlank.Auth.Network.ServerPacket
{
    public class PROTOCOL_BASE_MAP_MATCHINGLIST_ACK : SendPacket
    {
        private List<MapMatch> Matchs;
        private int Total;

        public PROTOCOL_BASE_MAP_MATCHINGLIST_ACK(List<MapMatch> Matchs, int Total)
        {
            this.Matchs = Matchs;
            this.Total = Total;
        }

        public override void write()
        {
            writeH(671);
            writeH(0);
            writeC((byte)Matchs.Count);
            for (int i = 0; i < Matchs.Count; i++)
            {
                MapMatch Match = Matchs[i];
                writeD(Match.Mode);
                writeC((byte)Match.Id);
                writeC((byte)MapModel.getRule(Match.Mode).Rule);
                writeC((byte)MapModel.getRule(Match.Mode).StageOptions);
                writeC((byte)MapModel.getRule(Match.Mode).Conditions);
                writeC((byte)Match.Limit);
                writeC((byte)Match.Tag);
                writeC(0);
                writeC(0);
            }

            if (Matchs.Count != 100)
            {
                writeD(1);
            }
            else
            {
                writeD(0);
            }

            writeH((short)(Total - 100));
            writeH((short)MapModel.Matchs.Count);
        }
    }
}
