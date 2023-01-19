using ExtremeParse.Models;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace ExtremeParse.Bot.Client.Mediator.Website.Server;
internal static class HttpClientSender
{
    private readonly static Uri uri = new("https://localhost:7122/User/api/tg/add"); //TODO

    public async static Task<HttpStatusCode> SendData(CardModel Card)
    {
        var request = JsonConvert.SerializeObject(Card);

        using var HttpClient = new HttpClient();
        var content = new StringContent(request, Encoding.UTF8, "application/json");

        try
        {
            var response = await HttpClient.PostAsync(uri, content);
            return response.StatusCode;
        }
        catch { return HttpStatusCode.NotFound; }
        finally { HttpClient.Dispose(); }
    }
}
