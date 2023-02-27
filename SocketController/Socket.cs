using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;

namespace dsapi.SocketController
{
    public static class Socket
    {
        public static ConcurrentDictionary<string, WebSocket> sockets = new ConcurrentDictionary<string, WebSocket>();

        public static Task SendMessage(string guid,string data,CancellationToken ct = default(CancellationToken))
        {
            var buffer = Encoding.UTF8.GetBytes(data);
            var segment = new ArraySegment<byte>(buffer);

            return sockets[guid].SendAsync(segment,WebSocketMessageType.Text,true,ct);
        }

        public static void CleanSoket()
        {
            foreach(var socket in sockets)
            {
                if (socket.Value.State != WebSocketState.Open)
                    sockets.TryRemove(socket);
            }
        }
    }
}
