﻿using PointBlank.Core.Managers.Events;
using PointBlank.Core.Models.Account.Players;
using PointBlank.Core.Models.Enums;
using PointBlank.Core.Network;

namespace PointBlank.Game.Network.ServerPacket
{
    public class PROTOCOL_BASE_ATTENDANCE_ACK : SendPacket
    {
        private EventVisitModel _event;
        private PlayerEvent _pev;
        private uint _erro;

        public PROTOCOL_BASE_ATTENDANCE_ACK(EventErrorEnum erro, EventVisitModel ev, PlayerEvent pev)
        {
            _erro = (uint)erro;
            _event = ev;
            _pev = pev;
        }

        public override void write()
        {
            writeH(545);
            writeD(_erro);
            if (_erro == 0x80001504)
            {
                writeD(_event.id);
                writeC((byte)_pev.LastVisitSequence1);
                writeC((byte)_pev.LastVisitSequence2);
                writeH(1);
                writeC(0); // Day
            }
        }
    }
}