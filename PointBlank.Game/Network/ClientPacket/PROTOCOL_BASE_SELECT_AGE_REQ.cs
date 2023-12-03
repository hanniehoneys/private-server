﻿using PointBlank.Core.Network;
using PointBlank.Game.Data.Model;
using PointBlank.Game.Network.ServerPacket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointBlank.Game.Network.ClientPacket
{
    public class PROTOCOL_BASE_SELECT_AGE_REQ : ReceivePacket
    {
        private int Year;

        public PROTOCOL_BASE_SELECT_AGE_REQ(GameClient Client, byte[] Buffer)
        {
            makeme(Client, Buffer);
        }

        public override void read()
        {
            Year = readD();
        }

        public override void run()
        {
            Account Player = _client._player;
            int YearNow = int.Parse(DateTime.Now.ToString("yyyy"));
            int Result = YearNow - (Year / 10000);
            Player.age = Result;
            ComDiv.updateDB("accounts", "age", Result, "player_id", Player.player_id);
            _client.SendPacket(new PROTOCOL_BASE_SELECT_AGE_ACK(0, Result));
        }
    }
}
