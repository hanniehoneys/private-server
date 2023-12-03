﻿using PointBlank.Core.Network;
using PointBlank.Core.Sql;
using Npgsql;
using System;
using System.Data;

namespace PointBlank.Core.Managers.Server
{
    public static class ServerConfigSyncer
    {
        public static ServerConfig GenerateConfig(int configId)
        {
            ServerConfig cfg = null;
            if (configId == 0)
            {
                return cfg;
            }
            try
            {
                using (NpgsqlConnection connection = SqlConnection.getInstance().conn())
                {
                    NpgsqlCommand command = connection.CreateCommand();
                    connection.Open();
                    command.Parameters.AddWithValue("@cfg", configId);
                    command.CommandText = "SELECT * FROM info_login_configs WHERE config_id=@cfg";
                    command.CommandType = CommandType.Text;
                    NpgsqlDataReader data = command.ExecuteReader();
                    while (data.Read())
                    {
                        cfg = new ServerConfig
                        {
                            configId = configId,
                            onlyGM = data.GetBoolean(1),
                            missions = data.GetBoolean(2),
                            UserFileList = data.GetString(3),
                            ClientVersion = data.GetString(4),
                            GiftSystem = data.GetBoolean(5),
                            ExitURL = data.GetString(6),
                            ChatColor = data.GetInt32(7),
                            AnnouceColor = data.GetInt32(8),
                            Chat = data.GetString(9),
                            Annouce = data.GetString(10),
                        };
                    }
                    command.Dispose();
                    data.Close();
                    connection.Dispose();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Logger.error(ex.ToString());
            }
            return cfg;
        }

        public static bool updateMission(ServerConfig cfg, bool mission)
        {
            cfg.missions = mission;
            return ComDiv.updateDB("info_login_configs", "missions", mission, "config_id", cfg.configId);
        }
    }

    public class ServerConfig
    {
        public int configId;
        public string UserFileList, ClientVersion, ExitURL, URL1, URL2;
        public bool onlyGM, missions, GiftSystem;
        public string Annouce, Chat;
        public int AnnouceColor, ChatColor;
    }
}