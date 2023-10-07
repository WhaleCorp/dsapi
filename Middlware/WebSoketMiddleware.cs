using dsapi.SocketController;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Text;

namespace dsapi.Middlware
{
    public class WebSoketMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public WebSoketMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            if (httpContext.WebSockets.IsWebSocketRequest)
            {
                var socket = await httpContext.WebSockets.AcceptWebSocketAsync();
                var guid = Guid.NewGuid().ToString();
                Socket.sockets.TryAdd(guid, socket);

                while (socket.State == WebSocketState.Open)
                {
                    var token = CancellationToken.None;
                    var buffer = new ArraySegment<byte>(new byte[4096]);
                    var received = await socket.ReceiveAsync(buffer, token);
                    Debug.WriteLine(received.MessageType);
                    switch (received.MessageType)
                    {
                        case WebSocketMessageType.Close:
                            Debug.WriteLine("It's close");
                            Socket.sockets.TryRemove(guid, out socket);
                            Socket.guids.TryRemove(Socket.guids.First(e => e.Value == guid).Key, out guid);
                            break;
                        case WebSocketMessageType.Binary:
                            Debug.WriteLine("It's binary");
                            break;
                        case WebSocketMessageType.Text:
                            var messageBytes = buffer.Skip(buffer.Offset).Take(received.Count).ToArray();
                            string receivedMessage = Encoding.UTF8.GetString(messageBytes);
                            if (Socket.guids.ContainsKey(receivedMessage)) return;
                            Socket.guids.TryAdd(receivedMessage, guid);
                            Socket.SendMessageAsync(receivedMessage, "recived");
                            break;
                    }
                }
            }
            else
            {
                await _next.Invoke(httpContext);
            }
        }
    }
}
