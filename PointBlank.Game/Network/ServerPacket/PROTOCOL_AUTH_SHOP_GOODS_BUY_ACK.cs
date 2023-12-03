﻿using PointBlank.Core;
using PointBlank.Core.Managers;
using PointBlank.Core.Models.Account.Players;
using PointBlank.Core.Models.Shop;
using PointBlank.Core.Network;
using PointBlank.Game.Data.Model;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace PointBlank.Game.Network.ServerPacket
{
    public class PROTOCOL_AUTH_SHOP_GOODS_BUY_ACK : SendPacket
    {
        private List<GoodItem> Items = new List<GoodItem>();
        private Account Player;
        private uint Error;

        public PROTOCOL_AUTH_SHOP_GOODS_BUY_ACK(uint Error, List<GoodItem> Item = null, Account Player = null)
        {
            this.Error = Error;
            if (this.Error == 1)
            {
                this.Player = Player;
                AddItem(Item);
            }
        }

        public override void write()
        {
            writeH(1044);
            writeH(0);
            if (Error == 1)
            {
                writeC((byte)Items.Count);
                for (int i = 0; i < Items.Count; i++)
                {
                    GoodItem Item = Items[i];
                    writeD(0);
                    writeD(Item.id);
                    writeC(0);
                }
                writeD(Player._money);
                writeD(Player._gp);
                writeD(uint.Parse(DateTime.Now.AddYears(-10).ToString("yyMMddHHmm")));
            }
            else
            {
                writeD(Error);
            }
        }

        private void AddItem(List<GoodItem> Goods)
        {
            try
            {
                for (int i = 0; i < Goods.Count; i++)
                {
                    GoodItem Model = Goods[i];
                    Items.Add(Model);
                }
            }
            catch (Exception ex)
            {
                Logger.warning("PROTOCOL_AUTH_SHOP_GOODS_BUY_ACK: " + ex.ToString());
            }
        }
    }
}