namespace BioEngine.Social.Twitter
{
    public class TooLongTweetTextException : TwitterException
    {
        public TooLongTweetTextException() : base("Текст твита больше 140 символов")
        {
        }
    }
}