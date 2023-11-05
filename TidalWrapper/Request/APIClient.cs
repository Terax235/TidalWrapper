namespace TidalWrapper.Requests
{
    internal class APIClient : HttpClient
    {
        public static string UserAgent { get; set; } = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) TIDAL/2.30.4 Chrome/91.0.4472.164 Electron/13.6.9 Safari/537.36"; // Todo
        public bool UseAuthorizationToken;
        internal readonly Client? client;
        public APIClient(Client client, bool useAuthorizationToken = true) : base()
        {
            this.client = client;

            DefaultRequestHeaders.Add("User-Agent", UserAgent);
            UseAuthorizationToken = useAuthorizationToken;
        }

        public APIClient()
        {
            UseAuthorizationToken = false;
        }
    }
}