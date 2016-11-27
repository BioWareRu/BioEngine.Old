using BioEngine.Common.Models;
using BioEngine.Site.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace BioEngine.Site.ViewComponents
{
    public class BannerViewComponent : ViewComponent
    {
        private BannerManager Manager;
        private AppSettings Settings;

        public BannerViewComponent(BannerManager manager, IOptions<AppSettings> settings)
        {
            Manager = manager;
            Settings = settings.Value;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var banner = await Manager.Next();
            if (banner != null)
            {
                return View(new BannerViewModel(banner, Settings));
            }
            else
            {
                return Content(string.Empty);
            }
        }
    }

    public class BannerViewModel
    {
        private Advertisement Banner;
        private AppSettings Settings;

        public BannerViewModel(Advertisement banner, AppSettings settings)
        {
            Banner = banner;
            Settings = settings;
        }

        public string ImageUrl
        {
            get
            {
                return Settings.IPBUploadsDomain + Banner.Images.Large;
            }
        }

        public string AdLink { get { return Banner.AdLink; } }
    }
}
