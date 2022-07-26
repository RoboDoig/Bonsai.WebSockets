using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using System.Text.Json;

namespace Bonsai.WebSockets
{
    public class RestClient : Combinator<string>
    {
        public string Address { get; set; }
        public string Route { get; set; }
        public RestClient.RequestType RequestMethod { get; set; }

        public override IObservable<string> Process<TSource>(IObservable<TSource> source)
        {
            return Observable.Using(() =>
            {
                return new RestSharp.RestClient(Address);
            },
            client =>
            {
                return source.Select(val =>
                {
                    //var request = new RestRequest("/public/v2/users/2");
                    //Observable.FromAsync(() => client.GetAsync(request));
                    ////TODO async
                    
                    var request = new RestRequest(Route);

                    if (RequestMethod == RequestType.GET)
                    {
                        var response = client.Get(request);
                        return response.Content;
                    } else
                    {
                        request.AddJsonBody(new TestRequest { Name = "Bob", Salary = "123", Age = "30" });
                        var response = client.Post(request);
                        return response.Content;
                    }
                });
            });
        }

        public class TestRequest
        {
            public string Name;
            public string Salary;
            public string Age;
        }

        public class PupilEvent
        {
            public string name;
            public Int64 timestamp;
        }

        public enum RequestType
        {
            GET,
            POST
        }
    }
}
