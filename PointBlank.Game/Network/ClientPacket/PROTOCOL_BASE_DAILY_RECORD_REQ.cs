﻿using PointBlank.Core;
using PointBlank.Game.Data.Model;
using PointBlank.Game.Network.ServerPacket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointBlank.Game.Network.ClientPacket
{
    public class PROTOCOL_BASE_DAILY_RECORD_REQ : ReceivePacket
    {
        public PROTOCOL_BASE_DAILY_RECORD_REQ(GameClient Client, byte[] Buffer)
        {
            makeme(Client, Buffer);
        }

        public override void read()
        {
            
        }

        public override void run()
        {
            try
            {
                Account Player = _client._player;
                if (Player.Daily != null)
                {
                    _client.SendPacket(new PROTOCOL_BASE_DAILY_RECORD_ACK(Player.Daily));
                }
            }
            catch (Exception ex)
            {
                Logger.error("PROTOCOL_BASE_DAILY_RECORD_REQ: " + ex.ToString());
            }
        }
    }
}
