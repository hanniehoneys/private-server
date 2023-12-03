﻿using PointBlank.Core;
using PointBlank.Core.Managers;
using PointBlank.Core.Managers.Events;
using PointBlank.Core.Models.Account;
using PointBlank.Core.Models.Account.Players;
using PointBlank.Core.Models.Enums;
using PointBlank.Core.Models.Shop;
using PointBlank.Core.Network;
using PointBlank.Game.Data.Model;
using PointBlank.Game.Network.ServerPacket;
using System;

namespace PointBlank.Game.Network.ClientPacket
{
    public class PROTOCOL_BASE_ATTENDANCE_CLEAR_ITEM_REQ : ReceivePacket
    {
        private EventErrorEnum erro = EventErrorEnum.VISIT_EVENT_SUCCESS;
        private int eventId, type, day;

        public PROTOCOL_BASE_ATTENDANCE_CLEAR_ITEM_REQ(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            eventId = readD();
            type = readC();
            day = readC();
        }

        public override void run()
        {
            try
            {
                if (_client == null)
                {
                    return;
                }
                Account p = _client._player;
                if (p == null || p.player_name.Length == 0 || type > 1)
                {
                    erro = EventErrorEnum.VISIT_EVENT_USERFAIL;
                }
                else if (p._event != null)
                {
                    if (p._event.LastVisitSequence1 == p._event.LastVisitSequence2)
                    {
                        erro = EventErrorEnum.VISIT_EVENT_ALREADYCHECK;
                    }
                    else
                    {
                        EventVisitModel eventv = EventVisitSyncer.getEvent(eventId);
                        if (eventv == null)
                        {
                            erro = EventErrorEnum.VISIT_EVENT_UNKNOWN;
                            goto Result;
                        }

                        if (eventv.EventIsEnabled())
                        {
                            VisitItem chI = eventv.getReward(p._event.LastVisitSequence2, type);
                            if (chI != null)
                            {
                                GoodItem good = ShopManager.getGood(chI.good_id);
                                if (good != null)
                                {
                                    p._event.NextVisitDate = int.Parse(DateTime.Now.AddYears(-10).AddDays(1).ToString("yyMMdd"));
                                    ComDiv.updateDB("player_events", "player_id", p.player_id, new string[] { "next_visit_date", "last_visit_sequence2" }, p._event.NextVisitDate, ++p._event.LastVisitSequence2);

                                    _client.SendPacket(new PROTOCOL_INVENTORY_GET_INFO_ACK(0, p, new ItemsModel(good._item._id, good._item._category, good._item._name, good._item._equip, chI.count)));
                                    _client.SendPacket(new PROTOCOL_SERVER_MESSAGE_ANNOUNCE_ACK(Translation.GetLabel("Attendance Checked!!!")));
                                }
                                else
                                {
                                    erro = EventErrorEnum.VISIT_EVENT_NOTENOUGH;
                                }
                            }
                            else
                            {
                                erro = EventErrorEnum.VISIT_EVENT_UNKNOWN;
                            }
                        }
                        else
                        {
                            erro = EventErrorEnum.VISIT_EVENT_WRONGVERSION;
                        }
                    }
                }
                else
                {
                    erro = EventErrorEnum.VISIT_EVENT_UNKNOWN;
                }
            Result:
                _client.SendPacket(new PROTOCOL_BASE_ATTENDANCE_CLEAR_ITEM_ACK(erro));
            }
            catch (Exception ex)
            {
                Logger.info("PROTOCOL_BASE_ATTENDANCE_CLEAR_ITEM_REQ: " + ex.ToString());
            }
        }
    }
}