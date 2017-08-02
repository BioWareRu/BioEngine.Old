using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BioEngine.Common.Base;
using BioEngine.Common.Interfaces;
using BioEngine.Common.Models;
using BioEngine.Routing.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BioEngine.Routing.Gallery
{
    public class GalleryUrlManager : UrlManager<GalleryRoutesEnum>
    {
        public GalleryUrlManager(IUrlHelper urlHelper, IOptions<AppSettings> appSettings) : base(urlHelper, appSettings)
        {
        }

        public string PublicUrl(GalleryPic picture, bool absolute = false)
        {
            var page = (int) Math.Ceiling((double) picture.Position / GalleryCat.PicsOnPage);
            return CatPublicUrl(picture.Cat, page, absolute);
        }

        public string CatPublicUrl(GalleryCat cat, int page = 1, bool absolute = false)
        {
            var url = CatUrl(cat);
            if (page > 1)
                url += $"/page/{page}";
            return GetUrl(GalleryRoutesEnum.CatPage,
                new {parentUrl = cat.Parent.ParentUrl, url}, absolute);
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

        public string ParentGalleryUrl(IChildModel childModel, bool absolute = false)
        {
            return ParentGalleryUrl(childModel.Parent, absolute);
        }

        public string ParentGalleryUrl(IParentModel parentModel, bool absolute = false)
        {
            return GetUrl(GalleryRoutesEnum.ParentPage, new {parentUrl = parentModel.ParentUrl}, absolute);
        }

        public string ThumbUrl(GalleryPic picture, int width = 100, int height = 0, int index = 0)
        {
            var file = picture.Files[index];
            var fileName = Path.GetFileNameWithoutExtension(file.Name);
            var ext = Path.GetExtension(file.Name);
            return
                $"{Settings.ImagesDomain}{picture.Parent.ParentUrl}/{CatUrl(picture.Cat)}/{fileName}.{width}x{height}{ext}";
        }

        public string FullUrl(GalleryPic picture, int index = 0)
        {
            var file = picture.Files[index];
            return
                $"{Settings.ImagesDomain}{picture.Parent.ParentUrl}/{CatUrl(picture.Cat)}/{file.Name}";
        }
    }
}