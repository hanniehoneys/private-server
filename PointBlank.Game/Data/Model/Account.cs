﻿using PointBlank.Core.Managers;
using PointBlank.Core.Models.Account;
using PointBlank.Core.Models.Account.Players;
using PointBlank.Core.Models.Account.Title;
using PointBlank.Core.Models.Enums;
using PointBlank.Core.Network;
using PointBlank.Game.Data.Xml;
using PointBlank.Game.Data.Managers;
using PointBlank.Game.Data.Sync;
using System;
using System.Collections.Generic;
using System.Net;
using PointBlank.Game.Data.Configs;

namespace PointBlank.Game.Data.Model
{
    public class Account
    {
        public bool _isOnline, HideGMcolor, AntiKickGM, LoadedShop, DebugPing, ICafe = false;
        public string player_name = "", password, login,  FindPlayer = "";
        public long player_id, ban_obj_id;
        public uint LastRankUpDate, LastLoginDate;
        public IPAddress PublicIP;
        public CouponEffects effects;
        public PlayerSession Session;
        public int Sight, FindClanId, LastClanPage, LastRoomPage, LastPlayerPage, tourneyLevel, channelId = -1, clanAccess, clanDate, _exp, _gp, clanId, _money, brooch, insignia, medal, blue_order, _slotId = -1, name_color, _rank, pc_cafe, matchSlot = -1, age;
        public PlayerEquipedItems _equip = new PlayerEquipedItems();
        public PlayerInventory _inventory = new PlayerInventory();
        public List<PlayerItemTopup> _topups = new List<PlayerItemTopup>();
        public List<Character> Characters = new List<Character>();
        public PlayerConfig _config;
        public GameClient _connection;
        public Room _room;
        public PlayerBonus _bonus;
        public Match _match;
        public AccessLevel access;
        public PlayerDailyRecord Daily = new PlayerDailyRecord();
        public PlayerMissions _mission = new PlayerMissions();
        public PlayerStats _statistic = new PlayerStats();
        public FriendSystem FriendSystem = new FriendSystem();
        public PlayerTitles _titles = new PlayerTitles();
        public AccountStatus _status = new AccountStatus();
        public PlayerEvent _event;
        public DateTime LastLobbyEnter, LastPingDebug;
        
        public Account()
        {
            LastLobbyEnter = DateTime.Now;
        }

        public void SimpleClear()
        {
            _titles = new PlayerTitles();
            _mission = new PlayerMissions();
            _inventory = new PlayerInventory();
            _status = new AccountStatus();
            Characters = new List<Character>();
            Daily = new PlayerDailyRecord();
            _topups.Clear();
            FriendSystem.CleanList();
            Session = null;
            _event = null;
            _match = null;
            _room = null;
            _config = null;
            _connection = null;
        }

        public void SetPublicIP(IPAddress address)
        {
            if (address == null)
            {
                PublicIP = new IPAddress(new byte[4]);
            }
            PublicIP = address;
        }

        public void SetPublicIP(string address)
        {
            PublicIP = IPAddress.Parse(address);
        }

        public Channel getChannel()
        {
            return ChannelsXml.getChannel(channelId);
        }

        public void ResetPages()
        {
            LastRoomPage = 0;
            LastPlayerPage = 0;
        }

        public bool getChannel(out Channel channel)
        {
            channel = ChannelsXml.getChannel(channelId);
            return channel != null;
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
            lock (AccountManager._accounts)
            {
                AccountManager._accounts[player_id] = this;
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

        public Character getCharacterSlot(int Slot)
        {
            for (int i = 0; i < Characters.Count; i++)
            {
                Character Character = Characters[i];
                if (Character.Slot == Slot)
                {
                    return Character;
                }
            }
            return null;
        }

        public int getRank()
        {
            return _bonus == null || _bonus.fakeRank == 55 ? _rank : _bonus.fakeRank;
        }

        public void Close(int time, bool kicked = false)
        {
            if (_connection != null)
            {
                _connection.Close(time, kicked);
            }
        }

        public void SendPacket(SendPacket sp)
        {
            if (_connection != null)
            {
                _connection.SendPacket(sp);
            }
        }

        public void SendPacket(SendPacket sp, bool OnlyInServer)
        {
            if (_connection != null)
            {
                _connection.SendPacket(sp);
            }
            else if (!OnlyInServer && (_status.serverId != 255 && _status.serverId != GameConfig.serverId))
            {
                GameSync.SendBytes(player_id, sp, _status.serverId);
            }
        }

        public void SendPacket(byte[] data)
        {
            if (_connection != null)
            {
                _connection.SendPacket(data);
            }
        }

        public void SendPacket(byte[] data, bool OnlyInServer)
        {
            if (_connection != null)
            {
                _connection.SendPacket(data);
            }
            else if (!OnlyInServer && (_status.serverId != 255 && _status.serverId != GameConfig.serverId))
            {
                GameSync.SendBytes(player_id, data, _status.serverId);
            }
        }

        public void SendCompletePacket(byte[] data)
        {
            if (_connection != null)
            {
                _connection.SendCompletePacket(data);
            }
        }

        public void SendCompletePacket(byte[] data, bool OnlyInServer)
        {
            if (_connection != null)
            {
                _connection.SendCompletePacket(data);
            }
            else if (!OnlyInServer && (_status.serverId != 255 && _status.serverId != GameConfig.serverId))
            {
                GameSync.SendCompleteBytes(player_id, data, _status.serverId);
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

        public void LoadPlayerBonus()
        {
            PlayerBonus bonus = PlayerManager.getPlayerBonusDB(player_id);
            if (bonus.ownerId == 0)
            {
                PlayerManager.CreatePlayerBonusDB(player_id);
                bonus.ownerId = player_id;
            }
            _bonus = bonus;
        }

        public uint getSessionId()
        {
            return Session != null ? Session._sessionId : 0;
        }

        public void SetPlayerId(long id)
        {
            player_id = id;
            GetAccountInfos(35);
        }

        public void SetPlayerId(long id, int LoadType)
        {
            player_id = id;
            GetAccountInfos(LoadType);
        }

        public void GetAccountInfos(int LoadType)
        {
            if (LoadType > 0 && player_id > 0)
            {
                if ((LoadType & 1) == 1)
                {
                    Characters = CharacterManager.getCharacters(player_id);
                    _titles = TitleManager.getInstance().getTitleDB(player_id);
                    _topups = PlayerManager.getPlayerTopups(player_id);
                    ICafe = ICafeManager.GetCafe(_connection.GetIPAddress());
                    Daily = PlayerManager.getPlayerDailyRecord(player_id);
                    if (ICafe)
                    {
                        pc_cafe = 1;
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
                        AccountManager.getFriendlyAccounts(FriendSystem);
                    }
                }
                if ((LoadType & 8) == 8)
                {
                    _event = PlayerManager.getPlayerEventDB(player_id);
                }
                if ((LoadType & 16) == 16)
                {
                    _config = PlayerManager.getConfigDB(player_id);
                }
                if ((LoadType & 32) == 32)
                {
                    List<Friend> fs = PlayerManager.getFriendList(player_id);
                    if (fs.Count > 0)
                    {
                        FriendSystem._friends = fs;
                    }
                }
            }
        }

        public bool UseChatGM()
        {
            return !HideGMcolor && (_rank == 53 || _rank == 54);
        }

        public bool IsGM()
        {
            return _rank == 53 || _rank == 54 || HaveGMLevel();
        }

        public bool HaveGMLevel()
        {
            return (int)access > 2;
        }

        public bool HaveAcessLevel()
        {
            return (int)access > 0;
        }
    }
}