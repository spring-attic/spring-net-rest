#region License

/*
 * Copyright 2002-2011 the original author or authors.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#endregion

using System;
using System.Linq;
using System.Collections.Generic;

using Spring.Rest.Client;
using Spring.Http.Converters.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Spring.RestWindowsPhoneQuickStart.Twitter
{
    // From Spring.Social Java project: http://http://git.springsource.org/spring-social/

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
            restTemplate.MessageConverters.Add(new NJsonHttpMessageConverter());
        }

        public void GetPublicTimeline(Action<IList<Tweet>> operationCompleted)
        {
            restTemplate.GetForObjectAsync<JToken>(PUBLIC_TIMELINE_URL,
                r =>
                {
                    IList<Tweet> tweets = ExtractTimelineTweetsFromResponse(r.Response);
                    operationCompleted(tweets);
                });
        }

        public void GetUserTimeline(string screenName, Action<IList<Tweet>> operationCompleted)
        {
            restTemplate.GetForObjectAsync<JToken>(USER_TIMELINE_URL + "?screen_name={screenName}",
                r =>
                {
                    IList<Tweet> tweets = ExtractTimelineTweetsFromResponse(r.Response);
                    operationCompleted(tweets);
                }, screenName);
        }

        public void GetUserTimeline(long userId, Action<IList<Tweet>> operationCompleted)
        {
            restTemplate.GetForObjectAsync<JToken>(USER_TIMELINE_URL + "?user_id={userId}",
                r =>
                {
                    IList<Tweet> tweets = ExtractTimelineTweetsFromResponse(r.Response);
                    operationCompleted(tweets);
                }, userId);
        }

        public void Search(string query, Action<IList<Tweet>> operationCompleted)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("query", query);
            parameters.Add("rpp", 50);
            parameters.Add("page", 1);

            restTemplate.GetForObjectAsync<JToken>(SEARCH_URL, parameters,
               r =>
               {
                   IList<Tweet> tweets = new List<Tweet>();
                   foreach (JToken item in r.Response.Value<JArray>("results"))
                   {
                       tweets.Add(PopulateTweetFromSearchResults(item));
                   }
                   operationCompleted(tweets);
               });
        }


        private IList<Tweet> ExtractTimelineTweetsFromResponse(JToken response)
        {
            IList<Tweet> tweets = new List<Tweet>();
            foreach (JToken item in response.Value<JArray>())
            {
                tweets.Add(PopulateTweetFromTimelineItem(item));
            }
            return tweets;
        }

        private Tweet PopulateTweetFromTimelineItem(JToken item)
        {
            Tweet tweet = new Tweet();

            tweet.ID = item.Value<long?>("id");
            tweet.Text = item.Value<string>("text");
            tweet.FromUser = item.Value<JToken>("user").Value<string>("screen_name");
            tweet.FromUserId = item.Value<JToken>("user").Value<long?>("id");
            tweet.ProfileImageUrl = item.Value<JToken>("user").Value<string>("profile_image_url");
            tweet.Source = item.Value<string>("source");
            tweet.ToUserId = item.Value<long?>("in_reply_to_user_id");
            //tweet.CreatedAt = item.Value<DateTime?>("created_at");

            return tweet;
        }

        private Tweet PopulateTweetFromSearchResults(JToken item)
        {
            Tweet tweet = new Tweet();
            
            tweet.ID = item.Value<long?>("id");
            tweet.FromUser = item.Value<string>("from_user");
            tweet.Text = item.Value<string>("text");
            //tweet.CreatedAt = item.Value<DateTime?>("created_at");
            tweet.FromUserId = item.Value<long?>("from_user_id");
            tweet.ToUserId = item.Value<long?>("to_user_id");
            tweet.LanguageCode = item.Value<string>("iso_language_code");
            tweet.ProfileImageUrl = item.Value<string>("profile_image_url");
            tweet.Source = item.Value<string>("source");

            return tweet;
        }
    }
}