using System.Net.Http.Headers;
using TidalWrapper.Responses;
using TidalWrapper.Exceptions;

namespace TidalWrapper.API
{
    public class Search: Engine
    {
        public async Task<TrackSearch> SearchTracks(string query, int? limit=10, string? countryCode="US")
        {
            Response<TrackSearch> tracks = await Request.GetJsonAsync<TrackSearch> (httpClient, $"https://api.tidal.com/v1/search/tracks?countryCode={countryCode}&query={query}&limit={limit}");
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
