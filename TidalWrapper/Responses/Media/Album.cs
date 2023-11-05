using Newtonsoft.Json;

namespace TidalWrapper.Responses
{
    public class AlbumBase
    {
        [JsonProperty("id")]
        public required int Id { get; set; }

        [JsonProperty("title")]
        public required string Title { get; set; }

        [JsonProperty("cover")]
        public required string Cover { get; set; }

        [JsonProperty("vibrantColor")]
        public required string VibrantColor { get; set; }

        [JsonProperty("videoCover")]
        public string? VideoCover { get; set; }
    }

    public class Album : AlbumBase
    {

    }
}