using ExtremeParse.Models;
using Newtonsoft.Json;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ExtremeParse.Bot.Client.Mediator.Parser;
internal static class ParserMediator
{
    private static Socket socket = null!;
    private static IPAddress host = null!;
    private static string envPath = null!;
    private static int port;


    public static async Task<CardModel> DispatchData(string commandName, string args)
    {
        var request = Encoding.ASCII.GetBytes($"{commandName} {args}");

        int bytesSend = 0;
        while (bytesSend < request.Length)
            bytesSend += await socket.SendAsync(request.AsMemory(), SocketFlags.None);

        var responseBytes = new byte[1024];
        int byteRead = 0;

        while (socket.Available != 0)
            byteRead += await socket.ReceiveAsync(responseBytes, SocketFlags.None);

        var responseString = Encoding.ASCII.GetString(responseBytes, 0, byteRead);
        var card = JsonConvert.DeserializeObject<CardModel>(responseString);

        //(responseBytes, 0, bytesRecieved, responseChars, 0);

        return card;
    }

    public static void Configure(string envfilepath)
    {
        envPath = envfilepath;

        var envVariables = File.ReadAllLines(envPath).Select(str => new { Name = str.Split('=')[0], Value = str.Split('=')[1] });

        host = IPAddress.Parse(envVariables.First(item => item.Name == "HOST").Value);
        port = int.Parse(envVariables.First(item => item.Name == "PORT").Value);

        socket = new(SocketType.Stream, ProtocolType.Tcp);
        socket.Connect(host, port);
    }
}
