using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BioEngine.Common.Base;
using BioEngine.Common.DB;
using BioEngine.Common.Interfaces;
using BioEngine.Common.Models;
using BioEngine.Site.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BioEngine.Site.Components.Url
{
    public class GalleryUrlManager : EntityUrlManager
    {
        public GalleryUrlManager(AppSettings settings, BWContext dbContext, IUrlHelper urlHelper,
            ParentEntityProvider parentEntityProvider)
            : base(settings, dbContext, urlHelper, parentEntityProvider)
        {
        }

        public async Task<string> PublicUrl(GalleryPic picture)
        {
            await Poppulate(picture);
            var position = await GetPicPosition(picture);
            var page = (int) Math.Ceiling((double) position / GalleryCat.PicsOnPage);
            return await CatPublicUrl(picture.Cat, page);
        }

        public async Task<string> CatPublicUrl(GalleryCat cat, int page = 1)
        {
            await Poppulate(cat);
            var url = CatUrl(cat);
            if (page > 1)
                url += $"/page/{page}";
            return GetUrl("Cat", "Gallery",
                new {parentUrl = await ParentUrl(cat), url});
        }

        private async Task Poppulate(GalleryCat cat)
        {
            while (cat != null)
            {
                if (cat.ParentCat == null)
                    if (cat.Pid > 0)
                        await DbContext.Entry(cat).Reference(x => x.ParentCat).LoadAsync();
                    else
                        break;
                cat = cat.ParentCat;
            }
        }

        protected string CatUrl(GalleryCat cat)
        {
            var urls = new SortedList<int, string>();
            var i = 0;
            while (cat != null)
            {
                urls.Add(i, cat.Url);
                i++;
                cat = cat.ParentCat;
            }
            return string.Join("/", urls.Reverse().Select(x => x.Value).ToArray());
        }

        private async Task<int> GetPicPosition(GalleryPic picture)
        {
            return
                await DbContext.GalleryPics.Where(x => x.CatId == picture.CatId && x.Pub == 1 && x.Id > picture.Id)
                    .OrderByDescending(x => x.Id)
                    .CountAsync();
        }

        private async Task Poppulate(GalleryPic picture)
        {
            if (picture.Cat == null)
                await DbContext.Entry(picture).Reference(x => x.Cat).LoadAsync();
            var cat = picture.Cat;
            while (cat != null)
            {
                if (cat.ParentCat == null)
                    if (cat.Pid > 0)
                        await DbContext.Entry(cat).Reference(x => x.ParentCat).LoadAsync();
                    else
                        break;
                cat = cat.ParentCat;
            }

            if (picture.GameId > 0 && picture.Game == null)
                await DbContext.Entry(picture).Reference(x => x.Game).LoadAsync();
            if (picture.DeveloperId > 0 && picture.Developer == null)
                await DbContext.Entry(picture).Reference(x => x.Developer).LoadAsync();
        }

        public async Task<string> ParentGalleryUrl(GalleryCat galleryCat)
        {
            var parent = await ParentEntityProvider.GetModelParent(galleryCat);
            return ParentGalleryUrl((dynamic) parent);
        }

        public async Task<string> ParentGalleryUrl<T>(T parentModel) where T : IParentModel
        {
            return await Task.FromResult(
                UrlHelper.Action<GalleryController>(x => x.ParentGallery(parentModel.ParentUrl)));
        }

        public async Task<string> ThumbUrl(GalleryPic picture, int weight = 100, int height = 0, int index = 0)
        {
            await Poppulate(picture);
            return
                $"{Settings.SiteDomain}/gallery/thumb/{picture.Id}/{weight}/{height}";
        }

        public async Task<string> FullUrl(GalleryPic picture, int index = 0)
        {
            await Poppulate(picture);
            var file = picture.Files[index];
            return
                $"{Settings.ImagesDomain}/{await ParentUrl(picture)}/{CatUrl(picture.Cat)}/{file.Name}";
        }
    }
}