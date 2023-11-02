﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TidalWrapper.Exceptions;
using TidalWrapper.Responses;

namespace TidalWrapper.API
{
    /// <summary>
    /// Static Engine
    /// </summary>
    internal static class StaticEngine
    {
        internal static readonly HttpClient httpClient = Request.CreateClient();

        /// <summary>
        /// Retrieves stream info for a given track
        /// </summary>
        /// <param name="track">Track entity</param>
        /// <returns>Stream info for the track</returns>
        /// <exception cref="APIException">Request failure</exception>
        /// <exception cref="Exception">Misc. failure</exception>
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
