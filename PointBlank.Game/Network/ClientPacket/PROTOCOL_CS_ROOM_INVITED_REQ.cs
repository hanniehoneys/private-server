﻿using PointBlank.Core;
using PointBlank.Game.Data.Managers;
using PointBlank.Game.Data.Model;
using PointBlank.Game.Network.ServerPacket;
using System;

namespace PointBlank.Game.Network.ClientPacket
{
    public class PROTOCOL_CS_ROOM_INVITED_REQ : ReceivePacket
    {
        private long pId;

        public PROTOCOL_CS_ROOM_INVITED_REQ(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            pId = readQ();
        }

        public override void run()
        {
            try
            {
                Account player = _client._player;
                if (player == null || player.clanId == 0)
                {
                    return;
                }
                Account member = AccountManager.getAccount(pId, 0);
                if (member != null && member.clanId == player.clanId)
                {
                    member.SendPacket(new PROTOCOL_CS_ROOM_INVITED_RESULT_ACK(_client.player_id), false);
                }
                player.SendPacket(new PROTOCOL_CS_ROOM_INVITED_ACK(0));
            }
            catch (Exception ex)
            {
                Logger.info(ex.ToString());
            }
        }
    }
}