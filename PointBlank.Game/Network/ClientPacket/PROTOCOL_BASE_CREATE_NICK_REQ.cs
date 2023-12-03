﻿using PointBlank.Core;
using PointBlank.Core.Filters;
using PointBlank.Core.Managers;
using PointBlank.Core.Models.Account.Players;
using PointBlank.Core.Network;
using PointBlank.Core.Xml;
using PointBlank.Game.Data.Configs;
using PointBlank.Game.Data.Managers;
using PointBlank.Game.Data.Model;
using PointBlank.Game.Network.ServerPacket;
using System;
using System.Collections.Generic;
using System.Text;

namespace PointBlank.Game.Network.ClientPacket
{
    public class PROTOCOL_BASE_CREATE_NICK_REQ : ReceivePacket
    {
        private string name;

        public PROTOCOL_BASE_CREATE_NICK_REQ(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            name = readUnicode(readC() * 2);
        }

        public override void run()
        {
            try
            {
                Account p = _client._player;
                if (p == null || p.player_name.Length > 0 || string.IsNullOrEmpty(name) || name.Length < GameConfig.minNickSize || name.Length > GameConfig.maxNickSize)
                {
                    _client.SendPacket(new  PROTOCOL_BASE_CREATE_NICK_ACK(0x80001013, ""));
                    return;
                }
                foreach (string s in NickFilter._filter)
                {
                    if (name.Contains(s))
                    {
                        _client.SendPacket(new PROTOCOL_BASE_CREATE_NICK_ACK(0x80001013, ""));
                        return;
                    }
                }
                if (!PlayerManager.isPlayerNameExist(name))
                {
                    if (AccountManager.updatePlayerName(name, p.player_id))
                    {
                        NickHistoryManager.CreateHistory(p.player_id, p.player_name, name, "สร้างตัวละคร");
                        p.player_name = name;
                        List<ItemsModel> awards = BasicInventoryXml.creationAwards;
                        List<ItemsModel> characters = BasicInventoryXml.Characters;
                        if (awards.Count > 0)
                        {
                            foreach (ItemsModel Item in awards)
                            {
                                if (Item._id != 0)
                                {
                                    _client.SendPacket(new PROTOCOL_INVENTORY_GET_INFO_ACK(0, p, Item));
                                }
                            }
                        }
                        //_client.SendPacket(new PROTOCOL_SHOP_PLUS_POINT_ACK(p._gp, p._gp, 4));
                        _client.SendPacket(new PROTOCOL_BASE_QUEST_GET_INFO_ACK(p));
                        _client.SendPacket(new PROTOCOL_BASE_CREATE_NICK_ACK(0, name));
                        if (characters.Count > 0)
                        {
                            foreach (ItemsModel Item in characters)
                            {
                                if (Item._id != 0)
                                {
                                    int Slots = p.Characters.Count;
                                    Character Character = new Character();
                                    Character.Id = Item._id;
                                    Character.Name = Item._name;
                                    Character.PlayTime = 0;
                                    Character.Slot = Slots++;
                                    Character.CreateDate = (uint)int.Parse(DateTime.Now.ToString("yyMMddHHmm"));
                                    p.Characters.Add(Character);
                                    _client.SendPacket(new PROTOCOL_INVENTORY_GET_INFO_ACK(0, p, Item));
                                    bool Result = CharacterManager.Create(Character, p.player_id);
                                    if (Result)
                                    {
                                        Console.WriteLine("Create New Chara Succes!! - " + p.player_id);
                                        _client.SendPacket(new PROTOCOL_CHAR_CREATE_CHARA_ACK(0, 3, Character, p));
                                    }
                                    else
                                    {
                                        Console.WriteLine("Create New Chara Failed!!");
                                        _client.SendPacket(new PROTOCOL_CHAR_CREATE_CHARA_ACK(0x80000000, 0, null, null));
                                    }
                                }
                            }
                        }
                        _client.SendPacket(new PROTOCOL_AUTH_FRIEND_INFO_ACK(p.FriendSystem._friends));
                    }
                    else
                    {
                        _client.SendPacket(new PROTOCOL_BASE_CREATE_NICK_ACK(0x80001013, ""));
                    }
                }
                else
                {
                    _client.SendPacket(new PROTOCOL_BASE_CREATE_NICK_ACK(0x80000113, ""));
                }
            }
            catch (Exception ex)
            {
                Logger.warning("PROTOCOL_LOBBY_CREATE_NICK_NAME_REQ: " + ex.ToString());
            }
        }
    }
}