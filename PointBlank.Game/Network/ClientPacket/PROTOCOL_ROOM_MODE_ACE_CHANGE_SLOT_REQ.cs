﻿using PointBlank.Core;
using PointBlank.Core.Models.Enums;
using PointBlank.Core.Models.Room;
using PointBlank.Game.Data.Model;
using PointBlank.Game.Data.Utils;
using PointBlank.Game.Network.ServerPacket;
using System;
using System.Collections.Generic;
using System.Threading;

namespace PointBlank.Game.Network.ClientPacket
{

    public class PROTOCOL_ROOM_MODE_ACE_CHANGE_SLOT_REQ : ReceivePacket
    {
        private int teamIdx;
        public PROTOCOL_ROOM_MODE_ACE_CHANGE_SLOT_REQ(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            teamIdx = readC();
        }

        public override void run()
        {
            try
            {
                Account player = _client._player;
                Room room = player == null ? null : player._room;
                if (room != null && !room.changingSlots)
                {
                    Slot slot = room.getSlot(player._slotId);
                    if (slot != null && slot.state == SlotState.NORMAL)
                    {
                        Monitor.Enter(room._slots);
                        room.changingSlots = true;
                        List<SlotChange> changeList = new List<SlotChange>();
                        room.SwitchNewSlot(changeList, player, slot, teamIdx, true);
                        if (changeList.Count > 0)
                        {
                            using (PROTOCOL_ROOM_TEAM_BALANCE_ACK packet = new PROTOCOL_ROOM_TEAM_BALANCE_ACK(changeList, room._leader, 0))
                            {
                                room.SendPacketToPlayers(packet);
                            }
                        }
                        room.changingSlots = false;
                        Monitor.Exit(room._slots);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.info("PROTOCOL_ROOM_MODE_ACE_CHANGE_SLOT: " + ex.ToString());
            }
        }
    }
}