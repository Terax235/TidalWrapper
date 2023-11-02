using Newtonsoft.Json;

namespace TidalWrapper.Responses
{
    internal class DeviceAuthorization
    {
        [JsonProperty("deviceCode")]
        public required string DeviceCode { get; set; }
        [JsonProperty("userCode")]
        public required string UserCode { get; set; }
        [JsonProperty("verificationUri")]
        public required string VerificationUri { get; set; }
        [JsonProperty("verificationUriComplete")]
        public required string VerificationUriComplete { get; set; }
        [JsonProperty("expiresIn")]
        public required int ExpiresIn { get; set; }
        [JsonProperty("interval")]
        public required int Interval { get; set; }
    }
}
