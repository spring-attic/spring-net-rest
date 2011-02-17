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

namespace Spring.RestWindowsPhoneQuickStart.Twitter
{
    // From Spring.Social Java project: http://http://git.springsource.org/spring-social/

    //Represents a Twitter status update (e.g., a "tweet").
    public class Tweet
    {
        public long? ID { get; set; }
        public string Text { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string FromUser { get; set; }
        public string ProfileImageUrl { get; set; }
        public long? ToUserId { get; set; }
        public long? FromUserId { get; set; }
        public string LanguageCode { get; set; }
        public string Source { get; set; }
    }
}
