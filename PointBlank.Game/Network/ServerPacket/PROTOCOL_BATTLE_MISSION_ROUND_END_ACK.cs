﻿using PointBlank.Core.Models.Enums;
using PointBlank.Core.Network;
using PointBlank.Game.Data.Model;

namespace PointBlank.Game.Network.ServerPacket
{
    public class PROTOCOL_BATTLE_MISSION_ROUND_END_ACK : SendPacket
    {
        private Room _room;
        private int _winner;
        private RoundEndType _reason;

        public PROTOCOL_BATTLE_MISSION_ROUND_END_ACK(Room room, int winner, RoundEndType reason)
        {
            _room = room;
            _winner = winner;
            _reason = reason;
        }

        public PROTOCOL_BATTLE_MISSION_ROUND_END_ACK(Room room, TeamResultType winner, RoundEndType reason)
        {
            _room = room;
            _winner = (int)winner;
            _reason = reason;
        }

        public override void write()
        {
            writeH(4131);
            writeC((byte)_winner);
            writeC((byte)_reason);
            if (_room.RoomType == RoomType.Boss || _room.RoomType == RoomType.CrossCounter)
            {
                writeH((ushort)_room.red_dino);
                writeH((ushort)_room.blue_dino);
            }
            else if (_room.RoomType == RoomType.DeathMatch || _room.RoomType == RoomType.FreeForAll)
            {
                writeH((ushort)_room._redKills);
                writeH((ushort)_room._blueKills);
            }
            else
            {
                writeH((ushort)_room.red_rounds);
                writeH((ushort)_room.blue_rounds);
            }
        }
    }
}