﻿namespace PointBlank.Core.Models.Enums
{
    public enum EventErrorEnum : uint
    {
        SUCCESS = 0,
        LOGIN_ALREADY_LOGIN_WEB = 0x80000101,
        LOGIN_USER_PASS_FAIL = 0x80000102,
        //07
        LOGIN_LOGOUTING = 0x80000104,
        LOGIN_TIME_OUT_1 = 0x80000105,
        LOGIN_TIME_OUT_2 = 0x80000106,
        LOGIN_BLOCK_ACCOUNT = 0x80000107,
        //13
        LOGIN_SERVER_USER_FULL = 0x8000010E,
        LOGIN_DB_BUFFER_FAIL = 0x80000114,
        LOGIN_ID_PASS_INCORRECT = 0x80000117, //ID ou senha incorretos. (Usuário)
        LOGIN_ID_PASS_INCORRECT2 = 0x80000118, //ID ou senha incorretos. (Senha)
        LOGIN_DELETE_ACCOUNT = 0x80000119,
        LOGIN_EMAIL_AUTH_ERROR = 0x80000120,
        LOGIN_BLOCK_IP = 0x80000121,
        LOGIN_EMAIL_ALERT2 = 0x80000122,
        //39
        LOGIN_MIGRATION = 0x80000124,
        LOGIN_NON_STRING = 0x80000125,
        LOGIN_BLOCK_COUNTRY = 0x80000126, //Login bloqueado devido à região.
        LOGIN_INVALID_ACCOUNT = 0x80000127, //Usuário ou senha incorreta.
        //41?
        //44 45 46 47
        LOGIN_PC_BLOCK = 0x80000133, //Este computador está bloqueado.
        LOGIN_INVENTORY_FAIL = 0x80001034,

        LOGIN_BLOCK_INNER = 0x80100000,
        LOGIN_BLOCK_OUTER = 0x80200000,
        LOGIN_BLOCK_GAME = 0x80800000,


        BATTLE_NO_REAL_IP = 0x80001008, //No RealIP
        BATTLE_NO_READY_TEAM = 0x80001009, //I was not a 2 team
        BATTLE_FIRST_MAINLOAD = 0x8000100A, //Loading (out of time)
        BATTLE_FIRST_HOLE = 0x8000100B, //Out by hole punching
        BATTLE_NO_ENEMY = 0x8000100C, //Praptor - Do not use
        BATTLE_WAIT_BATTLE_CLIMAX = 0x8000100F, //Praptor - Do not use
        BATTLE_NO_START_FOR_UNDER_NAT = 0x80001012, //Under the NAT that can not start the game
        BATTLE_PRESTART_BATTLE_ALREADYEND = 0x80001015, //The game is over when I am on board.
        BATTLE_START_BATTLE_ALREADYEND = 0x80001016, //The game is over when I am on board.


        VISIT_EVENT_USERFAIL = 0x80001500,
        VISIT_EVENT_NOTENOUGH = 0x80001501,
        VISIT_EVENT_ALREADYCHECK = 0x80001502,
        VISIT_EVENT_WRONGVERSION = 0x80001503,
        VISIT_EVENT_SUCCESS = 0x80001504,
        VISIT_EVENT_UNKNOWN = 0x80001505
    }
}