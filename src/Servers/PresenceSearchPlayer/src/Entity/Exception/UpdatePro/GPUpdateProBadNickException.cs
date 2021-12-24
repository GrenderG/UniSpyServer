using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Enumerate;

namespace UniSpyServer.Servers.PresenceSearchPlayer.Entity.Exception.UpdatePro
{
    public class GPUpdateProBadNickException : GPUpdateProException
    {
        public GPUpdateProBadNickException() : base("Nickname is invalid for updating profile!", GPErrorCode.UpdateProBadNick)
        {
        }

        public GPUpdateProBadNickException(string message) : base(message, GPErrorCode.UpdateProBadNick)
        {
        }

        public GPUpdateProBadNickException(string message, System.Exception innerException) : base(message, GPErrorCode.UpdateProBadNick, innerException)
        {
        }
    }
}