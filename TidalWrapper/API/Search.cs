using TidalWrapper.Responses;
using TidalWrapper.Exceptions;
using TidalWrapper.Requests;

namespace TidalWrapper.Engines
{
    /// <summary>
    /// Search Engine (Used for track/album/playlist search)
    /// </summary>
    [Obsolete("Using the search engine is deprecated. Future methods will go in according engines, including Track Engine, Album engine etc.")]
    public class SearchEngine : Engine
    {
        public SearchEngine(Client client, bool useToken) : base(client, useToken) { }
        /// <summary>
        /// Searches for tracks using a search query
        /// </summary>
        /// <param name="query">Query to search for</param>
        /// <param name="limit">Limit results</param>
        /// <param name="countryCode">Country code</param>
        /// <returns>Track results, including some meta information</returns>
        /// <exception cref="APIException">Request failure</exception>
        /// <exception cref="Exception">Misc. failure</exception>
        public async Task<TrackSearch> SearchTracks(string query, int? limit = 10, string? countryCode = "US")
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
    }
}
