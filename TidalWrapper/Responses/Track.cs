using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TidalWrapper.API;
using TidalWrapper.Exceptions;

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
            get {
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
