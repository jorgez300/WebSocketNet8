using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace WebSocketNet8Library
{
    public class SocketClientService : IDisposable
    {

        Uri uri = new("ws://localhost:5250/api/WebSocket/ws");
        public CancellationTokenSource cts = new CancellationTokenSource();
        ClientWebSocket ws = new ClientWebSocket();

        public async Task Send(string msg) {


            if (msg == "Exit")
            {
               await disconnect();
            }
            var bytes = Encoding.UTF8.GetBytes(msg);
            var arraySegment = new ArraySegment<byte>(bytes, 0, bytes.Length);
            await ws.SendAsync(arraySegment, WebSocketMessageType.Text, true, cts.Token);
            cts.CancelAfter(TimeSpan.FromSeconds(120));


        }

        public async Task<string> Receive() {

            var buffer = new byte[1024 * 4];
            var result = await ws.ReceiveAsync(new ArraySegment<byte>(buffer), cts.Token);
            var rspmsg = Encoding.UTF8.GetString(buffer, 0, result.Count);
            cts.CancelAfter(TimeSpan.FromSeconds(120));

            return rspmsg;

        }

        public async Task connect() {

            ws.Options.SetRequestHeader("name", "Client 1");

            cts.CancelAfter(TimeSpan.FromSeconds(120));

            await ws.ConnectAsync(uri, cts.Token);

        }

        public bool isConnected() {

            return (ws.State == WebSocketState.Open);
        }

        public async Task disconnect() {

            cts.Cancel();
            await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "Client closed", default);
        }

        public void Dispose()
        {
            _ = disconnect();
        }
    }
}
