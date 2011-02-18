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
                Bitmap gitHubLogo = template.GetForObject<Bitmap>("https://github.com/images/modules/header/logov3.png");

                // Save image to disk
                string filename = Path.Combine(Environment.CurrentDirectory, "GitHubLogo.png");
                gitHubLogo.Save(filename);
                Console.WriteLine(String.Format("Saved GitHub logo to '{0}'", filename));
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

