using System;
using System.Linq;
using System.Collections.Generic;

using Spring.Rest.Client;
using Spring.Http.Converters;
using Newtonsoft.Json.Linq;

using Spring.HttpMessageConverterQuickStart.Converters;

namespace Spring.HttpMessageConverterQuickStart
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                RestTemplate rt = new RestTemplate("http://twitter.com");
                // Override pre-defined message converters
                rt.MessageConverters = new List<IHttpMessageConverter>(); 
                rt.MessageConverters.Add(new NJsonHttpMessageConverter());
#if SILVERLIGHT
                rt.GetForObjectAsync<JArray>("/statuses/user_timeline.json?screen_name={name}&count={count}", 
                    r =>
                    {
                        if (r.Error == null)
                        {
                            var tweets = from el in r.Response.Children()
                                         select el.Value<string>("text");
                            foreach (string tweet in tweets)
                            {
                                Console.WriteLine(String.Format("* {0}", tweet));
                                Console.WriteLine();
                            }
                        }
                        else
                        {
                            Console.WriteLine(r.Error);
                        }
                    }, "SpringForNet", 10);
#else
                JArray jArray = rt.GetForObject<JArray>("/statuses/user_timeline.json?screen_name={name}&count={count}", "SpringForNet", 10);
                var tweets = from el in jArray.Children()
                             select el.Value<string>("text");
                foreach (string tweet in tweets)
                {
                    Console.WriteLine(String.Format("* {0}", tweet));
                    Console.WriteLine();
                }
#endif
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
			finally
			{
				Console.WriteLine("--- hit <return> to quit ---");
				Console.ReadLine();
			}
        }
    }
}

