using Newtonsoft.Json;
using System.Text;
using TidalWrapper.Engines;

namespace TidalWrapper.Responses
{
    public class TrackSearch
    {
        [JsonProperty("limit")]
        public required int Limit { get; set; }
        [JsonProperty("offset")]
        public required int Offset { get; set; }
        [JsonProperty("totalNumberOfItems")]
        public required int TotalItemCount { get; set; }
        [JsonProperty("items")]
        public required Track[] Items { get; set; }

        [JsonIgnore]
        public int ItemCount
        {
            get { return Items.Length; }
        }
    }
    public class Track
    {
        [JsonProperty("id")]
        public required int Id { get; set; }

        [JsonProperty("title")]
        public required string Title { get; set; }

        [JsonProperty("url")]
        public required string Url { get; set; }

        [JsonProperty("duration")]
        public required int Duration { get; set; }

        [JsonProperty("replayGain")]
        public required float ReplayGain { get; set; }

        [JsonProperty("peak")]
        public required float Peak { get; set; }

        [JsonProperty("allowStreaming")]
        public required bool AllowStreaming { get; set; }

        [JsonProperty("streamReady")]
        public required bool StreamReady { get; set; }

        [JsonProperty("adSupportedStreamReady")]
        public required bool AdSupportedStreamReady { get; set; }

        [JsonProperty("djReady")]
        public required bool DjReady { get; set; }

        [JsonProperty("stemReady")]
        public required bool SteamReady { get; set; }

        [JsonProperty("streamStartDate")]
        public required DateTime StreamStartDate { get; set; }

        [JsonProperty("premiumStreamingOnly")]
        public required bool PremiumStreamingOnly { get; set; }

        [JsonProperty("trackNumber")]
        public required int TrackNumber { get; set; }

        [JsonProperty("volumeNumber")]
        public required int VolumeNumber { get; set; }

        [JsonProperty("version")]
        public string? Version { get; set; }

        [JsonProperty("popularity")]
        public required int Popularity { get; set; }

        [JsonProperty("copyright")]
        public required string Copyright { get; set; }

        [JsonProperty("isrc")]
        public required string ISRC { get; set; }

        [JsonProperty("editable")]
        public required bool Editable { get; set; }

        [JsonProperty("explicit")]
        public required bool Explicit { get; set; }

        [JsonProperty("audioQuality")]
        public required string Quality { get; set; }

        [JsonProperty("album")]
        public required AlbumBase Album { get; set; }

        [JsonProperty("artist")]
        public required ArtistBase Artist { get; set; }

        [JsonProperty("artists")]
        public required ArtistBase[] Artists { get; set; }

        [Obsolete("Please use the track engine for retrieving stream information on a track instead.")]
        public async Task<StreamInfo> GetStreamInfo()
        {
            return await StaticEngine.GetStreamInfo(this);
        }
    }

    public class StreamInfo
    {
        [JsonProperty("trackId")]
        public required int TrackId { get; set; }

        [JsonProperty("assetPresentation")]
        public required string AssetPresentation { get; set; }

        [JsonProperty("audioMode")]
        public required string AudioMode { get; set; }

        [JsonProperty("audioQuality")]
        public required string AudioQuality { get; set; }

        [JsonProperty("manifestMimeType")]
        public required string ManifestMimeType { get; set; }

        [JsonProperty("manifestHash")]
        public required string ManifestHash { get; set; }

        [JsonProperty("manifest")]
        public required string Manifest { get; set; }

        [JsonProperty("albumReplayGain")]
        public required float AlbumReplayGain { get; set; }

        [JsonProperty("albumPeakAmplitude")]
        public required float AlbumPeakAmplitude { get; set; }

        [JsonProperty("trackReplayGain")]
        public required float TrackReplayGain { get; set; }

        [JsonProperty("trackPeakAmplitude")]
        public required float TrackPeakAmplitude { get; set; }

        [JsonIgnore]
        public StreamResource? StreamResource
        {
            get
            {
                byte[] byteArray = Convert.FromBase64String(Manifest);
                string decodedString = Encoding.UTF8.GetString(byteArray);
                return JsonConvert.DeserializeObject<StreamResource>(decodedString);
            }
        }
    }

    public class StreamResource
    {
        [JsonProperty("mimeType")]
        public required string MimeType { get; set; }

        [JsonProperty("codecs")]
        public required string Codecs { get; set; }

        [JsonProperty("encryptionType")]
        public required string EncryptionType { get; set; }

        [JsonProperty("urls")]
        public required string[] Urls { get; set; }
    }
}
