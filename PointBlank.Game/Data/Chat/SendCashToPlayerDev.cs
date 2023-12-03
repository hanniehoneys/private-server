﻿using PointBlank.Core;
using PointBlank.Core.Managers;
using PointBlank.Game.Network.ServerPacket;
using PointBlank.Game.Data.Managers;
using PointBlank.Game.Data.Model;
using PointBlank.Game.Data.Sync.Server;
using System;

namespace PointBlank.Game.Data.Chat
{
    public static class SendCashToPlayerDev
    {
        public static string SendCashToPlayer(string str)
        {           
            string txt = str.Substring(str.IndexOf(" ") + 1);
            string[] split = txt.Split(' ');
            long player_id = Convert.ToInt64(split[0]);
            int cash = Convert.ToInt32(split[1]);

            Account pR = AccountManager.getAccount(player_id, 0);
            if (pR == null)
            {
                return Translation.GetLabel("[*]SendCash_Fail4");
            }
            if (pR._money + cash > 999999999)
            {
                return Translation.GetLabel("[*]SendCash_Fail4");
            }
            if (PlayerManager.updateAccountCash(pR.player_id, pR._money + cash))
            {
                pR._money += cash;
                pR.SendPacket(new PROTOCOL_AUTH_GET_POINT_CASH_ACK(0, pR._gp, pR._money), false);
                SendItemInfo.LoadGoldCash(pR);
                return Translation.GetLabel("GiveCashSuccessD", pR._money, pR.player_name);
            }
            else
            {
                return Translation.GetLabel("GiveCashFail2");
            }
        }
    }
}