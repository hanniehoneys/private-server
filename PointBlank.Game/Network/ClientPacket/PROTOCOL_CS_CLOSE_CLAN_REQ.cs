﻿using PointBlank.Core;
using PointBlank.Core.Models.Account.Clan;
using PointBlank.Core.Network;
using PointBlank.Game.Data.Managers;
using PointBlank.Game.Data.Model;
using PointBlank.Game.Data.Sync.Server;
using PointBlank.Game.Network.ServerPacket;
using System;

namespace PointBlank.Game.Network.ClientPacket
{
    public class PROTOCOL_CS_CLOSE_CLAN_REQ : ReceivePacket
    {
        private uint erro;

        public PROTOCOL_CS_CLOSE_CLAN_REQ(GameClient client, byte[] data)
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
                Account player = _client._player;
                if (player != null)
                {
                    Clan clan = ClanManager.getClan(player.clanId);
                    if (clan._id > 0 && clan.owner_id == _client.player_id &&
                        ComDiv.deleteDB("clan_data", "clan_id", clan._id) &&
                        ComDiv.updateDB("accounts", "player_id", player.player_id, new string[] { "clan_id", "clanaccess", "clan_game_pt", "clan_wins_pt" }, 0, 0, 0, 0) &&
                        ClanManager.RemoveClan(clan))
                    {
                        player.clanId = 0;
                        player.clanAccess = 0;
                        SendClanInfo.Load(clan, 1);
                    }
                    else
                    {
                        erro = 2147487850;
                    }
                }
                else
                {
                    erro = 2147487850;
                }
                _client.SendPacket(new PROTOCOL_CS_CLOSE_CLAN_ACK(erro));
            }
            catch (Exception ex)
            {
                Logger.info("PROTOCOL_CS_CLOSE_CLAN_REQ: " + ex.ToString());
            }
        }
    }
}