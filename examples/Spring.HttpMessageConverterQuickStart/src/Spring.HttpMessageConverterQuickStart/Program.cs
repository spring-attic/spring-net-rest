using System;
using System.IO;
using System.Drawing;

using Spring.Rest.Client;

using Spring.HttpMessageConverterQuickStart.Converters;

namespace Spring.HttpMessageConverterQuickStart
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Configure RestTemplate by adding the new converter to the default list
                RestTemplate template = new RestTemplate();
                template.MessageConverters.Add(new ImageHttpMessageConverter());

                // Get image from url
#if NET_4_0
                Bitmap gitHubLogo = template.GetForObjectAsync<Bitmap>("https://github.com/images/modules/header/logov3.png").Result;
#else
                Bitmap gitHubLogo = template.GetForObject<Bitmap>("https://github.com/images/modules/header/logov3.png");
#endif

                // Save image to disk
                string filename = Path.Combine(Environment.CurrentDirectory, "GitHubLogo.png");
                gitHubLogo.Save(filename);
                Console.WriteLine(String.Format("Saved GitHub logo to '{0}'", filename));
            }
#if NET_4_0
            catch (AggregateException ae)
            {
                ae.Handle(ex =>
                {
                    if (ex is HttpResponseException)
                    {
                        Console.WriteLine(((HttpResponseException)ex).GetResponseBodyAsString());
                        return true;
                    }
                    return false;
                });
            }
#else
            catch (HttpResponseException ex)
            {
                Console.WriteLine(ex.GetResponseBodyAsString());
            }
#endif
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

