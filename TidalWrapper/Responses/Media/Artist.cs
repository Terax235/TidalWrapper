using Newtonsoft.Json;

namespace TidalWrapper.Responses
{
    public class ArtistBase
    {
        [JsonProperty("id")]
        public required int Id { get; set; }

        [JsonProperty("name")]
        public required string Name { get; set; }

        [JsonProperty("type")]
        public required string Type { get; set; }

        [JsonProperty("picture")]
        public string? PictureId { get; set; }
    }
}