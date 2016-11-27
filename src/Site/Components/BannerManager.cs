using BioEngine.Common.DB;
using BioEngine.Common.Models;
using BioEngine.Site.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BioEngine.Site.Components
{
    public class BannerManager
    {
        //private BWContext DbContext;

        private Stack<Advertisement> Banners;

        public BannerManager(BWContext dbContext)
        {
            var currentTs = DateTimeOffset.Now.ToUnixTimeSeconds();
            var banners = dbContext.Advertiesements.Where(x => x.AdActive == 1 && x.AdLocation == "ad_sidebar" && x.AdStart < currentTs && (x.AdEnd == 0 || x.AdEnd > currentTs)).ToList();
            banners.Shuffle();
            Banners = new Stack<Advertisement>(banners);
            //Banners.Shuffle();
        }

        public async Task<Advertisement> Next()
        {
            return await Task.Run(() =>
            {
                if (Banners.Count > 0)
                {
                    return Banners.Pop();
                }
                return null;
            });

        }
    }
}
