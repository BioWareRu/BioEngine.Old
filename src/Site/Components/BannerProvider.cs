using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BioEngine.Common.DB;
using BioEngine.Common.Models;
using BioEngine.Site.Extensions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace BioEngine.Site.Components
{
    [UsedImplicitly]
    public class BannerProvider
    {
        private readonly BWContext _dbContext;
        //private BWContext DbContext;

        private Stack<Advertisement> _banners;

        public BannerProvider(BWContext dbContext)
        {
            _dbContext = dbContext;
        }

        private async Task<Stack<Advertisement>> GetBanners()
        {
            if (_banners != null) return _banners;

            var currentTs = DateTimeOffset.Now.ToUnixTimeSeconds();
            var banners =
                await _dbContext.Advertiesements.Where(
                        x =>
                            (x.AdActive == 1) && (x.AdLocation == "ad_sidebar") && (x.AdStart < currentTs) &&
                            ((x.AdEnd == 0) || (x.AdEnd > currentTs)))
                    .ToListAsync();
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