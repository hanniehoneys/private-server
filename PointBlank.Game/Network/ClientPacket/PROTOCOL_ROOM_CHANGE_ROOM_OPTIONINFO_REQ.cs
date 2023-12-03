﻿using PointBlank.Core;
using PointBlank.Core.Models.Enums;
using PointBlank.Game.Data.Model;
using PointBlank.Game.Network.ServerPacket;
using System;

namespace PointBlank.Game.Network.ClientPacket
{
    public class PROTOCOL_ROOM_CHANGE_ROOM_OPTIONINFO_REQ : ReceivePacket
    {
        private string leader;

        public PROTOCOL_ROOM_CHANGE_ROOM_OPTIONINFO_REQ(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            try
            {
                Account player = _client._player;
                Room room = player == null ? null : player._room;
                if (room != null && room._leader == player._slotId && room.RoomState == RoomState.Ready)
                {
                    leader = readS(32);
                    room.killtime = readD();
                    Logger.warning("killtime2 : " + room.killtime);
                    room.Limit = readC();
                    room.WatchRuleFlag = readC();
                    if (room.RoomType == RoomType.Ace)
                        room.WatchRuleFlag = 142;
                    room.BalanceType = readH();
                    if (room.RoomType == RoomType.Ace)
                        room.BalanceType = 0;
                    using (PROTOCOL_ROOM_CHANGE_ROOM_OPTIONINFO_ACK packet = new PROTOCOL_ROOM_CHANGE_ROOM_OPTIONINFO_ACK(room, leader))
                    {
                        room.SendPacketToPlayers(packet);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.info(ex.ToString());
            }
        }

        public override void run()
        {

        }
    }
}