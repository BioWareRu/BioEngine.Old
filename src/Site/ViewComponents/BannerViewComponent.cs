using System.Threading.Tasks;
using BioEngine.Common.Base;
using BioEngine.Common.Models;
using BioEngine.Site.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BioEngine.Site.ViewComponents
{
    public class BannerViewComponent : ViewComponent
    {
        private readonly BannerProvider _provider;
        private readonly AppSettings _settings;

        public BannerViewComponent(BannerProvider provider, IOptions<AppSettings> settings)
        {
            _provider = provider;
            _settings = settings.Value;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var banner = await _provider.Next();
            if (banner != null)
                return View(new BannerViewModel(banner, _settings));
            return Content(string.Empty);
        }
    }

    public class BannerViewModel
    {
        private readonly Advertisement _banner;
        private readonly AppSettings _settings;

        public BannerViewModel(Advertisement banner, AppSettings settings)
        {
            _banner = banner;
            _settings = settings;
        }

        public string ImageUrl => _settings.IPBUploadsDomain + _banner.Images.Large;

        public string AdLink => _banner.AdLink;
    }
}