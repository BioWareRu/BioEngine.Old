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

        public Uri PublicUrl(GalleryPic picture, bool absolute = false)
        {
            var page = (int) Math.Ceiling((double) picture.Position / GalleryCat.PicsOnPage);
            return CatPublicUrl(picture.Cat, page, absolute);
        }

        public Uri CatPublicUrl(GalleryCat cat, int page = 1, bool absolute = false)
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

        public Uri ParentGalleryUrl(IChildModel childModel, bool absolute = false)
        {
            return ParentGalleryUrl(childModel.Parent, absolute);
        }

        public Uri ParentGalleryUrl(IParentModel parentModel, bool absolute = false)
        {
            return GetUrl(GalleryRoutesEnum.ParentPage, new {parentUrl = parentModel.ParentUrl}, absolute);
        }

        public Uri ThumbUrl(GalleryPic picture, int width = 100, int height = 0, int index = 0)
        {
            var file = picture.Files[index];
            var fileName = Path.GetFileNameWithoutExtension(file.Name);
            var ext = Path.GetExtension(file.Name);
            return
                new Uri(
                    $"{Settings.ImagesDomain}{picture.Parent.ParentUrl}/{CatUrl(picture.Cat)}/{fileName}.{width}x{height}{ext}");
        }

        public Uri FullUrl(GalleryPic picture, int index = 0)
        {
            var file = picture.Files[index];
            return
                new Uri($"{Settings.ImagesDomain}{picture.Parent.ParentUrl}/{CatUrl(picture.Cat)}/{file.Name}");
        }
    }
}