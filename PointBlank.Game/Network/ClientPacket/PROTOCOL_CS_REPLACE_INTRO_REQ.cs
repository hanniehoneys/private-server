﻿using PointBlank.Core;
using PointBlank.Core.Models.Account.Clan;
using PointBlank.Core.Network;
using PointBlank.Game.Data.Managers;
using PointBlank.Game.Data.Model;
using PointBlank.Game.Network.ServerPacket;
using System.Text;

namespace PointBlank.Game.Network.ClientPacket
{
    public class PROTOCOL_CS_REPLACE_INTRO_REQ : ReceivePacket
    {
        private string clan_info;
        private uint erro;

        public PROTOCOL_CS_REPLACE_INTRO_REQ(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            clan_info = readUnicode(readC() * 2);
        }

        public override void run()
        {
            try
            {
                Account p = _client._player;
                if (p != null)
                {
                    Clan c = ClanManager.getClan(p.clanId);
                    if (c._id > 0 && c._info != clan_info && (c.owner_id == _client.player_id || p.clanAccess >= 1 && p.clanAccess <= 2))
                    {
                        if (ComDiv.updateDB("clan_data", "clan_info", clan_info, "clan_id", c._id))
                        {
                            c._info = clan_info;
                        }
                        else
                        {
                            erro = 2147487860;
                        }
                    }
                    else
                    {
                        erro = 2147487835;
                    }
                }
                else
                {
                    erro = 2147487835;
                }
            }
            catch
            {
                erro = 2147487860;
            }
            _client.SendPacket(new PROTOCOL_CS_REPLACE_INTRO_ACK(erro));
        }
    }
}