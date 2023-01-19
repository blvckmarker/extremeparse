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
        var responseChars = new char[1024];

        int charCount = 0;

        while (true)
        {
            int waitFor = socket.Available;
            int bytesRecieved = await socket.ReceiveAsync(responseBytes, SocketFlags.None);

            charCount = Encoding.ASCII.GetChars(responseBytes, 0, bytesRecieved, responseChars, 0);

            if (bytesRecieved == waitFor || waitFor == 0) break;
        }
        var response = Encoding.ASCII.GetString(responseBytes, 0, charCount);
        var card = JsonConvert.DeserializeObject<CardModel>(response);

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
