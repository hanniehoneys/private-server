﻿using PointBlank.Core.Models.Enums;
using PointBlank.Core.Models.Room;
using PointBlank.Core.Network;
using PointBlank.Game.Data.Model;

namespace PointBlank.Game.Network.ServerPacket
{
    public class PROTOCOL_BATTLE_DEATH_ACK : SendPacket
    {
        private Room room;
        private FragInfos kills;
        private Slot killer;
        private bool isBotMode;

        public PROTOCOL_BATTLE_DEATH_ACK(Room r, FragInfos kills, Slot killer, bool isBotMode)
        {
            room = r;
            this.kills = kills;
            this.killer = killer;
            this.isBotMode = isBotMode;
        }

        public override void write()
        {
            writeH(4112);
            writeC((byte)kills.killingType);
            writeC(kills.killsCount);
            writeC(kills.killerIdx);
            writeD(kills.weapon);
            writeT(kills.x);
            writeT(kills.y);
            writeT(kills.z);
            writeC(kills.flag);
            for (int i = 0; i < kills.frags.Count; i++)
            {
                Frag frag = kills.frags[i];
                writeC(frag.victimWeaponClass);
                writeC(frag.hitspotInfo);
                writeH((short)frag.killFlag);
                writeC(frag.flag);
                writeT(frag.x);
                writeT(frag.y);
                writeT(frag.z);
                writeC((byte)frag.AssistSlot);
            }
            writeH((short)room._redKills);
            writeH((short)room._redDeaths);
            writeH((short)room._redAssists);
            writeH((short)room._blueKills);
            writeH((short)room._blueDeaths);
            writeH((short)room._blueAssists);
            for (int i = 0; i < 16; i++)
            {
                Slot slot = room._slots[i];
                writeH((short)slot.allKills);
                writeH((short)slot.allDeaths);
                writeH((short)slot.allAssists);
            }
            if (killer.killsOnLife == 2)
            {
                writeC(1);
            }
            else if (killer.killsOnLife == 3)
            {
                writeC(2);
            }
            else if (killer.killsOnLife > 3)
            {
                writeC(3);
            }
            else
            {
                writeC(0);
            }
            writeH((ushort)kills.Score);
            writeH((ushort)room.red_dino);
            writeH((ushort)room.blue_dino);
        }
    }
}