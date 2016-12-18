using System;
using System.Collections.Generic;
using System.Linq;
using BioEngine.Common.Models;
using BioEngine.Site.Components;
using BioEngine.Site.Components.Url;

namespace BioEngine.Site.ViewModels
{
    public abstract class BaseViewModel
    {
        protected readonly AppSettings AppSettings;
        protected readonly IEnumerable<Settings> Settings;
        public readonly UrlManager UrlManager;

        protected BaseViewModel(BaseViewModelConfig config)
        {
            Settings = config.Settings;
            AppSettings = config.AppSettings;
            UrlManager = config.UrlManager;
            SiteTitle = Title = AppSettings.Title;
            ImageUrl = new Uri(AppSettings.SocialLogo);
        }

        public string SiteTitle { get; set; }

        public string Title { get; set; }

        public Uri ImageUrl { get; protected set; }

        public string Description { get; protected set; }

        public List<BreadCrumbsItem> BreadCrumbs { get; private set; } = new List<BreadCrumbsItem>();

        public string ThemeName { get; set; } = "default";

        public string GetSettingValue(string settingName)
        {
            return Settings.FirstOrDefault(x => x.Name == settingName)?.Value;
        }
    }

    public struct BaseViewModelConfig
    {
        public readonly UrlManager UrlManager;
        public readonly AppSettings AppSettings;
        public readonly List<Settings> Settings;

        public BaseViewModelConfig(UrlManager urlManager, AppSettings appSettings, List<Settings> settings)
        {
            UrlManager = urlManager;
            AppSettings = appSettings;
            Settings = settings;
        }
    }

    public struct BreadCrumbsItem
    {
        public string Url { get; }
        public string Title { get; }

        public BreadCrumbsItem(string url, string title)
        {
            Url = url;
            Title = title;
        }
    }
}