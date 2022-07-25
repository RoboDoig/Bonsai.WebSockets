using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Bonsai.WebSockets
{
    public class Server : Source<int>
    {
        public string IpAddress { get; set; }
        public int Port { get; set; }

        public override IObservable<int> Generate()
        {
            return Observable.Create<int>((observer, cancellationToken) =>
            {
                // Setup server
                var server = new TcpListener(IPAddress.Parse(IpAddress), Port);
                server.Start();

                cancellationToken.Register(() => { server.Stop(); });

                // TODO - server listener logic
                return Task.Factory.StartNew(() =>
                {
                    var client = server.AcceptTcpClient();
                    NetworkStream stream = client.GetStream();

                    while (!cancellationToken.IsCancellationRequested)
                    {
                        while (!stream.DataAvailable) ;

                        Byte[] bytes = new Byte[client.Available];
                        stream.Read(bytes, 0, bytes.Length);

                        observer.OnNext(bytes.Length);
                    }
                });
            });
        }
    }
}
