using System;
using System.Net.WebSockets;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;

namespace Bonsai.WebSockets
{
    public class SocketClient : Source<ClientWebSocket>
    {
        public string UriString { get; set; }

        public override IObservable<ClientWebSocket> Generate()
        {
            return Observable.Create<ClientWebSocket>(async (observer, cancellationToken) =>
            {
                var client = new ClientWebSocket();
                await client.ConnectAsync(new Uri(UriString), cancellationToken);

                observer.OnNext(client);

                var token = new CancellationToken();
                return Disposable.Create(async () => await client.CloseAsync(WebSocketCloseStatus.NormalClosure, "close", token));
            });
        }
    }
}
