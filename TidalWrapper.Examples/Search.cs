using TidalWrapper.Responses;

namespace TidalWrapper.Examples
{
    public class Search
    {
        public static async Task Main(string[] args)
        {
            Client client = new("zU4XHVVkc2tDPo4t");

            if (args.Length == 0)
            {
                await client.LoginWithDeviceCode(); // Login using device code, which will enter an interactive prompt
                Console.WriteLine("You can use the following refresh token to login in future:");
                Console.WriteLine(client.AuthCache?.RefreshToken);
            }
            else
            {
                await client.LoginWithRefreshToken(args[0]); // Try to login with a provided refresh token
            }

            Console.Write("Please enter a search query: ");
            string? query = Console.ReadLine();
            if (!string.IsNullOrEmpty(query))
            {
                TrackSearch results = await client.Tracks.Search(query, 10, client.CountryCode);
                Console.WriteLine($"=== Received {results.ItemCount} tracks ===");
                for (int i = 0; i < results.Items.Length; i++)
                {
                    Track track = results.Items[i];
                    Console.WriteLine($"{i + 1}. {track.Title} ({track.Url})");
                }
            }
        }
    }
}
