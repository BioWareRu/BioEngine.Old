using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BioEngine.Common.Base;
using BioEngine.Common.Interfaces;
using BioEngine.Common.Models;
using HtmlAgilityPack;

namespace BioEngine.Site.ViewModels
{
    public abstract class BaseViewModel
    {
        protected readonly AppSettings AppSettings;
        protected readonly IEnumerable<Settings> Settings;
        protected readonly IContentHelperInterface ContentHelper;

        protected BaseViewModel(BaseViewModelConfig config)
        {
            Settings = config.Settings;
            AppSettings = config.AppSettings;
            ContentHelper = config.ContentHelper;
            SiteTitle = AppSettings.Title;
            ImageUrl = new Uri(AppSettings.SocialLogo);
        }

        public string SiteTitle { get; set; }

        //public string Title { get; set; }
        public abstract string Title();

        protected Uri ImageUrl { get; set; }

        public List<BreadCrumbsItem> BreadCrumbs { get; private set; } = new List<BreadCrumbsItem>();

        public string ThemeName { get; set; } = "default";

        public string GetSettingValue(string settingName)
        {
            return Settings.FirstOrDefault(x => x.Name == settingName)?.Value;
        }

        protected abstract Task<string> GetDescription();

        public async Task<string> GetPageDescription()
        {
            var description = await GetDescription();
            if (!string.IsNullOrEmpty(description))
            {
                description = await ContentHelper.ReplacePlaceholdersAsync(description);
            }

            return ContentHelper.StripTags(description);
        }

        public virtual Uri GetImageUrl()
        {
            return ImageUrl;
        }

        public void SetImageUrl(Uri imageUrl)
        {
            ImageUrl = imageUrl;
        }

        public static string GetDescriptionFromHtml(string html)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            string description = null;

            foreach (var childNode in htmlDoc.DocumentNode.ChildNodes.Where(x => x.Name == "p" || x.Name == "div"))
            {
                var childText = HtmlEntity.DeEntitize(childNode.InnerText.Trim('\r', '\n')).Trim();
                if (!string.IsNullOrWhiteSpace(childText))
                {
                    description = childText;
                    break;
                }
            }

            return description;
        }

        public Uri GetImageFromHtml(string html)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var contentBlock = htmlDoc.DocumentNode.Descendants("img").FirstOrDefault();
            var url = contentBlock?.GetAttributeValue("src", "");
            Uri uri = null;
            if (!string.IsNullOrEmpty(url))
            {
                var parsed = Uri.TryCreate(url, UriKind.Absolute, out uri);
                if (!parsed)
                {
                    Uri.TryCreate(AppSettings.SiteDomain + url, UriKind.Absolute, out uri);
                }
            }
            return uri;
        }
    }

    public struct BaseViewModelConfig
    {
        public IContentHelperInterface ContentHelper { get; }
        public readonly AppSettings AppSettings;
        public readonly IEnumerable<Settings> Settings;

        public BaseViewModelConfig(AppSettings appSettings, IEnumerable<Settings> settings,
            IContentHelperInterface contentHelper)
        {
            ContentHelper = contentHelper;
            AppSettings = appSettings;
            Settings = settings;
        }
    }

    public struct BreadCrumbsItem
    {
        public Uri Url { get; }
        public string Title { get; }

        public BreadCrumbsItem(Uri url, string title)
        {
            Url = url;
            Title = title;
        }
    }
}