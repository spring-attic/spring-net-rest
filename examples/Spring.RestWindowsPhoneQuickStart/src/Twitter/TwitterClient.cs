using System;
using System.Collections.Generic;

using Spring.Json;
using Spring.Rest.Client;
using Spring.Http.Converters.Json;

namespace Spring.RestWindowsPhoneQuickStart.Twitter
{
    // See Spring.Social Twitter project: https://www.springframework.net/social-twitter/

    // Central class for interacting with Twitter
    // Simplified implementation that only performs unauthenticated operations against Twitter's API.
    public class TwitterClient
    {
        private RestTemplate restTemplate;

        private const string API_URL_BASE = "https://api.twitter.com/1/";
        private const string PUBLIC_TIMELINE_URL = "statuses/public_timeline.json";
        private const string USER_TIMELINE_URL = "statuses/user_timeline.json";

        private const string SEARCH_URL = "https://search.twitter.com/search.json?q={query}&rpp={rpp}&page={page}";

        public TwitterClient()
        {
            restTemplate = new RestTemplate(API_URL_BASE);
            // Using simple Spring.NET JSON support instead of JSON.NET (for light-weight WP apps)
            restTemplate.MessageConverters.Add(new SpringJsonHttpMessageConverter());
        }

        public void GetPublicTimelineAsync(Action<IList<Tweet>> operationCompleted)
        {
            restTemplate.GetForObjectAsync<JsonValue>(PUBLIC_TIMELINE_URL,
                r =>
                {
                    IList<Tweet> tweets = ExtractTimelineTweetsFromResponse(r.Response);
                    operationCompleted(tweets);
                });
        }

        public void GetUserTimelineAsync(string screenName, Action<IList<Tweet>> operationCompleted)
        {
            restTemplate.GetForObjectAsync<JsonValue>(USER_TIMELINE_URL + "?screen_name={screenName}",
                r =>
                {
                    IList<Tweet> tweets = ExtractTimelineTweetsFromResponse(r.Response);
                    operationCompleted(tweets);
                }, screenName);
        }

        public void GetUserTimelineAsync(long userId, Action<IList<Tweet>> operationCompleted)
        {
            restTemplate.GetForObjectAsync<JsonValue>(USER_TIMELINE_URL + "?user_id={userId}",
                r =>
                {
                    IList<Tweet> tweets = ExtractTimelineTweetsFromResponse(r.Response);
                    operationCompleted(tweets);
                }, userId);
        }

        public void SearchAsync(string query, Action<IList<Tweet>> operationCompleted)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("query", query);
            parameters.Add("rpp", 50);
            parameters.Add("page", 1);

            restTemplate.GetForObjectAsync<JsonValue>(SEARCH_URL, parameters,
               r =>
               {
                   IList<Tweet> tweets = new List<Tweet>();
                   foreach (JsonValue item in r.Response.GetValues("results"))
                   {
                       tweets.Add(PopulateTweetFromSearchResults(item));
                   }
                   operationCompleted(tweets);
               });
        }


        private IList<Tweet> ExtractTimelineTweetsFromResponse(JsonValue response)
        {
            IList<Tweet> tweets = new List<Tweet>();
            foreach (JsonValue item in response.GetValues())
            {
                tweets.Add(PopulateTweetFromTimelineItem(item));
            }
            return tweets;
        }

        private Tweet PopulateTweetFromTimelineItem(JsonValue item)
        {
            Tweet tweet = new Tweet();

            tweet.ID = item.GetValue<long?>("id");
            tweet.Text = item.GetValue<string>("text");
            tweet.FromUser = item.GetValue("user").GetValue<string>("screen_name");
            tweet.FromUserId = item.GetValue("user").GetValue<long?>("id");
            tweet.ProfileImageUrl = item.GetValue("user").GetValue<string>("profile_image_url");
            tweet.Source = item.GetValue<string>("source");
            tweet.ToUserId = item.GetValue<long?>("in_reply_to_user_id");
            tweet.CreatedAt = item.GetValue<string>("created_at");

            return tweet;
        }

        private Tweet PopulateTweetFromSearchResults(JsonValue item)
        {
            Tweet tweet = new Tweet();

            tweet.ID = item.GetValue<long?>("id");
            tweet.FromUser = item.GetValue<string>("from_user");
            tweet.Text = item.GetValue<string>("text");
            tweet.CreatedAt = item.GetValue<string>("created_at");
            tweet.FromUserId = item.GetValue<long?>("from_user_id");
            tweet.ToUserId = item.GetValue<long?>("to_user_id");
            tweet.LanguageCode = item.GetValue<string>("iso_language_code");
            tweet.ProfileImageUrl = item.GetValue<string>("profile_image_url");
            tweet.Source = item.GetValue<string>("source");

            return tweet;
        }
    }
}