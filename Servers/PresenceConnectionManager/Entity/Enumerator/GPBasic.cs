﻿namespace PresenceConnectionManager.Enumerator
{
    public enum GPBasic : uint
    {
        // Callbacks
        ////////////
        Error = 0,
        RecvBuddyRequest,
        RecvBuddyStatus,
        RecvBuddyMessage,
        RecvBuddyUTM,
        RecvGameInvite,
        TransferCallback,
        RecvBuddyAuth,
        RecvBuddyRevoke,

        // Global States.
        /////////////////
        InfoCaching = 0x0100,
        Simulation,
        InfoChachingBuddyAndBlockOnly,

        // Blocking
        ///////////
        Blocking = 1,
        NonBlocking = 0,

        // Firewall
        ///////////
        Firewall = 1,
        NoFirewall = 0,

        // Check Cache
        //////////////
        CheckCache = 1,
        DontCheckCache = 0,

        // Is Valid Email.
        // PANTS|02.15.00
        //////////////////
        EmailValid = 1,
        EmailInvalid = 0,

        // Fatal Error.
        ///////////////
        Fatal = 1,
        NonFatal = 0,

        // Sex
        //////
        Male = 0x0500,
        Fefamle,
        PAT,

        // Profile Search.
        //////////////////
        More = 0x0600,
        Done,

        // Set Info
        ///////////
        Nick = 0x0700,
        Uniquenick,
        Email,
        Password,
        FirstName,
        LastName,
        ICQUIN,
        HomePage,
        ZIPCode,
        CountryCode,
        Birthday,
        Sex,
        CPUBrand,
        CPUSpeed,
        Memory,
        VideoCard1String,
        VideoCard1RAM,
        VideoCard2String,
        VideoCard2RAM,
        ConnectionID,
        ConnectionSpeed,
        HasNetwork,
        OSString,
        AIMName,  // PANTS|03.20.01
        PIC,
        OccupationID,
        IndustryID,
        InComeID,
        MarriedID,
        ChildCount,
        Interest1,

        // New Profile.
        ///////////////
        Replace = 1,
        DontReplace = 0,

        // Is Connected.
        ////////////////
        Connected = 1,
        NotConnected = 0,

        // Public mask.
        ///////////////
        MaskNone = 0x00000000,
        MaskHomepage = 0x00000001,
        MaskZIPCode = 0x00000002,
        MaskContryCode = 0x00000004,
        MaskBirthday = 0x00000008,
        MaskSex = 0x00000010,
        MaskEmail = 0x00000020,
        MaskAll = 0xFFFFFFFF,

        // Session flags
        /////////////////
        SessIsClosed = 0x00000001,
        SessIsOpen = 0x00000002,
        SessHasPassword = 0x00000004,
        SessIsBehindNAT = 0x00000008,
        SessIsRanked = 0x000000010,

        // CPU Brand ID
        ///////////////
        Intel = 1,
        AMD,
        CYRIX,
        Motorola,
        Alpha,

        // Connection ID.
        /////////////////
        Modem = 1,
        ISDN,
        CableModem,
        DSL,
        Satellite,
        Ethernet,
        Wireless,

        // Transfer callback type.
        // *** the transfer is ended when these types are received
        //////////////////////////
        TransferSendRequest = 0x800,  // arg->num == numFiles
        TransferAccepted,
        TransferRejected,        // ***
        TransferNotAccepting,   // ***
        TransferNoConnection,   // ***
        TransferDone,            // ***
        TransferCancelled,       // ***
        TransferLostConnection, // ***
        TransferError,           // ***
        TransferThrottle,  // arg->num == Bps
        FileBegin,
        FileProgress,  // arg->num == numBytes
        FileEnd,
        FileDirectory,
        FileSkip,
        FileFaild,  // arg->num == error

        //FILE_FAILED error
        ///////////////////////
        FileReadError = 0x900,
        FileWriteError,
        FileDataError,

        // Transfer Side.
        /////////////////
        TransferSender = 0xA00,
        TansferReciever,

        // UTM send options.
        ////////////////////
        DontRout = 0xB00, // only send direct

        // Quiet mode flags.
        ////////////////////
        SlienceNone = 0x00000000,
        SlienceMessage = 0x00000001,
        SlienceUTMS = 0x00000002,
        SlienceList = 0x00000004, // includes requests, auths, and revokes
        SlienceAll = 0xFFFFFFFF,

        NewStatusInfoSupported = 0xC00,
        NewStatusInfoNotSupported = 0xC01,

        //BM status
        BmMessage = 1,
        BmRquest = 2,
        BmReply = 3, // only used on the backend
        BmAuth = 4,
        BmUTM = 5,
        BmRevoke = 6,  // remote buddy removed from local list
        BmStatus = 100,
        BmInvite = 101,
        BmPing = 102,
        BmPong = 103,
        BmKeysRequest = 104,
        BmKeysReply = 105,
        BmFileSendRequest = 200,
        BmFileSendReply = 201,
        BmFileBegin = 202,
        BmFileEnd = 203,
        BmFileData = 204,
        BmFile_SKIP = 205,
        BmFileTransferThrottle = 206,
        BmFileTransferCancel = 207,
        BmFileTransferKeepAlive = 208,
    }

    public enum GPSPResult : uint
    {
        NoError,
        MemoryError,
        ParameterError,
        NetworkError,
        ServerError,
        MISCError,
        Count
    }

    public enum GPStatus : uint
    {
        // Status
        /////////
        Offline = 0,
        Online = 1,
        Playing = 2,
        Staging = 3,
        Chatting = 4,
        Away = 5,
    }

    /// <summary>
    /// This enum rapresers the known Parter IDs, This value was setted to 0 when
    /// a game is directly connecting to GameSpy, otherwise the Partner ID would be
    /// different for any service that uses GameSpy as backend (for example Nintendo
    /// Wifi Connection).
    /// </summary>
    public enum PartnerID : uint
    {
        /// <summary>
        /// The client is directly connecting to the Server
        /// </summary>
        Gamespy = 0,

        // Unknown Partner ID from 1 to 9
        // EA partner ID should range here, it was

        /// <summary>
        /// Unknown usage for this partner id, but it exists in the
        /// GameSpy SDK
        /// </summary>
        IGN = 10,

        //Nintendo = 11, // Please verify this
    }

    /// <summary>
    /// This enum rapresents the gender of a player.
    /// </summary>
    public enum PlayerSexType : ushort
    {
        /// <summary>
        /// Gender is male
        /// </summary>
        Male,

        /// <summary>
        /// Gender is female
        /// </summary>
        Female,

        /// <summary>
        /// Unspecified or unknown gender, this is
        /// used to mask the gender when the information is queried
        /// </summary>
        Pat
    }
}
