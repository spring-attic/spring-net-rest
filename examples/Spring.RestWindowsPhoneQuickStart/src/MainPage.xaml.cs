using System;
using System.Windows;
using System.Collections.Generic;
using System.Runtime.Serialization;
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
            twitter.GetPublicTimeline(
                r =>
                {
                    this.TweetListBox.ItemsSource = r;
                });
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            twitter.Search(this.SearchTextBox.Text,
                r =>
                {
                    this.TweetListBox.ItemsSource = r;
                });
        }

        private void AccountButton_Click(object sender, RoutedEventArgs e)
        {
            twitter.GetUserTimeline(this.AccountTextBox.Text,
                r =>
                {
                    this.TweetListBox.ItemsSource = r;
                });
        }
    }
}