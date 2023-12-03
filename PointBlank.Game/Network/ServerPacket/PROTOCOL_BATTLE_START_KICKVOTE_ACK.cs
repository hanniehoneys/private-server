﻿using PointBlank.Core.Models.Room;
using PointBlank.Core.Network;

namespace PointBlank.Game.Network.ServerPacket
{
    public class PROTOCOL_BATTLE_START_KICKVOTE_ACK : SendPacket
    {
        private VoteKick vote;

        public PROTOCOL_BATTLE_START_KICKVOTE_ACK(VoteKick vote)
        {
            this.vote = vote;
        }

        public override void write()
        {
            writeH(3399);
            writeC((byte)vote.creatorIdx);
            writeC((byte)vote.victimIdx);
            writeC((byte)vote.motive);
        }
    }
}