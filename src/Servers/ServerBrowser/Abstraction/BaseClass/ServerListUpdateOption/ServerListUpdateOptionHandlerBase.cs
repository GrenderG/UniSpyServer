﻿using ServerBrowser.Entity.Enumerate;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Extensions;
using ServerBrowser.Entity.Structure.Misc;
using System.Linq;
namespace ServerBrowser.Abstraction.BaseClass
{
    internal abstract class ServerListUpdateOptionHandlerBase : SBCmdHandlerBase
    {
        protected new ServerListUpdateOptionRequestBase _request => (ServerListUpdateOptionRequestBase)base._request;
        protected new ServerListUpdateOptionResultBase _result
        {
            get => (ServerListUpdateOptionResultBase)base._result;
            set => base._result = value;
        }
        protected new ServerListUpdateOptionResponseBase _response
        {
            get => (ServerListUpdateOptionResponseBase)base._response;
            set => base._response = value;
        }
        public ServerListUpdateOptionHandlerBase(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }
        protected override void RequestCheck()
        {
            string secretKey = DataOperationExtensions
                .GetSecretKey(_request.GameName);
            //we first check and get secrete key from database
            if (secretKey == null)
            {
                _result.ErrorCode = SBErrorCode.UnSupportedGame;
                return;
            }
            _result.GameSecretKey = secretKey;
            //this is client public ip and default query port
            _result.ClientRemoteIP = _session.RemoteIPEndPoint.Address.GetAddressBytes();
            _session.GameSecretKey = secretKey;
            _session.ClientChallenge = _request.ClientChallenge;
        }
        protected override void Encrypt()
        {
            SBEncryption enc;
            if (_session.EncParams == null)
            {
                _session.EncParams = new SBEncryptionParameters();
                enc = new SBEncryption(
                _result.GameSecretKey,
                _request.ClientChallenge,
                _session.EncParams);
            }
            else
            {
                enc = new SBEncryption(_session.EncParams);
            }

            var cryptHeader = _response.SendingBuffer.Take(14);
            var cipherBody = enc.Encrypt(_response.SendingBuffer.Skip(14).ToArray());
            _sendingBuffer = cryptHeader.Concat(cipherBody).ToArray();
        }
    }
}