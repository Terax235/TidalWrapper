using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TidalWrapper.Exceptions;
using TidalWrapper.Responses;

namespace TidalWrapper.API
{
    internal static class StaticEngine
    {
        internal static readonly HttpClient httpClient = Request.CreateClient();

        public static async Task<StreamInfo> GetStreamInfo(Track track)
        {
            string url = $"https://api.tidal.com/v1/tracks/{track.Id}/playbackinfopostpaywall?countryCode=DE&audioquality=HI_RES&playbackmode=STREAM&assetpresentation=FULL";
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

        internal static void SetAuth(OAuthToken auth)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(auth.TokenType, auth.AccessToken);
        }
    }
}
