using ExtremeParse.Models;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ExtremeParse.Bot.Client.Mediator.Parser;
internal static class ParserMediator
{
    private static TcpClient TcpClient = null!;
    private static IPAddress host = null!;
    private static string envPath = null!;
    private static int port;


    public static async Task<CardModel> DispatchData(string commandName, string args)
    {
        var message = Encoding.ASCII.GetBytes($"{commandName} {args}");
        using var stream = TcpClient.GetStream();

        stream.Write(message, 0, message.Length);
        message = new byte[4096];

        var bytes = stream.Read(message, 0, message.Length);
        var response = Encoding.ASCII.GetString(message, 0, bytes);

        await Console.Out.WriteLineAsync($"Recieved {response}");

        return new CardModel();
    }

    public static void Configure(string envfilepath)
    {
        envPath = envfilepath;

        var envVariables = File.ReadAllLines(envPath).Select(str => new { Name = str.Split('=')[0], Value = str.Split('=')[1] });

        host = IPAddress.Parse(envVariables.First(item => item.Name == "HOST").Value);
        port = int.Parse(envVariables.First(item => item.Name == "PORT").Value);

        TcpClient = new TcpClient(host.ToString(), port);
    }
}
