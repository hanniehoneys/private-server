﻿using PointBlank.Core;
using PointBlank.Core.Network;
using PointBlank.Game.Data.Model;
using System;

namespace PointBlank.Game.Network.ServerPacket
{
    public class PROTOCOL_LOBBY_GET_ROOMINFOADD_ACK : SendPacket
    {
        private Room room;
        private Account leader;

        public PROTOCOL_LOBBY_GET_ROOMINFOADD_ACK(Room room, Account leader)
        {
            this.room = room;
            this.leader = leader;
        }

        public override void write()
        {
            if (room == null || leader == null)
            {
                return;
            }
            writeH(3084);
            writeC(0);
            try
            {
                writeUnicode(leader.player_name, 66);
                writeC((byte)room.killtime);
                writeC((byte)(room.rounds - 1));
                writeH((ushort)room.getInBattleTime());
                writeC(room.Limit);
                writeC(room.WatchRuleFlag);
                writeH(room.BalanceType);
                writeB(new byte[16]);
                writeIP(leader.PublicIP);
            }
            catch (Exception ex)
            {
                writeC(0);
                writeUnicode("", 66);
                writeB(new byte[28]);
                Logger.warning("PROTOCOL_LOBBY_GET_ROOMINFOADD_ACK: " + ex.ToString());
            }
        }
    }
}