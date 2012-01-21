using System;

namespace Spring.RestWindowsPhoneQuickStart.Twitter
{
    // See Spring.Social Twitter project: https://www.springframework.net/social-twitter/

    /// <summary>
    /// Represents a Twitter status update (e.g., a "tweet").
    /// </summary>
    public class Tweet
    {
        public long? ID { get; set; }
        public string Text { get; set; }
        public string CreatedAt { get; set; }
        public string FromUser { get; set; }
        public string ProfileImageUrl { get; set; }
        public long? ToUserId { get; set; }
        public long? FromUserId { get; set; }
        public string LanguageCode { get; set; }
        public string Source { get; set; }
    }
}
