﻿using PointBlank.Core.Models.Account.Players;
using PointBlank.Core.Network;
using PointBlank.Game.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointBlank.Game.Network.ServerPacket
{
    public class PROTOCOL_BASE_DAILY_RECORD_ACK : SendPacket
    {
        private PlayerDailyRecord Record;

        public PROTOCOL_BASE_DAILY_RECORD_ACK(PlayerDailyRecord Record)
        {
            this.Record = Record;
        }

        public override void write()
        {
            writeH(623);
            writeH((short)Record.Total); // Total
            writeH((short)Record.Wins); // Wins
            writeH((short)Record.Loses); // Loses
            writeH((short)Record.Draws); // Draws
            writeH((short)Record.Kills); // Kills
            writeH((short)Record.Headshots); // Head Shots
            writeH((short)Record.Deaths); // Deaths
            writeD(Record.Exp); // EXP
            writeD(Record.Point); // Point
            writeD(0); // Play Time // 0
            writeC(1); // 0
            writeD(0); // ??? Play Time ??? //
            writeH(1); // ?
            writeD(0); // ?
            writeD(0); // ?
            writeD(0); // ?
            writeC(0); // ?
        }
    }
}
