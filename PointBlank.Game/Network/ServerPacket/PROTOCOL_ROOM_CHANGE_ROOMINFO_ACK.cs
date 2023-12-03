﻿using PointBlank.Core.Network;
using PointBlank.Game.Data.Model;
using System.Net;

namespace PointBlank.Game.Network.ServerPacket
{
    public class PROTOCOL_ROOM_CHANGE_ROOMINFO_ACK : SendPacket
    {
        private Room room;
        private bool isBotMode;

        public PROTOCOL_ROOM_CHANGE_ROOMINFO_ACK(Room r)
        {
            room = r;
            if (room != null)
            {
                isBotMode = room.isBotMode();
            }
        }

        public PROTOCOL_ROOM_CHANGE_ROOMINFO_ACK(Room r, bool isBotMode)
        {
            room = r;
            this.isBotMode = isBotMode;
        }

        public override void write()
        {
            Account leader = room.getLeader();

            if (room == null)
            {
                return;
            }
            writeH(3857);
            writeD(room._roomId);
            writeUnicode(room.name, 46);
            writeC((byte)room.mapId);
            writeC((byte)room.rule);
            writeC(room.stage);
            writeC((byte)room.RoomType);
            writeC((byte)room.RoomState);
            writeC((byte)room.getAllPlayers().Count);
            writeC((byte)room.getSlotCount());
            writeC((byte)room._ping);
            writeH((short)room.weaponsFlag);
            writeD(room.getFlag());
            writeC(0);
            writeC(0);
            writeUnicode(leader != null ? leader.player_name : "", 66);
            writeD(room.killtime);
            writeC(room.Limit);
            writeC(room.WatchRuleFlag);
            writeH(room.BalanceType);
            writeB(new byte[16]);
            if (isBotMode)
            {
                writeC(room.aiCount);
                writeC(room.aiLevel);
            }
        }
    }
}