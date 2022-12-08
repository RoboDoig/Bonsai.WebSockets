using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bonsai.WebSockets
{
    public class ReceiveMessage : Combinator<ClientWebSocket, string>
    {
        const int chunkSize = 1024 * 4;
        ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[chunkSize]);

        public override IObservable<string> Process(IObservable<ClientWebSocket> source)
        {
            return Observable.Create<string>(async (observer, cancellationToken) =>
            {
                var result = await source.Select(x =>
                {
                    x.ReceiveAsync(buffer, cancellationToken);
                    return buffer.ToString();
                });

                observer.OnNext(result);
            }).Repeat();
        }
    }
}
