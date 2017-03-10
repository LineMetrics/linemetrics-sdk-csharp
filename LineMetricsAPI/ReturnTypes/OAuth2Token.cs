using System;
using System.Runtime.Serialization;

namespace LineMetrics.API.ReturnTypes
{
    [DataContract]
    public class OAuth2Token
    {
        [DataMember(Name = "access_token")]
        public string AccessToken { get; set; }

        [DataMember(Name = "token_type")]
        public string TokenType { get; set; }

        [DataMember(Name = "expires_in")]
        public int ExpiresIn { get; set; }

        public override string ToString()
        {
            return String.Format("AccessToken: {0}, TokenType: {1}, ExpiresIn: {2}s", AccessToken, TokenType, ExpiresIn);
        }
    }
}
