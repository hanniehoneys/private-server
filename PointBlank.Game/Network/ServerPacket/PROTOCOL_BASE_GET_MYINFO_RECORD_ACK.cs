﻿using PointBlank.Core.Models.Account.Players;
using PointBlank.Core.Network;
using System;

namespace PointBlank.Game.Network.ServerPacket
{
    public class PROTOCOL_BASE_GET_MYINFO_RECORD_ACK : SendPacket
    {
        private PlayerStats s;

        public PROTOCOL_BASE_GET_MYINFO_RECORD_ACK(PlayerStats s)
        {
            this.s = s;
        }

        public override void write()
        {
            //Console.WriteLine("[RECEIVE]PROTOCOL_BASE_GET_MYINFO_RECORD_ACK");
            writeH(577);
            writeD(s.fights);
            writeD(s.fights_win);
            writeD(s.fights_lost);
            writeD(s.fights_draw);
            writeD(s.kills_count);
            writeD(s.headshots_count);
            writeD(s.deaths_count);
            writeD(s.totalfights_count);
            writeD(s.totalkills_count);
            writeD(s.escapes);
            writeD(s.assist);
            writeD(s.fights);
            writeD(s.fights_win);
            writeD(s.fights_lost);
            writeD(s.fights_draw);
            writeD(s.kills_count);
            writeD(s.headshots_count);
            writeD(s.deaths_count);
            writeD(s.totalfights_count);
            writeD(s.totalkills_count);
            writeD(s.escapes);
            writeD(s.assist);
        }
    }
}