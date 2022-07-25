using System;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Reactive.Linq;

namespace Bonsai.WebSockets
{
    [Combinator]
    public class WebSocketClient
    {
        public string Address { get; set; }

        public IObservable<long> Process(IObservable<long> source)
        {
            return Observable.Using(() =>
            {
                ClientWebSocket socket = new ClientWebSocket();
                socket.ConnectAsync(new Uri(Address), new System.Threading.CancellationToken()).GetAwaiter().GetResult();
                return socket;
            },
            socket =>
            {
                return source.Do(val =>
                {
                    Console.WriteLine(socket.State);
                });
            });
        }
    }
}
