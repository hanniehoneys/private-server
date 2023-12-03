﻿using PointBlank.Core;
using PointBlank.Core.Managers.Events;
using PointBlank.Core.Models.Enums;
using PointBlank.Core.Network;
using PointBlank.Game.Data.Model;
using PointBlank.Game.Network.ServerPacket;
using System;

namespace PointBlank.Game.Network.ClientPacket
{
    public class PROTOCOL_BASE_ATTENDANCE_REQ : ReceivePacket
    {
        private EventErrorEnum erro = EventErrorEnum.VISIT_EVENT_SUCCESS;
        private int eventId;
        private int day;
        private EventVisitModel eventv;

        public PROTOCOL_BASE_ATTENDANCE_REQ(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            eventId = readD();
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
                if (p == null || string.IsNullOrEmpty(p.player_name))
                {
                    erro = EventErrorEnum.VISIT_EVENT_USERFAIL;
                }
                else if (p._event != null)
                {
                    int dateNow = int.Parse(DateTime.Now.ToString("yyMMdd"));
                    if (p._event.NextVisitDate <= dateNow)
                    {
                        eventv = EventVisitSyncer.getEvent(eventId);
                        if (eventv == null)
                        {
                            erro = EventErrorEnum.VISIT_EVENT_UNKNOWN;
                            goto Result;
                        }

                        if (eventv.EventIsEnabled())
                        {
                            p._event.NextVisitDate = int.Parse(DateTime.Now.AddDays(1).ToString("yyMMdd"));
                            ComDiv.updateDB("player_events", "player_id", p.player_id, new string[] { "next_visit_date", "last_visit_sequence1" }, p._event.NextVisitDate, ++p._event.LastVisitSequence1);
                            bool IsReward = false;
                            try
                            {
                                IsReward = eventv.box[p._event.LastVisitSequence2].reward1.IsReward;
                            }
                            catch
                            {

                            }
                            if (!IsReward)
                            {
                                ComDiv.updateDB("player_events", "last_visit_sequence2", ++p._event.LastVisitSequence2, "player_id", p.player_id);
                            }
                        }
                        else
                        {
                            erro = EventErrorEnum.VISIT_EVENT_WRONGVERSION;
                        }
                    }
                    else
                    {
                        erro = EventErrorEnum.VISIT_EVENT_ALREADYCHECK;
                    }
                }
                else
                {
                    erro = EventErrorEnum.VISIT_EVENT_UNKNOWN;
                }
            Result:
                _client.SendPacket(new PROTOCOL_BASE_ATTENDANCE_ACK(erro, eventv, p._event));
            }
            catch (Exception ex)
            {
                Logger.info("PROTOCOL_BASE_ATTENDANCE_REQ: " + ex.ToString());
            }
        }
    }
}