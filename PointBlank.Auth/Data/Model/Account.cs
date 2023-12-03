﻿using PointBlank.Core.Managers;
using PointBlank.Core.Models.Account;
using PointBlank.Core.Models.Account.Players;
using PointBlank.Core.Models.Account.Title;
using PointBlank.Core.Models.Enums;
using PointBlank.Core.Network;
using PointBlank.Auth.Data.Configs;
using PointBlank.Auth.Data.Managers;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using PointBlank.Core;

namespace PointBlank.Auth.Data.Model
{
    public class Account
    {
        public bool _myConfigsLoaded, _isOnline, ICafe = false;
        public CouponEffects effects;
        public uint LastRankUpDate;
        public string player_name = "", password, login, token, hwid;
        public int tourneyLevel, _exp, _gp, clan_id, clanAccess, _money, pc_cafe, _rank, brooch, insignia, medal, blue_order, name_color, access, age;
        public long player_id, ban_obj_id;
        public PhysicalAddress MacAddress;
        public PlayerEquipedItems _equip = new PlayerEquipedItems();
        public PlayerInventory _inventory = new PlayerInventory();
        public AuthClient _connection;
        public PlayerBonus _bonus;
        public PlayerMissions _mission = new PlayerMissions();
        public PlayerStats _statistic = new PlayerStats();
        public PlayerConfig _config;
        public PlayerTitles _titles;
        public AccountStatus _status = new AccountStatus();
        public PlayerEvent _event;
        public FriendSystem FriendSystem = new FriendSystem();
        public PlayerDailyRecord Daily = new PlayerDailyRecord();
        public List<Account> _clanPlayers = new List<Account>();
        public List<Character> Characters = new List<Character>();
        public List<PlayerItemTopup> _topups = new List<PlayerItemTopup>();

        public void SimpleClear()
        {
            _config = null;
            _titles = null;
            _bonus = null;
            _event = null;
            _connection = null;
            _inventory = new PlayerInventory();
            Characters = new List<Character>();
            FriendSystem = new FriendSystem();
            _clanPlayers = new List<Account>();
            _equip = new PlayerEquipedItems();
            _mission = new PlayerMissions();
            _status = new AccountStatus();
            _topups = new List<PlayerItemTopup>();
            Daily = new PlayerDailyRecord();
        }

        public void setOnlineStatus(bool online)
        {
            if (_isOnline != online && ComDiv.updateDB("accounts", "online", online, "player_id", player_id))
            {
                _isOnline = online;
            }
        }

        public void updateCacheInfo()
        {
            if (player_id == 0)
            {
                return;
            }
            lock (AccountManager.getInstance()._accounts)
            {
                AccountManager.getInstance()._accounts[player_id] = this;
            }
        }

        public void Close(int time)
        {
            if (_connection != null)
            {
                _connection.Close(time, true);
            }
        }

        public void SendPacket(SendPacket sp)
        {
            if (_connection != null)
            {
                _connection.SendPacket(sp);
            }
        }

        public void SendPacket(byte[] data)
        {
            if (_connection != null)
            {
                _connection.SendPacket(data);
            }
        }

        public void SendCompletePacket(byte[] data)
        {
            if (_connection != null)
            {
                _connection.SendCompletePacket(data);
            }
        }

        public void LoadInventory()
        {
            lock (_inventory._items)
            {
                _inventory._items.AddRange(PlayerManager.getInventoryItems(player_id));
            }
        }

        public void LoadMissionList()
        {
            PlayerMissions m = MissionManager.getInstance().getMission(player_id, _mission.mission1, _mission.mission2, _mission.mission3, _mission.mission4);
            if (m == null)
            {
                MissionManager.getInstance().addMissionDB(player_id);
            }
            else
            {
                _mission = m;
            }
        }

        public void SetPlayerId(long id, int LoadType)
        {
            player_id = id;
            GetAccountInfos(LoadType);
        }

        public void SetPlayerId()
        {
            _titles = new PlayerTitles();
            _bonus = new PlayerBonus();
            GetAccountInfos(8);
        }

        public void GetAccountInfos(int LoadType)
        {
            if (LoadType == 0 || player_id == 0)
            {
                return;
            }
            if ((LoadType & 1) == 1)
            {
                _titles = TitleManager.getInstance().getTitleDB(player_id);
                _topups = PlayerManager.getPlayerTopups(player_id);
                Characters = CharacterManager.getCharacters(player_id);
                Daily = PlayerManager.getPlayerDailyRecord(player_id);
                if (Daily == null)
                {
                    PlayerManager.CreatePlayerDailyRecord(player_id);
                }
            }
            if ((LoadType & 2) == 2)
            {
                _bonus = PlayerManager.getPlayerBonusDB(player_id);
            }
            if ((LoadType & 4) == 4)
            {
                List<Friend> fs = PlayerManager.getFriendList(player_id);
                if (fs.Count > 0)
                {
                    FriendSystem._friends = fs;
                }
            }
            if ((LoadType & 8) == 8)
            {
                _event = PlayerManager.getPlayerEventDB(player_id);
                if (_event == null)
                {
                    PlayerManager.addEventDB(player_id);
                    _event = new PlayerEvent();
                }
            }
            if ((LoadType & 16) == 16)
            {
                _config = PlayerManager.getConfigDB(player_id);
                if (_config == null)
                {
                    PlayerManager.CreateConfigDB(player_id);
                }
                if (_connection != null)
                {
                    ICafe = ICafeManager.GetCafe(_connection.GetIPAddress());
                    if (ICafe)
                    {
                        pc_cafe = 1;
                    }
                }
            }
        }

