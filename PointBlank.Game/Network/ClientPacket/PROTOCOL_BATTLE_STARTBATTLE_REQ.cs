﻿using PointBlank.Core;
using PointBlank.Core.Network;
using PointBlank.Core.Models.Enums;
using PointBlank.Core.Models.Room;
using PointBlank.Game.Data.Configs;
using PointBlank.Game.Data.Model;
using PointBlank.Game.Data.Utils;
using PointBlank.Game.Network.ServerPacket;
using System;

namespace PointBlank.Game.Network.ClientPacket
{
    public class PROTOCOL_BATTLE_STARTBATTLE_REQ : ReceivePacket
    {
        public TimerState countdown = new TimerState();
        public PROTOCOL_BATTLE_STARTBATTLE_REQ(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {

        }

        public override void run()
        {
            try
            {
                if (_client == null)
                {
                    return;
                }
                Account player = _client._player;
                Room room = player == null ? null : player._room;
                if (room != null && room.isPreparing())
                {
                    bool isBotMode = room.isBotMode();
                    Slot slot = room.getSlot(player._slotId);
                    if (slot == null)
                    {
                        return;
                    }
                    if (slot.state == SlotState.PRESTART)
                    {
                        room.changeSlotState(slot, SlotState.BATTLE_READY, true);
                        slot.StopTiming();
                        if (isBotMode)
                        {
                            _client.SendPacket(new PROTOCOL_BATTLE_CHANGE_DIFFICULTY_LEVEL_ACK(room));
                        }
                        _client.SendPacket(new PROTOCOL_ROOM_CHANGE_ROOMINFO_ACK(room, isBotMode));
                    }
                    else
                    {
                        _client.SendPacket(new PROTOCOL_SERVER_MESSAGE_KICK_BATTLE_PLAYER_ACK(EventErrorEnum.BATTLE_FIRST_HOLE));
                        _client.SendPacket(new PROTOCOL_BATTLE_GIVEUPBATTLE_ACK(player, 0));
                        room.changeSlotState(slot, SlotState.NORMAL, true);
                        AllUtils.BattleEndPlayersCount(room, isBotMode);
                        return;
                    }
                    int blue12 = 0, red12 = 0, total = 0, red9 = 0, blue9 = 0;
                    for (int i = 0; i < 16; i++)
                    {
                        Slot slotR = room._slots[i];
                        if ((int)slotR.state >= 10)
                        {
                            total++;
                            if (slotR._team == 0)
                            {
                                red9++;
                            }
                            else
                            {
                                blue9++;
                            }
                            if ((int)slotR.state >= 14)
                            {
                                if (slotR._team == 0)
                                {
                                    red12++;
                                }
                                else
                                {
                                    blue12++;
                                }
                            }
                        }
                    }

                    using (PROTOCOL_LOBBY_CHATTING_ACK packet = new PROTOCOL_LOBBY_CHATTING_ACK("Battle Server", 0, 6, false, player.player_name + " finished loading"))
                        room.SendPacketToPlayers(packet);

                    if (room.RoomState == RoomState.Battle ||
                        room._slots[room._leader].state >= SlotState.BATTLE_READY && isBotMode &&
                        (room._leader % 2 == 0 && red12 > red9 / 2 || room._leader % 2 == 1 && blue12 > blue9 / 2) ||
                        room._slots[room._leader].state >= SlotState.BATTLE_READY &&
                        ((!GameConfig.isTestMode || GameConfig.udpType != UdpState.RELAY) &&
                        blue12 > blue9 / 2 && red12 > red9 / 2 ||
                        GameConfig.isTestMode && GameConfig.udpType == UdpState.RELAY))
                    {
                        if(room.Countdown == false)
                        {
                            using (PROTOCOL_LOBBY_CHATTING_ACK packet = new PROTOCOL_LOBBY_CHATTING_ACK("Battle Server", 0, 3, false, "Game Start With " + (blue12 + red12) + " Players"))
                                room.SendPacketToPlayers(packet);
                        }
                        room.SpawnReadyPlayers(isBotMode);
                    }
                }
                else
                {
                    _client.SendPacket(new PROTOCOL_SERVER_MESSAGE_KICK_BATTLE_PLAYER_ACK(EventErrorEnum.BATTLE_FIRST_HOLE));
                    _client.SendPacket(new PROTOCOL_BATTLE_STARTBATTLE_ACK());
                    if (room != null)
                    {
                        room.changeSlotState(player._slotId, SlotState.NORMAL, true);
                    }
                    if (room == null && player != null)
                    {
                        _client.SendPacket(new PROTOCOL_LOBBY_ENTER_ACK());
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.warning("PROTOCOL_BATTLE_STARTBATTLE_REQ: " + ex.ToString());
            }
        }
    }
}