using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TidalWrapper.Responses
{
    public class OAuthToken
    {
        [JsonProperty("access_token")]
        public required string AccessToken { get; set; }

        [JsonProperty("refresh_token")]
        public string? RefreshToken { get; set; }

        [JsonProperty("token_type")]
        public required string TokenType { get; set; }

        [JsonProperty("expires_in")]
        public required int ExpiresIn { get; set; }

        [JsonProperty("user")]
        public required OAuthUser User { get; set; }

        [JsonProperty("scope")]
        public required string Scope { get; set; }

        [JsonProperty("clientName")]
        public required string ClientName { get; set; }
    }

    public class OAuthUser
    {
        [JsonProperty("userId")]
        public required long UserId { get; set; }

        [JsonProperty("email")]
        public string? Email { get; set; }

        [JsonProperty("countryCode")]
        public required string CountryCode { get; set; }

        [JsonProperty("fullName")]
        public string? FullName { get; set; }

        [JsonProperty("firstName")]
        public string? FirstName { get; set; }

        [JsonProperty("lastName")]
        public string? LastName { get; set; }

        [JsonProperty("nickname")]
        public string? Nickname { get; set; }

        [JsonProperty("username")]
        public required string Username { get; set; }

        [JsonProperty("address")]
        public string? Address { get; set; }

        [JsonProperty("city")]
        public string? City { get; set; }


        [JsonProperty("postalcode")]
        public int? PostalCode { get; set; }


        [JsonProperty("usState")]
        public string? UsState { get; set; }


        [JsonProperty("phoneNumber")]
        public string? PhoneNumber { get; set; }


        [JsonProperty("birthday")]
        public string? Birthday { get; set; }


        [JsonProperty("channelId")]
        public required int ChannelId { get; set; }

        [JsonProperty("parentId")]
        public required int ParentId { get; set; }


        [JsonProperty("acceptedEULA")]
        public required bool AcceptedEULA { get; set; }

        [JsonProperty("created")]
        private long Created { get; set; }
        public DateTime CreatedAt
        {
            get { return DateTimeOffset.FromUnixTimeMilliseconds(Created).Date; }
        }

        [JsonProperty("updated")]
        private long Updated { get; set; }
        public DateTime UpdatedAt
        {
            get { return DateTimeOffset.FromUnixTimeMilliseconds(Updated).Date; }
        }

        [JsonProperty("facebookUid")]
        public required long FacebookUID { get; set; }

        [JsonProperty("appleUid")]
        public string? AppleUID { get; set; }

        [JsonProperty("googleUid")]
        public string? GoogleUID { get; set; }

        [JsonProperty("accountLinkCreated")]
        public required bool AccountLinkCreated { get; set; }

        [JsonProperty("emailVerified")]
        public required bool EmailVerified { get; set; }

        [JsonProperty("newUser")]
        public required bool NewUser { get; set; }
    }
}
