using TidalWrapper.Requests;

namespace TidalWrapper.Engines
{
    /// <summary>
    /// Abstract Engine
    /// </summary>
    public abstract class Engine
    {
        /// <summary>
        /// HTTP client for the engine
        /// </summary>
        internal readonly APIClient httpClient;
        internal readonly Client client;

        public Engine(Client client, bool useAuthorizationToken)
        {
            this.client = client;
            httpClient = new(client, useAuthorizationToken);
        }
    }
}
