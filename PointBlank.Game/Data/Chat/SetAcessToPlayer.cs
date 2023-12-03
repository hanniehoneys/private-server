﻿using PointBlank.Core;
using PointBlank.Core.Managers;
using PointBlank.Game.Network.ServerPacket;
using PointBlank.Game.Data.Managers;
using PointBlank.Game.Data.Model;
using System;
using PointBlank.Core.Models.Enums;

namespace PointBlank.Game.Data.Chat
{
    public static class SetAcessToPlayer
    {
        public static string SetAcessPlayer(string str)
        {           
            string txt = str.Substring(str.IndexOf(" ") + 1);
            string[] split = txt.Split(' ');
            long player_id = Convert.ToInt64(split[0]);
            int access = Convert.ToInt32(split[1]);

            Account pR = AccountManager.getAccount(player_id, 0);
            if (pR == null)
            {
                return Translation.GetLabel("[*]SetAcess_Fail4");
            }
            if (access < 0 || access > 6)
            {
                return Translation.GetLabel("[*]SetAcess_Fail4");
            }
            if (PlayerManager.updateAccountAccess(pR.player_id, access))
            {
                try
                {
                    pR.access = (AccessLevel)access;
                    return Translation.GetLabel("SetAcessS", access, pR.player_name);
                }
                catch
                {
                    return Translation.GetLabel("SetAcessF");
                }
            }
            else
            {
                return Translation.GetLabel("SetAcessF");
            }
        }        
    }
}