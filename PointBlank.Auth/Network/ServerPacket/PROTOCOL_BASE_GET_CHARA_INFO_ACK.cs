﻿using PointBlank.Auth.Data.Model;
using PointBlank.Core;
using PointBlank.Core.Models.Account.Players;
using PointBlank.Core.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointBlank.Auth.Network.ServerPacket
{
    public class PROTOCOL_BASE_GET_CHARA_INFO_ACK : SendPacket
    {
        private Account Player;

        public PROTOCOL_BASE_GET_CHARA_INFO_ACK(Account Player)
        {
            this.Player = Player;
        }

        public override void write()
        {
            try
            {
                writeH(660);
                writeH(0);
                writeC((byte)Player.Characters.Count);
                foreach (Character Character in Player.Characters)
                {
                    if (Character != null)
                    {
                        writeC((byte)Character.Slot);
                        if (Character.Slot == 0 || Character.Slot == 1)
                        {
                            writeC(20);
                        }
                        else
                        {
                            writeC(30);
                        }
                        writeD((int)Character.ObjId);
                        writeD(Character.CreateDate);
                        writeD((int)Character.PlayTime);
                        writeD(Character.PlayTime);
                        writeUnicode(Character.Name, 66);

                        if (Player._equip._primary == 0)
                            writeD(104006);
                        else
                            writeD(Player._equip._primary);
                        writeD((int)(Player._inventory.getItem(Player._equip._primary)?._objId ?? 0));

                        if (Player._equip._secondary == 0)
                            writeD(202003);
                        else
                            writeD(Player._equip._secondary);
                        writeD((int)(Player._inventory.getItem(Player._equip._secondary)?._objId ?? 0));
                        if (Player._equip._melee == 0)
                            writeD(301001);
                        else
                            writeD(Player._equip._melee);
                        writeD((int)(Player._inventory.getItem(Player._equip._melee)?._objId ?? 0));
                        if (Player._equip._grenade == 0)
                            writeD(407001);
                        else
                            writeD(Player._equip._grenade);
                        writeD((int)(Player._inventory.getItem(Player._equip._grenade)?._objId ?? 0));
                        if (Player._equip._special == 0)
                            writeD(508001);
                        else
                            writeD(Player._equip._special);
                        writeD((int)(Player._inventory.getItem(Player._equip._special)?._objId ?? 0));
                        writeD(Character.Id);
                        writeD((int)(Player._inventory.getItem(Character.Id)?._objId ?? 0));
                        writeD(Player._equip.face);
                        if (Player._inventory.getItem(Player._equip.face) == null)
                        {
                            writeD(0);
                        }
                        else
                        {
                            writeD((int)Player._inventory.getItem(Player._equip.face)._objId);
                        }
                        writeD(Player._equip._helmet);
                        if (Player._inventory.getItem(Player._equip._helmet) == null)
                        {
                            writeD(0);
                        }
                        else
                        {
                            writeD((int)Player._inventory.getItem(Player._equip._helmet)._objId);
                        }
                        writeD(Player._equip.jacket);
                        writeD((int)Player._inventory.getItem(Player._equip.jacket)._objId);
                        writeD(Player._equip.poket);
                        writeD((int)Player._inventory.getItem(Player._equip.poket)._objId);
                        writeD(Player._equip.glove);
                        writeD((int)Player._inventory.getItem(Player._equip.glove)._objId);
                        writeD(Player._equip.belt);
                        writeD((int)Player._inventory.getItem(Player._equip.belt)._objId);
                        writeD(Player._equip.holster);
                        writeD((int)Player._inventory.getItem(Player._equip.holster)._objId);
                        writeD(Player._equip.skin);
                        writeD((int)Player._inventory.getItem(Player._equip.skin)._objId);
                        writeD(Player._equip._beret);
                        if (Player._inventory.getItem(Player._equip._beret) == null)
                        {
                            writeD(0);
                        }
                        else
                        {
                            writeD((int)Player._inventory.getItem(Player._equip._beret)._objId);
                        }
                        writeC(0);
                        writeC(255);
                        writeC(255);
                        writeC(255);
                        writeC(0);
                        writeC(0);
                        writeC(0);
                    }
                }
                writeC(0);
            }
            catch (Exception ex)
            {
                Logger.error("PROTOCOL_BASE_GET_CHARA_INFO_ACK: " + ex.ToString());
            }
        }
    }
}