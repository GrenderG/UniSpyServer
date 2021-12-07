using UniSpyServer.UniSpyLib.Abstraction.Contract;
namespace UniSpyServer.Servers.GameStatus.Entity.Contract
{
    public class HandlerContract : HandlerContractBase
    {
        public new string Name => (string)base.Name;

        public HandlerContract(string name) : base(name)
        {
        }

    }
}