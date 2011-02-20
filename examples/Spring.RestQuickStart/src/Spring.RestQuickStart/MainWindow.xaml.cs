using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Windows;

using Spring.Rest.Client;
using Spring.Http;
using Spring.Http.Converters;
using Spring.Http.Converters.Json;

namespace Spring.RestQuickStart
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            // Note that you can also use the NJsonHttpMessageConverter based on Json.NET library 
            // that supports getting/setting values from JSON directly, 
            // without the need to deserialize/serialize to a .NET class.

            IHttpMessageConverter jsonConverter = new JsonHttpMessageConverter();
            jsonConverter.SupportedMediaTypes.Add(new MediaType("text", "javascript"));

            RestTemplate template = new RestTemplate();
            template.MessageConverters.Add(jsonConverter);

            template.GetForObjectAsync<GImagesResponse>("https://ajax.googleapis.com/ajax/services/search/images?v=1.0&rsz=8&q={query}",
                r =>
                {
                    if (r.Error == null)
                    {
                        this.ResultsItemsControl.ItemsSource = r.Response.Data.Items;
                    }
                }, this.SearchTextBox.Text);
        }

        [DataContract]
        public class GImagesResponse
        {
            [DataMember(Name = "responseData")]
            public GImagesResults Data { get; set; }

            [DataMember(Name = "responseStatus")]
            public int Status { get; set; }
        }

        [DataContract]
        public class GImagesResults
        {
            [DataMember(Name = "results")]
            public List<GImage> Items { get; set; }
        }

        [DataContract]
        public class GImage
        {
            [DataMember(Name = "visibleUrl")]
            public string SiteUrl { get; set; }

            [DataMember(Name = "tbWidth")]
            public int ThumbnailWidth { get; set; }

            [DataMember(Name = "tbHeight")]
            public int ThumbnailHeight { get; set; }

            [DataMember(Name = "tbUrl")]
            public string ThumbnailUrl { get; set; }   
        }
    }
}
