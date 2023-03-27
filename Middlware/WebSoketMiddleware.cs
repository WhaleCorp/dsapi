using dsapi.SocketController;
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
        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext.WebSockets.IsWebSocketRequest)
            {
                var socket = await httpContext.WebSockets.AcceptWebSocketAsync();

                Socket.sockets.TryAdd(Guid.NewGuid().ToString(),socket);
                while (socket.State == WebSocketState.Open)
                {
                    var token = CancellationToken.None;
                    var buffer = new ArraySegment<byte>(new byte[4096]);
                    var received = await socket.ReceiveAsync(buffer, token);
                    switch (received.MessageType)
                    {
                        case WebSocketMessageType.Close:
                            // nothing to do for now...
                            break;

                        case WebSocketMessageType.Text:
                            var incoming = Encoding.UTF8.GetString(buffer.Array, buffer.Offset, buffer.Count);
                            // get rid of trailing crap from buffer
                            incoming = incoming.Replace("\0", "");
                            var data = Encoding.UTF8.GetBytes("data from server :" + DateTime.Now.ToLocalTime() + " " + incoming);
                            buffer = new ArraySegment<byte>(data);

                            // send to all open sockets
                            await Task.WhenAll(Socket.sockets.Where(s => s.Value.State == WebSocketState.Open)
                                .Select(s => s.Value.SendAsync(buffer, WebSocketMessageType.Text, true, token)));
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