        public Character getCharacter(int ItemId)
        {
            for (int i = 0; i < Characters.Count; i++)
            {
                Character Character = Characters[i];
                if (Character.Id == ItemId)
                {
                    return Character;
                }
            }
            return null;
        }

        public bool CompareToken(string Token)
        {
            return AuthConfig.isTestMode || Token == token;
        }

        public bool DiscountPlayerItems()
        {
            try
            {
                bool updEffect = false;
                long data_atual = Convert.ToInt64(DateTime.Now.AddYears(-10).ToString("yyMMddHHmm"));
                List<object> removedItems = new List<object>();
                int bonuses = _bonus != null ? _bonus.bonuses : 0, freepass = _bonus != null ? _bonus.freepass : 0;
                lock (_inventory._items)
                {
                    for (int i = 0; i < _inventory._items.Count; i++)
                    {
                        ItemsModel item = _inventory._items[i];
                        if (item._count <= data_atual & item._equip == 2)
                        {
                            if (item._category == 2 && ComDiv.getIdStatics(item._id, 1) == 6)
                            {
                                Character Character = getCharacter(item._id);
                                if (Character != null)
                                {
                                    int Index = 0;
                                    for (int Slot = 0; Slot < Characters.Count; Slot++)
                                    {
                                        Character Chara = Characters[Slot];
                                        if (Chara.Slot != Character.Slot)
                                        {
                                            Chara.Slot = Index;
                                            CharacterManager.Update(Index, Chara.ObjId);
                                            Index++;
                                        }
                                    }
                                    if (CharacterManager.Delete(Character.ObjId, player_id))
                                    {
                                        Characters.Remove(Character);
                                    }
                                }
                            }
                            else if (item._category == 3)
                            {
                                if (_bonus == null)
                                {
                                    continue;
                                }
                                bool changed = _bonus.RemoveBonuses(item._id);
                                if (!changed)
                                {
                                    if (item._id == 1600014)
                                    {
                                        ComDiv.updateDB("player_bonus", "sightcolor", 4, "player_id", player_id);
                                        _bonus.sightColor = 4;
                                    }
                                    else if (item._id == 1600006)
                                    {
                                        ComDiv.updateDB("accounts", "name_color", 0, "player_id", player_id);
                                        name_color = 0;
                                    }
                                    else if (item._id == 1600009)
                                    {
                                        ComDiv.updateDB("player_bonus", "fakerank", 55, "player_id", player_id);
                                        _bonus.fakeRank = 55;
                                    }
                                    else if (item._id == 1600010)
                                    {
                                        if (_bonus.fakeNick.Length > 0)
                                        {
                                            ComDiv.updateDB("player_bonus", "fakenick", "", "player_id", player_id);
                                            ComDiv.updateDB("accounts", "player_name", _bonus.fakeNick, "player_id", player_id);
                                            player_name = _bonus.fakeNick;
                                            _bonus.fakeNick = "";
                                        }
                                    }
                                    else if (item._id == 1600187)
                                    {
                                        ComDiv.updateDB("player_bonus", "muzzle", 0, "player_id", player_id);
                                        _bonus.muzzle = 0;
                                    }
                                }
                                CouponFlag cupom = CouponEffectManager.getCouponEffect(item._id);
                                if (cupom != null && cupom.EffectFlag > 0 && effects.HasFlag(cupom.EffectFlag))
                                {
                                    effects -= cupom.EffectFlag;
                                    updEffect = true;
                                }
                            }
                            removedItems.Add(item._objId);
                            _inventory._items.RemoveAt(i--);
                        }
                        else if (item._count == 0)
                        {
                            removedItems.Add(item._objId);
                            _inventory._items.RemoveAt(i--);
                        }
                    }
                    ComDiv.deleteDB("player_items", "object_id", removedItems.ToArray(), "owner_id", player_id);
                }
                removedItems = null;
                if (_bonus != null && (_bonus.bonuses != bonuses || _bonus.freepass != freepass))
                {
                    PlayerManager.updatePlayerBonus(player_id, _bonus.bonuses, _bonus.freepass);
                }
                if (effects < 0)
                {
                    effects = 0;
                }
                if (updEffect)
                {
                    PlayerManager.updateCupomEffects(player_id, effects);
                }

                if (pc_cafe >= 1)
                    _inventory.LoadCafeItems();

                _inventory.LoadBasicItems();
                int type = PlayerManager.CheckEquipedItems(_equip, _inventory._items);
                if (type > 0)
                {
                    DBQuery query = new DBQuery();
                    if ((type & 2) == 2)
                    {
                        PlayerManager.updateWeapons(_equip, query);
                    }
                    if ((type & 1) == 1)
                    {
                        PlayerManager.updateChars(_equip, query);
                    }
                    ComDiv.updateDB("accounts", "player_id", player_id, query.GetTables(), query.GetValues());
                    query = null;
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.error("DiscountPlayerItems: " + ex.ToString());
                return false;
            }
        }

        public bool IsGM()
        {
            return _rank == 53 || _rank == 54;
        }
        public bool Observer()
        {
            return _rank == 53 || _rank == 54;
        }

        public bool HaveGMLevel()
        {
            return access > 2;
        }

        public bool HaveAcessLevel()
        {
            return access > 0;
        }
    }
}