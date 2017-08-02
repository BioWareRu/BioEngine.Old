using System.Collections.Generic;
using System.Linq;
using BioEngine.Common.Base;
using BioEngine.Common.Interfaces;
using BioEngine.Common.Models;
using BioEngine.Routing.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BioEngine.Routing.Files
{
    public class FilesUrlManager : UrlManager<FilesRoutesEnum>
    {
        public FilesUrlManager(IUrlHelper urlHelper, IOptions<AppSettings> appSettings) : base(urlHelper, appSettings)
        {
        }

        public string PublicUrl(File file, bool absolute = false)
        {
            var url = CatUrl(file.Cat) + "/" + file.Url;
            return GetUrl(FilesRoutesEnum.FilePage, new {parentUrl = file.Parent.ParentUrl, url}, absolute);
        }

        public string DownloadUrl(File file)
        {
            var url = CatUrl(file.Cat) + "/" + file.Url;
            return GetUrl(FilesRoutesEnum.FileDownloadPage, new {parentUrl = file.Parent.ParentUrl, url});
        }

        public string CatUrl(File file)
        {
            var urls = new SortedList<int, string>();
            var cat = file.Cat;
            var i = 0;
            while (cat != null)
            {
                urls.Add(i, cat.Url);
                i++;
                cat = cat.ParentCat;
            }
            return string.Join("/", urls.Reverse().Select(x => x.Value).ToArray());
        }

        public string ParentFilesUrl(IChildModel childModel, bool absolute = false)
        {
            return ParentFilesUrl(childModel.Parent, absolute);
        }

        public string ParentFilesUrl(IParentModel parent, bool absolute = false)
        {
            return GetUrl(FilesRoutesEnum.FilesByParent, new {parentUrl = parent.ParentUrl}, absolute);
        }

        public string CatPublicUrl(FileCat cat, int page = 1)
        {
            var url = CatUrl(cat) + "/" + cat.Url;
            if (page > 1)
                url += $"/page/{page}";
            return GetUrl(FilesRoutesEnum.FilePage, new {parentUrl = cat.Parent.ParentUrl, url});
        }

        private static string CatUrl(FileCat cat)
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
    }
}