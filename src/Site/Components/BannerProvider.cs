using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BioEngine.Common.DB;
using BioEngine.Common.Models;
using BioEngine.Site.Extensions;
using JetBrains.Annotations;

namespace BioEngine.Site.Components
{
    [UsedImplicitly]
    public class BannerProvider
    {
        //private BWContext DbContext;

        private readonly Stack<Advertisement> _banners;

        public BannerProvider(BWContext dbContext)
        {
            var currentTs = DateTimeOffset.Now.ToUnixTimeSeconds();
            var banners =
                dbContext.Advertiesements.Where(
                    x =>
                        (x.AdActive == 1) && (x.AdLocation == "ad_sidebar") && (x.AdStart < currentTs) &&
                        ((x.AdEnd == 0) || (x.AdEnd > currentTs))).ToList();
            banners.Shuffle();
            _banners = new Stack<Advertisement>(banners);
            //Banners.Shuffle();
        }

        public async Task<Advertisement> Next()
        {
            return await Task.Run(() => _banners.Count > 0 ? _banners.Pop() : null);
        }
    }
}