using System.Net;

namespace VRCProxyChecker
{
    internal class Main
    {
        public static void Init()
        {
            string[] Proxies = File.ReadAllLines("Proxie.txt");
            List<string> Working = new();
            foreach (string Proxie in Proxies)
            {
                WebProxy Proxy;
                if (Proxie.Contains("@"))
                {
                    string[] Splitted = Proxie.Split(':', '@');
                    string AuthUser = Splitted[0];
                    string AuthPassword = Splitted[1];

                    Proxy = new WebProxy { Address = new Uri("http://" + Proxie), Credentials = new NetworkCredential(AuthUser, AuthPassword) };
                }
                else Proxy = new WebProxy { Address = new Uri("http://" + Proxie) };

                HttpClient Client = new(new HttpClientHandler { UseCookies = false, Proxy = Proxy });
                Client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/100.0.4896.79 Safari/537.36");
                HttpRequestMessage loginPayLoad = new (HttpMethod.Get, "https://api.vrchat.cloud/api/1/visits");
                HttpResponseMessage loginResp = Client.Send(loginPayLoad);
                if (loginResp.IsSuccessStatusCode) Working.Add(Proxie);
            }
            File.WriteAllLines("Working.txt", Working.ToArray());
        }
    }
}
