using System.Collections.Generic;
using System.Linq;
using BioEngine.Common.Models;

namespace BioEngine.Site.ViewModels
{
    public abstract class BaseViewModel
    {
        private readonly IEnumerable<Settings> _settings;

        protected BaseViewModel(IEnumerable<Settings> settings)
        {
            _settings = settings;
        }

        public string SiteTitle { get; set; }

        public string Title { get; set; }

        public string ImageUrl { get; protected set; }

        public string Description { get; protected set; }

        public List<BreadCrumbsItem> BreadCrumbs { get; private set; } = new List<BreadCrumbsItem>();

        public bool IsDevelopment { get; set; }

        public string ThemeName { get; set; } = "default";

        public string GetSettingValue(string settingName)
        {
            return _settings.FirstOrDefault(x => x.Name == settingName)?.Value;
        }
    }

    public struct BreadCrumbsItem
    {
        public string Title;
        public string Url;
    }
}