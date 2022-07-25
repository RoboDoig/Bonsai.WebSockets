using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Reactive.Linq;

namespace Bonsai.WebSockets
{
    [Combinator]
    public class Client
    {
        public string Host { get; set; }
        public int Port { get; set; }

        public IObservable<long> Process(IObservable<long> source)
        {
            return Observable.Using(() =>
            {
                return new TcpClient(Host, Port);
            },
            client =>
            {
                NetworkStream stream = client.GetStream();
                return source.Do(val =>
                {
                    Byte[] bytes = System.Text.Encoding.UTF8.GetBytes("test-gribble");
                    stream.Write(bytes, 0, bytes.Length);
                });
            });
        }
    }
}
