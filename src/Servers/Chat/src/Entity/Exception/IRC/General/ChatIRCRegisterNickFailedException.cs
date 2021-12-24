using UniSpyServer.Servers.Chat.Entity.Structure.Misc;

namespace UniSpyServer.Servers.Chat.Entity.Exception.IRC.General
{
    public sealed class ChatIRCRegisterNickFaildException : IRCException
    {
        public ChatIRCRegisterNickFaildException()
        {
        }

        public ChatIRCRegisterNickFaildException(string message) : base(message, IRCErrorCode.RegisterNickFailed)
        {
        }

        public ChatIRCRegisterNickFaildException(string message, System.Exception innerException) : base(message, IRCErrorCode.RegisterNickFailed, innerException)
        {
        }
    }
}