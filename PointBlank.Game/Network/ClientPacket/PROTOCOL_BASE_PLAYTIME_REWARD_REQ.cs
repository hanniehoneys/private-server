﻿using PointBlank.Core;
using PointBlank.Core.Managers;
using PointBlank.Core.Managers.Events;
using PointBlank.Core.Models.Account.Players;
using PointBlank.Core.Models.Shop;
using PointBlank.Core.Network;
using PointBlank.Game.Data.Model;
using PointBlank.Game.Network.ServerPacket;
using System;

namespace PointBlank.Game.Network.ClientPacket
{
    public class PROTOCOL_BASE_PLAYTIME_REWARD_REQ : ReceivePacket
    {
        private int goodId;

        public PROTOCOL_BASE_PLAYTIME_REWARD_REQ(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            goodId = readD();
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
                PlayerEvent pev = player._event;
                GoodItem good = ShopManager.getGood(goodId);
                if (good == null || pev == null)
                {
                    return;
                }
                PlayTimeModel eventPt = EventPlayTimeSyncer.getRunningEvent();
                if (eventPt != null)
                {
                    long count = eventPt.GetRewardCount(goodId);
                    if (pev.LastPlaytimeFinish == 1 && count > 0 && ComDiv.updateDB("player_events", "last_playtime_finish", 2, "player_id", _client.player_id))
                    {
                        pev.LastPlaytimeFinish = 2;
                        _client.SendPacket(new PROTOCOL_INVENTORY_GET_INFO_ACK(0, player, new ItemsModel(good._item._id, good._item._category, "PlayTime Reward", good._item._equip, count)));
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.info("PROTOCOL_BASE_PLAYTIME_REWARD_REQ: " + ex.ToString());
            }
        }
    }
}