using System.Collections.Generic;
using System.Linq;
using BioEngine.Common.DB;
using BioEngine.Common.Models;
using BioEngine.Site.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace BioEngine.Site.Components.Url
{
    public class FileUrlManager : EntityUrlManager
    {
        public FileUrlManager(AppSettings settings, BWContext dbContext, IUrlHelper urlHelper)
            : base(settings, dbContext, urlHelper)
        {
        }

        public string PublicUrl(File file)
        {
            Poppulate(file);
            var url = CatUrl(file.Cat) + "/" + file.Url;
            return GetUrl("Show", "Files",
                new {parentUrl = ParentUrl(file), url});
            /*return _urlHelper.Action<FilesController>(x => x.Show(ParentUrl(file), CatUrl(file), file.Url));*/
        }

        public string DownloadUrl(File file)
        {
            Poppulate(file);
            var url = CatUrl(file.Cat) + "/" + file.Url;
            return GetUrl("Download", "Files",
                new {parentUrl = ParentUrl(file), url});
            /*return _urlHelper.Action<FilesController>(x => x.Show(ParentUrl(file), CatUrl(file), file.Url));*/
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

        private void Poppulate(File file)
        {
            if (file.Cat == null)
                DbContext.Entry(file).Reference(x => x.Cat).Load();
            var cat = file.Cat;
            while (cat != null)
            {
                if (cat.ParentCat == null)
                    if (cat.Pid > 0)
                        DbContext.Entry(cat).Reference(x => x.ParentCat).Load();
                    else
                        break;
                cat = cat.ParentCat;
            }

            if (file.GameId > 0 && file.Game == null)
                DbContext.Entry(file).Reference(x => x.Game).Load();
            if (file.DeveloperId > 0 && file.Developer == null)
                DbContext.Entry(file).Reference(x => x.Developer).Load();
        }

        public string ParentFilesUrl(Developer developer)
        {
            return UrlHelper.Action<FilesController>(x => x.ParentFiles(developer.Url));
        }

        public string ParentFilesUrl(Game game)
        {
            return UrlHelper.Action<FilesController>(x => x.ParentFiles(game.Url));
        }

        public string ParentFilesUrl(Topic topic)
        {
            return UrlHelper.Action<FilesController>(x => x.ParentFiles(topic.Url));
        }

        public string CatPublicUrl(FileCat cat, int page = 1)
        {
            Poppulate(cat);
            var url = CatUrl(cat) + "/" + cat.Url;
            if (page > 1)
                url += $"/page/{page}";
            return GetUrl("Show", "Files",
                new {parentUrl = ParentUrl(cat), url});
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

        private void Poppulate(FileCat cat)
        {
            while (cat != null)
            {
                if (cat.ParentCat == null)
                    if (cat.Pid > 0)
                        DbContext.Entry(cat).Reference(x => x.ParentCat).Load();
                    else
                        break;
                cat = cat.ParentCat;
            }
        }
    }
}