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
                Bitmap nuGetLogo = template.GetForObjectAsync<Bitmap>("http://nuget.org/Content/Images/nugetlogo.png").Result;
#else
                Bitmap nuGetLogo = template.GetForObject<Bitmap>("http://nuget.org/Content/Images/nugetlogo.png");
#endif

                // Save image to disk
                string filename = Path.Combine(Environment.CurrentDirectory, "NuGetLogo.png");
                nuGetLogo.Save(filename);
                Console.WriteLine(String.Format("Saved NuGet logo to '{0}'", filename));
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

