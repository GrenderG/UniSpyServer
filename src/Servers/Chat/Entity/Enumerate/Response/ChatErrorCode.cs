﻿namespace Chat.Entity.Structure
{
    public enum ChatErrorCode
    {
        #region Chat system error
        Parse,
        DataOperation,
        ConstructResponse,
        NotChannelOperator,
        UserAlreadyInChannel,
        NoError,
        #endregion

        #region IRC erros
        TooManyChannels = 405,
        ErrOneUSNickName = 432,
        NickNameInUse = 433,
        MoreParameters = 461,
        ChannelIsFull = 471,
        InviteOnlyChan = 473,
        BannedFromChan = 474,
        BadChannelKey = 475,
        BadChanMask = 476,
        LoginFailed = 708,
        NoUniqueNick = 709,
        UniqueNIickExpired = 710,
        RegisterNickFailed = 711,
        #endregion
    }
}