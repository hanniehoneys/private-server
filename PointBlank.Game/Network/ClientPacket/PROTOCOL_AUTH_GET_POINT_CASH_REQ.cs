﻿using PointBlank.Core;
using PointBlank.Game.Data.Model;
using PointBlank.Game.Network.ServerPacket;
using System;

namespace PointBlank.Game.Network.ClientPacket
{
    public class PROTOCOL_AUTH_GET_POINT_CASH_REQ : ReceivePacket
    {
        public PROTOCOL_AUTH_GET_POINT_CASH_REQ(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {

        }

        public override void run()
        {
            try
            {
                Account p = _client._player;
                if (p == null)
                {
                    return;
                }
                _client.SendPacket(new PROTOCOL_AUTH_GET_POINT_CASH_ACK(0, p._gp, p._money));
            }
            catch (Exception ex)
            {
                Logger.info(ex.ToString());
            }
        }
    }
}