using System.Net.WebSockets;
using System.Text;

namespace WebSocketNet8Library
{
    public class SocketServerService
    {
        public List<WebSocket> webSockets = new List<WebSocket>();

        CancellationTokenSource cts = new CancellationTokenSource();

        public async Task connect(WebSocket webSocket, string userName)
        {

            webSockets.Add(webSocket);

            await Broadcast($"{userName}: joined");
            await Broadcast($"{webSockets.Count} users");

            await ReceiveMesage(webSocket, userName);

            cts.CancelAfter(TimeSpan.FromSeconds(120));

        }

        public async Task ReceiveMesage(WebSocket webSocket, string name)
        {


            var buffer = new byte[1024 * 4];

            while (!webSocket.CloseStatus.HasValue)
            {
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), cts.Token);
                if (result.MessageType == WebSocketMessageType.Text)
                {

                    await Broadcast($"{name}: {Encoding.UTF8.GetString(buffer, 0, result.Count)}");

                }
                else
                {

                    webSockets.Remove(webSocket);

                    await Broadcast($"{name}: left");
                    await Broadcast($"{webSockets.Count} users");

                    await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, cts.Token);
                }
            }
        }

        public async Task Broadcast(string msg)
        {
            await Console.Out.WriteLineAsync(msg);
            var bytes = Encoding.UTF8.GetBytes(msg);
            foreach (var socket in webSockets)
            {
                if (socket.State == WebSocketState.Open)
                {
                    var arraySegment = new ArraySegment<byte>(bytes, 0, bytes.Length);
                    await socket.SendAsync(arraySegment, WebSocketMessageType.Text, true, cts.Token);
                }
            }

        }

        public async Task StopServerAsync()
        {
            await Broadcast("Server: closed");
            cts.Cancel();

        }
    }
}
