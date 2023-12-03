﻿using PointBlank.Core;
using PointBlank.Core.Managers;
using PointBlank.Core.Models.Account.Clan;
using PointBlank.Core.Network;
using PointBlank.Game.Data.Managers;
using PointBlank.Game.Data.Model;
using PointBlank.Game.Network.ServerPacket;
using System;
using System.Collections.Generic;

namespace PointBlank.Game.Network.ClientPacket
{
    public class PROTOCOL_CS_REQUEST_LIST_REQ : ReceivePacket
    {
        private int page;

        public PROTOCOL_CS_REQUEST_LIST_REQ(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            page = readC();
        }

        public override void run()
        {
            try
            {
                Account player = _client._player;
                if (player == null)
                {
                    return;
                }
                if (player.clanId == 0)
                {
                    _client.SendPacket(new PROTOCOL_CS_REQUEST_LIST_ACK(-1));
                    return;
                }
                List<ClanInvite> clanInvites = PlayerManager.getClanRequestList(player.clanId);
                using (SendGPacket p = new SendGPacket())
                {
                    int count = 0;
                    int startid = 0;
                    if (page == 0)
                        startid = 13;
                    else
                        startid = 14;
                    for (int i = (page * startid); i < clanInvites.Count; i++)
                    {
                        WriteData(clanInvites[i], p);
                        if (++count == 13)
                        {
                            break;
                        }
                    }
                    _client.SendPacket(new PROTOCOL_CS_REQUEST_LIST_ACK(0, count, page, p.mstream.ToArray()));
                }
            }
            catch (Exception ex)
            {
                Logger.info("PROTOCOL_CS_REQUEST_LIST_REQ: " + ex.ToString());
            }
        }

        private void WriteData(ClanInvite invite, SendGPacket p)
        {
            p.writeQ(invite.player_id);
            Account pl = AccountManager.getAccount(invite.player_id, 0);
            if (pl != null)
            {
                p.writeUnicode(pl.player_name, 66);
                p.writeC((byte)pl._rank);
                p.writeC((byte)pl.name_color);
                p.writeD(invite.inviteDate);
                p.writeD(pl._statistic.kills_count);
                p.writeD(pl._statistic.deaths_count);
                p.writeD(pl._statistic.fights);
                p.writeD(pl._statistic.fights_win);
                p.writeD(pl._statistic.fights_lost);
                p.writeUnicode(invite.text, true);
            }
            p.writeD(invite.inviteDate);
        }
    }
}