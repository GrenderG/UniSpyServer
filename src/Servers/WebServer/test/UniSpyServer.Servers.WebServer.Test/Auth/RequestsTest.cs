using Xunit;
using UniSpyServer.Servers.WebServer.Entity.Structure.Request.AuthRequest;

namespace UniSpyServer.Servers.WebServer.Test.Auth
{
    public class RequestsTest
    {
        //
        // These are the SOAP requests of AUTH
        // Endpoint: {FQDN}/AuthService/
        //
        [Fact]
        public void LoginProfile()
        {
            var request = new LoginProfileRequest(RawRequests.LoginProfile);
            request.Parse();
            Assert.Equal("1", request.Version.ToString());
            Assert.Equal("0", request.GameId.ToString());
            Assert.Equal("0", request.PartnerCode.ToString());
            Assert.Equal("0", request.NamespaceId.ToString());
            Assert.Equal("spyguy@unispy.org", request.Email);
            Assert.Equal("spyguy", request.Uniquenick);
            Assert.Equal("XXXXXXXXXXX", request.CDKey);
            Assert.Equal("XXXXXXXXXXX", request.Password[0].FieldName);
            Assert.Equal("Value", request.Password[0].FiledType);
        }
        [Fact]
        public void LoginPs3Cert()
        {
            var request = new LoginPs3CertRequest(RawRequests.LoginPs3Cert);
            request.Parse();
            Assert.Equal("0", request.GameId.ToString());
            Assert.Equal("0", request.PartnerCode.ToString());
            Assert.Equal("0", request.PS3cert.ToString());
        }
        [Fact]
        public void LoginRemoteAuth()
        {
            var request = new LoginRemoteAuthRequest(RawRequests.LoginRemoteAuth);
            request.Parse();
            Assert.Equal("1", request.Version.ToString());
            Assert.Equal("0", request.GameId.ToString());
            Assert.Equal("0", request.PartnerCode.ToString());
            Assert.Equal("0", request.NamespaceId.ToString());
            Assert.Equal("XXXXXXXXXXX", request.AuthToken);
            Assert.Equal("XXXXXXXXXXX", request.Challenge);
        }
        [Fact]
        public void LoginUniqueNick()
        {
            var request = new LoginUniqueNickRequest(RawRequests.LoginUniqueNick);
            request.Parse();
            Assert.Equal("1", request.Version.ToString());
            Assert.Equal("0", request.PartnerCode.ToString());
            Assert.Equal("0", request.NamespaceId.ToString());
            Assert.Equal("spyguy", request.Uniquenick);
            Assert.Equal("XXXXXXXXXXX", request.Password[0].FieldName);
            Assert.Equal("Value", request.Password[0].FiledType);
        }
    }
}