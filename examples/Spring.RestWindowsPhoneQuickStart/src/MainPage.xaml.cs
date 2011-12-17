using System.Windows;
using Microsoft.Phone.Controls;

using Spring.RestWindowsPhoneQuickStart.Twitter;

namespace Spring.RestWindowsPhoneQuickStart
{
    public partial class MainPage : PhoneApplicationPage
    {
        TwitterClient twitter = new TwitterClient();

        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            twitter.GetPublicTimelineAsync(
                r =>
                {
                    this.TweetListBox.ItemsSource = r;
                });
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            twitter.SearchAsync(this.SearchTextBox.Text,
                r =>
                {
                    this.TweetListBox.ItemsSource = r;
                });
        }

        private void AccountButton_Click(object sender, RoutedEventArgs e)
        {
            twitter.GetUserTimelineAsync(this.AccountTextBox.Text,
                r =>
                {
                    this.TweetListBox.ItemsSource = r;
                });
        }
    }
}