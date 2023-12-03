﻿using PointBlank.Core;
using PointBlank.Core.Managers;
using PointBlank.Core.Models.Enums;
using PointBlank.Core.Models.Room;
using PointBlank.Core.Network;
using PointBlank.Game.Data.Configs;
using PointBlank.Game.Data.Model;
using PointBlank.Game.Data.Sync.Client;
using PointBlank.Game.Data.Utils;
using PointBlank.Game.Network.ServerPacket;
using System;

namespace PointBlank.Game.Network.ClientPacket
{
    public class PROTOCOL_BATTLE_DEATH_REQ : ReceivePacket
    {
        private FragInfos kills = new FragInfos();
        private bool isSuicide;

        public PROTOCOL_BATTLE_DEATH_REQ(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            kills.killingType = (CharaKillType)readC();
            kills.killsCount = readC();
            kills.killerIdx = readC();
            kills.weapon = readD();
            kills.x = readT();
            kills.y = readT();
            kills.z = readT();
            kills.flag = readC();
            for (int i = 0; i < kills.killsCount; i++)
            {
                Frag frag = new Frag();
                frag.victimWeaponClass = readC();
                frag.SetHitspotInfo(readC());
                readH();
                frag.flag = readC();
                frag.x = readT();
                frag.y = readT();
                frag.z = readT();
                frag.AssistSlot = readC();
                kills.frags.Add(frag);
                if (frag.VictimSlot == kills.killerIdx)
                {
                    isSuicide = true;
                }
            }
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
                Room room = player._room;
                if (room == null || room.round.Timer != null || room.RoomState < RoomState.Battle)
                {
                    return;
                }
                bool isBotMode = room.isBotMode();
                Slot killer = room.getSlot(kills.killerIdx);
                if (killer == null || !isBotMode && (killer.state < SlotState.BATTLE || killer._id != player._slotId))
                {
                    return;
                }
                int score;
                RoomDeath.RegistryFragInfos(room, killer, out score, isBotMode, isSuicide, kills);
                if (isBotMode)
                {
                    killer.Score += killer.killsOnLife + room.IngameAiLevel + score;
                    if (killer.Score > 65535)
                    {
                        killer.Score = 65535;
                        if (ComDiv.updateDB("accounts", "access_level", -1, "player_id", player.player_id))
                        {
                            BanManager.SaveAutoBan(player.player_id, player.login, player.player_name, "ใช้โปรปั้มบอท", DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"), player.PublicIP.ToString(), "Ban from Server");
                            using (PROTOCOL_LOBBY_CHATTING_ACK packet = new PROTOCOL_LOBBY_CHATTING_ACK("AutoBan", 0, 53, false, "แบนผู้เล่น [" + player.player_name + "] ถาวร - ใช้โปรปั้มบอท"))
                            {
                                GameManager.SendPacketToAllClients(packet);
                            }
                            player.access = AccessLevel.Banned;
                            player.SendPacket(new PROTOCOL_AUTH_ACCOUNT_KICK_ACK(2), false);
                            player.Close(1000, true);
                        }
                        Logger.LogHack("[Nick: " + player.player_name + " PlayerId: " + player.player_id + " Login: " + player.login + "] reached the maximum score of BOT mode.");
                    }
                    kills.Score = killer.Score;
                }
                else
                {
                    killer.Score += score;
                    AllUtils.CompleteMission(room, player, killer, kills, MissionType.NA, 0);
                    kills.Score = score;
                }
                using (PROTOCOL_BATTLE_DEATH_ACK packet = new PROTOCOL_BATTLE_DEATH_ACK(room, kills, killer, isBotMode))
                {
                    room.SendPacketToPlayers(packet, SlotState.BATTLE, 0);
                }
                RoomDeath.EndBattleByDeath(room, killer, isBotMode, isSuicide);
            }
            catch (Exception ex)
            {
                Logger.info("PROTOCOL_BATTLE_DEATH_REQ: " + ex.ToString());
                _client.Close(0);
            }
        }
    }
}