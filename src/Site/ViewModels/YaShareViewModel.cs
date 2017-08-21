using System;

namespace BioEngine.Site.ViewModels
{
    public class YaShareViewModel
    {
        public YaShareViewModel(Uri url, string description = null, Uri image = null, string title = null)
        {
            Url = url;
            Description = description;
            Image = image;
            Title = title;
        }

        public Uri Url { get; }
        public string Description { get; }
        public Uri Image { get; }
        public string Title { get; }
    }
}