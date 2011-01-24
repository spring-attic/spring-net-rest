using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Net.Browser;

using Spring.Http.Client;
using Spring.Rest.Client;

namespace Spring.RestSilverlightQuickStart
{
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            RestTemplate rt = new RestTemplate("http://localhost:12345/Services/Service.svc/");
            //WebClientHttpRequestFactory requestFactory = new WebClientHttpRequestFactory();
            //requestFactory.WebRequestCreator = WebRequestCreatorType.ClientHttp;
            //rt.RequestFactory = requestFactory;

            rt.GetForMessageAsync<string>("users", 
                r =>
                {
                    if (r.Error != null)
                    {
                        TextBlock.Text = r.Error.ToString();
                    }
                    else
                    {
                        TextBlock.Text = String.Format("{0}; {1}; {2}; {3}",
                            r.Response.Body, r.Response.Headers.Location, r.Response.StatusCode, r.Response.StatusDescription);
                    }
                });

            //rt.PostForMessageAsync<string>("user", "Lisa Baia", 
            //    r =>
            //    {
            //        if (r.Error != null)
            //        {
            //            TextBlock.Text = r.Error.ToString();
            //        }
            //        else
            //        {
            //            TextBlock.Text = String.Format("{0}; {1}; {2}; {3}",
            //                r.Response.Body, r.Response.Headers.Location, r.Response.StatusCode, r.Response.StatusDescription);
            //        }
            //    });
        }
    }
}
