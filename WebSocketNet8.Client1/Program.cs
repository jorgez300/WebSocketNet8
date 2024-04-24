using System.ComponentModel.Design;
using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using WebSocketNet8Library;

Console.WriteLine("Press any key to start");
Console.ReadLine();



SocketClientService ws = new SocketClientService();

await ws.connect();


_ = Task.Run(async () =>
{
    while (true)
    {
        Console.WriteLine("Write a message");
        string msg = Console.ReadLine();
        await ws.Send(msg);

    }

}, ws.cts.Token);

_ = Task.Run(async () =>
{
    while (true)
    {

        string msg = await ws.Receive();
        await Console.Out.WriteLineAsync(msg);

    }

}, ws.cts.Token);


while (ws.isConnected()) { 

}




Console.WriteLine("Press any key to end");
Console.ReadLine();



//Uri uri = new("ws://localhost:5250/api/WebSocket/ws");
//var cts = new CancellationTokenSource();



//using (ClientWebSocket ws = new ClientWebSocket())
//{
//    ws.Options.SetRequestHeader("name", "Client 1");

//    cts.CancelAfter(TimeSpan.FromSeconds(120));

//    try
//    {
//        await ws.ConnectAsync(uri, cts.Token);

//        _ = Task.Run(async () =>
//        {

//            while (ws.State == WebSocketState.Open)
//            {
//                Console.WriteLine("Write a message");
//                string msg = Console.ReadLine();

//                if (msg == "Exit")
//                {
//                    cts.Cancel();
//                    await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "Client closed", default);
//                }
//                var bytes = Encoding.UTF8.GetBytes(msg);
//                var arraySegment = new ArraySegment<byte>(bytes, 0, bytes.Length);
//                await ws.SendAsync(arraySegment, WebSocketMessageType.Text, true, cts.Token);
//                cts.CancelAfter(TimeSpan.FromSeconds(120));
//            }

//        }, cts.Token);



//        _ = Task.Run(async () =>
//        {

//            while (ws.State == WebSocketState.Open)
//            {
//                Task.Delay(1000).Wait();
//                var buffer = new byte[1024 * 4];
//                var result = await ws.ReceiveAsync(new ArraySegment<byte>(buffer), cts.Token);
//                var rspmsg = Encoding.UTF8.GetString(buffer, 0, result.Count);
//                Console.WriteLine(rspmsg);
//                cts.CancelAfter(TimeSpan.FromSeconds(120));
//            }


//        }, cts.Token);

//        while (ws.State == WebSocketState.Open)
//        {

//        }


//    }
//    catch (WebSocketException e)
//    {
//        cts.Cancel();
//        await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "Client closed", default);
//        throw e;
//    }

//}







