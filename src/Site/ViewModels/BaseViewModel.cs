using BioEngine.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BioEngine.Site.ViewModels
{
    public abstract class BaseViewModel
    {
        public BaseViewModel(IEnumerable<Settings> settings)
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

        private IEnumerable<Settings> _settings;

        public string GetSettingValue(string settingName)
        {
            return _settings.Where(x => x.Name == settingName).FirstOrDefault()?.Value;
        }
    }

    public struct BreadCrumbsItem
    {
        public string Title;
        public string Url;
    }
}