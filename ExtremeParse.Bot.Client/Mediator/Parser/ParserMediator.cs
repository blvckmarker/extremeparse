using ExtremeParse.Models;
using Newtonsoft.Json;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ExtremeParse.Bot.Client.Mediator.Parser;
internal static class ParserMediator
{
    private static TcpClient TcpClient = new();
    private static IPAddress host = null!;
    private static string envPath = null!;
    private static int port;


    public static async Task<CardModel> DispatchData(string commandName, string args)
    {
        if (!TcpClient.Connected)
            TcpClient = new(host.ToString(), port);



        var message = Encoding.UTF8.GetBytes($"{commandName} {args}");
        using var stream = TcpClient.GetStream();

        await stream.WriteAsync(message);
        message = new byte[stream.Socket.Available];
        var bytes = await stream.ReadAsync(message, 0, message.Length);

        TcpClient.Close();

        var response = Encoding.UTF8.GetString(message, 0, bytes);
        var card = JsonConvert.DeserializeObject<CardModel>(response);

        return card ?? new CardModel();
    }

    public static void Configure(string envfilepath)
    {
        envPath = envfilepath;

        var envVariables = File.ReadAllLines(envPath).Select(str => new { Name = str.Split('=')[0], Value = str.Split('=')[1] });

        host = IPAddress.Parse(envVariables.First(item => item.Name == "HOST").Value);
        port = int.Parse(envVariables.First(item => item.Name == "PORT").Value);
    }
}
