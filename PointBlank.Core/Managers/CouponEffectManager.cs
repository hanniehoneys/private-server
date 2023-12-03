﻿using PointBlank.Core.Models.Enums;
using PointBlank.Core.Sql;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;

namespace PointBlank.Core.Managers
{
    public static class CouponEffectManager
    {
        private static List<CouponFlag> Effects = new List<CouponFlag>();

        public static void LoadCouponFlags()
        {
            try
            {
                using (NpgsqlConnection connection = SqlConnection.getInstance().conn())
                {
                    NpgsqlCommand command = connection.CreateCommand();
                    connection.Open();
                    command.CommandText = "SELECT * FROM info_cupons_flags";
                    command.CommandType = CommandType.Text;
                    NpgsqlDataReader data = command.ExecuteReader();
                    while (data.Read())
                    {
                        CouponFlag cupom = new CouponFlag
                        {
                            ItemId = data.GetInt32(0),
                            EffectFlag = (CouponEffects)data.GetInt64(1)
                        };
                        Effects.Add(cupom);
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
        }

        public static CouponFlag getCouponEffect(int id)
        {
            for (int i = 0; i < Effects.Count; i++)
            {
                CouponFlag flag = Effects[i];
                if (flag.ItemId == id)
                {
                    return flag;
                }
            }
            return null;
        }
    }

    public class CouponFlag
    {
        public int ItemId;
        public CouponEffects EffectFlag;
    }
}