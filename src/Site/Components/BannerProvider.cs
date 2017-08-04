using System.Collections.Generic;
using System.Threading.Tasks;
using BioEngine.Common.Models;
using BioEngine.Data.Base.Requests;
using BioEngine.Site.Extensions;
using JetBrains.Annotations;
using MediatR;

namespace BioEngine.Site.Components
{
    [UsedImplicitly]
    public class BannerProvider
    {
        private readonly IMediator _mediator;

        private Stack<Advertisement> _banners;

        public BannerProvider(IMediator mediator)
        {
            _mediator = mediator;
        }

        private async Task<Stack<Advertisement>> GetBanners()
        {
            if (_banners != null) return _banners;

            var banners = new List<Advertisement>(await _mediator.Send(new GetBannersRequest()));
            banners.Shuffle();
            _banners = new Stack<Advertisement>(banners);
            return _banners;
        }

        public async Task<Advertisement> Next()
        {
            var banners = await GetBanners();
            return banners.Count > 0 ? banners.Pop() : null;
        }
    }
}