﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BioEngine.Common.DB;
using BioEngine.Common.Models;
using BioEngine.Site.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace BioEngine.Site.Components.Url
{
    public class GalleryUrlManager : EntityUrlManager
    {
        public GalleryUrlManager(AppSettings settings, BWContext dbContext, IUrlHelper urlHelper)
            : base(settings, dbContext, urlHelper)
        {
        }

        public string PublicUrl(GalleryPic picture)
        {
            Poppulate(picture);
            var position = GetPicPosition(picture);
            var page = (int) Math.Ceiling((double) position / GalleryCat.PicsOnPage);
            return CatPublicUrl(picture.Cat, page);
        }

        public string CatPublicUrl(GalleryCat cat, int page = 1)
        {
            Poppulate(cat);
            var url = CatUrl(cat);
            if (page > 1)
            {
                url += $"/page/{page}";
            }
            return GetUrl("Cat", "Gallery",
                new {parentUrl = ParentUrl(cat), url});
        }

        private void Poppulate(GalleryCat cat)
        {
            while (cat != null)
            {
                if (cat.ParentCat == null)
                {
                    if (cat.Pid > 0)
                    {
                        DbContext.Entry(cat).Reference(x => x.ParentCat).Load();
                    }
                    else
                    {
                        break;
                    }
                }
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

        private int GetPicPosition(GalleryPic picture)
        {
            return
                DbContext.GalleryPics.Where(x => x.CatId == picture.CatId && x.Pub == 1 && x.Id > picture.Id)
                    .OrderByDescending(x => x.Id)
                    .Count();
        }

        private void Poppulate(GalleryPic picture)
        {
            if (picture.Cat == null)
            {
                DbContext.Entry(picture).Reference(x => x.Cat).Load();
            }
            var cat = picture.Cat;
            while (cat != null)
            {
                if (cat.ParentCat == null)
                {
                    if (cat.Pid > 0)
                    {
                        DbContext.Entry(cat).Reference(x => x.ParentCat).Load();
                    }
                    else
                    {
                        break;
                    }
                }
                cat = cat.ParentCat;
            }

            if (picture.GameId > 0 && picture.Game == null)
            {
                DbContext.Entry(picture).Reference(x => x.Game).Load();
            }
            if (picture.DeveloperId > 0 && picture.Developer == null)
            {
                DbContext.Entry(picture).Reference(x => x.Developer).Load();
            }
        }

        public string ParentGalleryUrl(Developer developer)
        {
            return UrlHelper.Action<GalleryController>(x => x.ParentGallery(developer.Url));
        }

        public string ParentGalleryUrl(Game game)
        {
            return UrlHelper.Action<GalleryController>(x => x.ParentGallery(game.Url));
        }

        public string ParentGalleryUrl(Topic topic)
        {
            return UrlHelper.Action<GalleryController>(x => x.ParentGallery(topic.Url));
        }

        public string ThumbUrl(GalleryPic picture, int weight = 100, int height = 0, int index = 0)
        {
            Poppulate(picture);
            return
                $"{Settings.SiteDomain}/gallery/thumb/{picture.Id}/{weight}/{height}";
        }

        public string FullUrl(GalleryPic picture, int index = 0)
        {
            Poppulate(picture);
            var file = picture.Files[index];
            return
                $"{Settings.ImagesDomain}/{ParentUrl(picture)}/{CatUrl(picture.Cat)}/{file.Name}";
        }
    }
}