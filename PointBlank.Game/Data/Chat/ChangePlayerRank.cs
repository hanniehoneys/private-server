﻿using PointBlank.Core;
using PointBlank.Core.Models.Account.Rank;
using PointBlank.Core.Network;
using PointBlank.Core.Xml;
using PointBlank.Game.Data.Managers;
using PointBlank.Game.Data.Model;
using PointBlank.Game.Data.Sync.Server;
using PointBlank.Game.Network.ServerPacket;
using System;

namespace PointBlank.Game.Data.Chat
{
    public static class ChangePlayerRank
    {
        public static string SetPlayerRank(string str)
        {
            string text = str.Substring(str.IndexOf(" ") + 1);
            string[] split = text.Split(' ');
            long player_id = Convert.ToInt64(split[0]);
            int rank = Convert.ToInt32(split[1]);
            if (rank > 60 || rank == 56 || rank < 0 || player_id <= 0)
            {
                return Translation.GetLabel("ChangePlyRankWrongValue");
            }
            else
            {
                Account pE = AccountManager.getAccount(player_id, 0);
                if (pE != null)
                {
                    if (ComDiv.updateDB("accounts", "rank", rank, "player_id", pE.player_id))
                    {
                        RankModel model = RankXml.getRank(rank);
                        pE._rank = rank;
                        SendItemInfo.LoadGoldCash(pE);
                        pE.SendPacket(new PROTOCOL_BASE_RANK_UP_ACK(pE._rank, model != null ? model._onNextLevel : 0), false);
                        if (pE._room != null)
                        {
                            pE._room.updateSlotsInfo();
                        }
                        return Translation.GetLabel("ChangePlyRankSuccess", rank);
                    }
                    return Translation.GetLabel("ChangePlyRankFailUnk");
                }
                return Translation.GetLabel("ChangePlyRankFailPlayer");
            }
        }
    }
}