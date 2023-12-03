﻿using Npgsql;
using PointBlank.Core.Network;
using PointBlank.Game.Data.Configs;
using PointBlank.Game.Data.Model;
using PointBlank.Core.Sql;
using PointBlank.Game.Data.Xml;
using System;
using System.Collections.Generic;
using System.Data;

namespace PointBlank.Game.Network.ServerPacket
{
    public class PROTOCOL_BASE_GET_CHANNELLIST_ACK : SendPacket
    {
        private List<Channel> Channels;
        private int ServerId;

        public PROTOCOL_BASE_GET_CHANNELLIST_ACK(List<Channel> Channels, int ServerId)
        {
            this.Channels = Channels;
            this.ServerId = ServerId;
        }

        public override void write()
        {
            writeH(541);
            writeH(0);
            writeC(0);
            writeC((byte)Channels.Count);
            for (int i = 0; i < Channels.Count; i++)
            {
                Channel Channel = Channels[i];
                using (NpgsqlConnection connection = SqlConnection.getInstance().conn())
                {
                    NpgsqlCommand command = connection.CreateCommand();
                    connection.Open();
                    command.Parameters.AddWithValue("@channelid", i);
                    command.Parameters.AddWithValue("@serverid", ServerId);
                    command.CommandText = "SELECT online FROM info_channels WHERE channel_id=@channelid and server_id=@serverid";
                    command.CommandType = CommandType.Text;
                    NpgsqlDataReader data = command.ExecuteReader();
                    while (data.Read())
                    {
                        writeH((ushort)data.GetInt32(0));
                    }
                    command.Dispose();
                    data.Close();
                    connection.Dispose();
                    connection.Close();
                }
            }
            writeH(100);
            writeC((byte)Channels.Count);
        }
    }
}