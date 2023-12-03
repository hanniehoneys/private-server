﻿using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Prng;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using PointBlank.Core;
using PointBlank.Core.Models.Servers;
using PointBlank.Core.Network;
using PointBlank.Core.Xml;
using PointBlank.Game.Data.Configs;
using PointBlank.Game.Data.Model;
using PointBlank.Game.Data.Xml;
using System;
using System.Collections.Generic;
using System.Net;

namespace PointBlank.Game.Network.ServerPacket
{
    public class PROTOCOL_BASE_CONNECT_ACK : SendPacket
    {
        private IPAddress Ip;
        private uint SessionId;
        private ushort SessionSeed;

        public PROTOCOL_BASE_CONNECT_ACK(GameClient Client)
        {
            SessionId = Client.SessionId;
            SessionSeed = Client.SessionSeed;
            Ip = Client.GetAddress();
        }

        public override void write()
        {
            SubjectPublicKeyInfo PublicKeyInfo = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(GeneratePair().Public);
            byte[] SerializedPublicBytes = PublicKeyInfo.ToAsn1Object().GetDerEncoded();
            byte[] Key = new byte[128];
            Buffer.BlockCopy(SerializedPublicBytes, 29, Key, 0, Key.Length);
            byte[] Exponent = new byte[] { 0x01, 0x00, 0x11 };
            short Size = 2;
            int Length = (Key.Length + Exponent.Length) + Size;

            CheckIp(Ip);
            writeH(514);
            writeH(2);
            writeC((byte)ChannelsXml.getChannels(GameConfig.serverId).Count);
            for (int i = 0; i < ChannelsXml.getChannels(GameConfig.serverId).Count; i++)
            {
                Channel channel = ChannelsXml._channels[i];
                writeC((byte)channel._type);
            }
            writeH((short)Length);
            writeH((short)Key.Length);
            writeB(Key);
            writeB(Exponent);
            writeC(3);
            writeH(24);
            writeH(SessionSeed);
            writeD(SessionId);
        }

        public void CheckIp(IPAddress Ip)
        {
            Logger.LogProblems(Ip.ToString(), "Ip/Game");
        }

        public static AsymmetricCipherKeyPair GeneratePair()
        {
            CryptoApiRandomGenerator RandomGenerator = new CryptoApiRandomGenerator();
            SecureRandom Secure = new SecureRandom(RandomGenerator);
            RsaKeyPairGenerator RSA = new RsaKeyPairGenerator();
            RSA.Init(new KeyGenerationParameters(Secure, 1024));
            AsymmetricCipherKeyPair Pair = RSA.GenerateKeyPair();
            return Pair;
        }
    }
}