﻿namespace UniSpyServer.Servers.Chat.Entity.Structure
{
    public enum ChatErrorCode
    {
        Parse,
        DataOperation,
        ConstructResponse,
        NotChannelOperator,
        UserAlreadyInChannel,
        UnSupportedGame,
        NoError,
        IRCError,
    }
}