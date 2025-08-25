using System.Text.RegularExpressions;
using TidalWrapper.Exceptions;
using TidalWrapper.Responses;
using TidalWrapper.Requests;

namespace TidalWrapper.Engines
{
    public class TrackEngine : Engine
    {
        public TrackEngine(Client client, bool useToken) : base(client, useToken) { }

        /// <summary>
        /// Searches for tracks using a search query
        /// </summary>
        /// <param name="query">Query to search for</param>
        /// <param name="limit">Limit results</param>
        /// <param name="countryCode">Country code</param>
        /// <returns>Track results, including some meta information</returns>
        /// <exception cref="APIException">Request failure</exception>
        /// <exception cref="Exception">Misc. failure</exception>
        public async Task<TrackSearch> Search(string query, int? limit = 10, string? countryCode = "US")
        {
            Response<TrackSearch> tracks = await Request.GetJsonAsync<TrackSearch>(httpClient, $"https://api.tidal.com/v1/search/tracks?countryCode={countryCode}&query={query}&limit={limit}");
            if (tracks.Data != null)
            {
                return tracks.Data;
            }
            else if (tracks.Error != null)
            {
                throw new APIException { StatusCode = tracks.StatusCode, Error = tracks.Error };
            }
            else
            {
                throw new Exception("Could not search for tracks.");
            }
        }

        /// <summary>
        /// Retrieves track information based on a given track id
        /// </summary>
        /// <param name="trackId">Track id</param>
        /// <param name="countryCode">Country code</param>
        /// <returns>Track information</returns>
        /// <exception cref="ArgumentException">Malformatted id</exception>
        /// <exception cref="APIException">Request failure</exception>
        /// <exception cref="Exception">Misc. failure</exception>
        public async Task<Track> GetTrackById(string trackId, string? countryCode = "US")
        {
            if (!TrackUtil.IsValidId(trackId))
            {
                throw new ArgumentException("Invalid track id format.");
            }

            Response<Track> track = await Request.GetJsonAsync<Track>(httpClient, $"https://api.tidal.com/v1/tracks/{trackId}?countryCode={countryCode}");
            if (track.Data != null)
            {
                return track.Data;
            }
            else if (track.Error != null)
            {
                throw new APIException { StatusCode = track.StatusCode, Error = track.Error };
            }
            else
            {
                throw new Exception("Could not get track.");
            }
        }

        /// <summary>
        /// Retrieves track information based on a given track url
        /// </summary>
        /// <param name="trackUrl">Track url</param>
        /// <param name="countryCode">Country code</param>
        /// <returns>Track information</returns>
        /// <exception cref="ArgumentException">Malformatted url</exception>
        /// <exception cref="APIException">Request failure</exception>
        /// <exception cref="Exception">Misc. failure</exception>
        public async Task<Track> GetTrackByUrl(string trackUrl, string? countryCode = "US")
        {
            // Remove query params from string
            int queryIndex = trackUrl.IndexOf('?');
            if (queryIndex >= 0)
            {
                trackUrl = trackUrl.Substring(0, queryIndex);
            }

            if (!TrackUtil.IsValidUrl(trackUrl))
            {
                throw new ArgumentException("Invalid track URL format.");
            }

            // Extract track id
            string[] splittedTrack = trackUrl.Split("track/");
            if (splittedTrack.Length < 2 || string.IsNullOrWhiteSpace(splittedTrack[1]))
            {
                throw new ArgumentException("Track ID could not be extracted.");
            }

            string trackId = splittedTrack[1].Split('/')[0];
            Response<Track> track = await Request.GetJsonAsync<Track>(httpClient, $"https://api.tidal.com/v1/tracks/{trackId}?countryCode={countryCode}");
            if (track.Data != null)
            {
                return track.Data;
            }
            else if (track.Error != null)
            {
                throw new APIException { StatusCode = track.StatusCode, Error = track.Error };
            }
            else
            {
                throw new Exception("Could not get track.");
            }
        }


        /// <summary>
        /// Retrieves stream info for a given track
        /// </summary>
        /// <param name="track">Track entity</param>
        /// <param name="countryCode">Country code</param>
        /// <returns>Stream info for the track</returns>
        /// <exception cref="APIException">Request failure</exception>
        /// <exception cref="Exception">Misc. failure</exception>
        public async Task<StreamInfo> GetStreamInfo(Track track, string? countryCode = "US")
        {
            string url = $"https://api.tidal.com/v1/tracks/{track.Id}/playbackinfopostpaywall?countryCode={countryCode}&audioquality={track.Quality}&playbackmode=STREAM&assetpresentation=FULL";
            Response<StreamInfo> streamInfo = await Request.GetJsonAsync<StreamInfo>(httpClient, url);
            if (streamInfo.Data != null)
            {
                return streamInfo.Data;
            }
            else if (streamInfo.Error != null)
            {
                throw new APIException { StatusCode = streamInfo.StatusCode, Error = streamInfo.Error };
            }
            else
            {
                throw new Exception("Could not fetch stream info.");
            }
        }
    }

    public partial class TrackUtil
    {
        [GeneratedRegex("^[0-9]+$")]
        private static partial Regex TrackId();

        [GeneratedRegex("^(https?://)?(www\\.)?tidal\\.com/((browse/track/|track/)\\d+)(/)?$")]
        private static partial Regex TrackUrl();

        public static bool IsValidId(string trackId)
        {
            return TrackId().IsMatch(trackId);
        }

        public static bool IsValidUrl(string url)
        {
            return TrackUrl().IsMatch(url);
        }
    }
}