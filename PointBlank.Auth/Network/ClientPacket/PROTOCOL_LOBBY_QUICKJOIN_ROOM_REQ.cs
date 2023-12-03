﻿using PointBlank.Auth.Network.ServerPacket;
using PointBlank.Core;
using PointBlank.Core.Models.Servers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointBlank.Auth.Network.ClientPacket
{
    public class PROTOCOL_LOBBY_QUICKJOIN_ROOM_REQ : ReceivePacket
    {
        private int Select;
        private List<QuickStart> Quicks = new List<QuickStart>();

        public PROTOCOL_LOBBY_QUICKJOIN_ROOM_REQ(AuthClient Client, byte[] Buffer)
        {
            makeme(Client, Buffer);
        }

        public override void read()
        {
            Select = readC();
            for (int i = 0; i < 3; i++)
            {
                QuickStart Quick = new QuickStart();
                Quick.MapId = readC();
                Quick.Rule = readC();
                Quick.StageOptions = readC();
                Quick.Type = readC();
                Quicks.Add(Quick);
            }
        }

        public override void run()
        {
            try
            {
                _client.SendPacket(new PROTOCOL_SERVER_MESSAGE_ANNOUNCE_ACK("ระบบนี้ยังไม่เปิดใช้งาน"));
            }
            catch (Exception ex)
            {
                Logger.error("PROTOCOL_LOBBY_QUICKJOIN_ROOM_REQ: " + ex.ToString());
            }
        }
    }
}
