using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BioEngine.Common.Base;
using BioEngine.Common.DB;
using BioEngine.Common.Models;
using BioEngine.Site.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace BioEngine.Site.Components.Url
{
    public class FileUrlManager : EntityUrlManager
    {
        public FileUrlManager(AppSettings settings, BWContext dbContext, IUrlHelper urlHelper,
            ParentEntityProvider parentEntityProvider)
            : base(settings, dbContext, urlHelper, parentEntityProvider)
        {
        }

        public async Task<string> PublicUrl(File file)
        {
            await Poppulate(file);
            var url = CatUrl(file.Cat) + "/" + file.Url;
            return GetUrl("Show", "Files",
                new {parentUrl = await ParentUrl(file), url});
            /*return _urlHelper.Action<FilesController>(x => x.Show(ParentUrl(file), CatUrl(file), file.Url));*/
        }

        public async Task<string> DownloadUrl(File file)
        {
            await Poppulate(file);
            var url = CatUrl(file.Cat) + "/" + file.Url;
            return GetUrl("Download", "Files",
                new {parentUrl = await ParentUrl(file), url});
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

        private async Task Poppulate(File file)
        {
            if (file.Cat == null)
                await DbContext.Entry(file).Reference(x => x.Cat).LoadAsync();
            var cat = file.Cat;
            while (cat != null)
            {
                if (cat.ParentCat == null)
                    if (cat.Pid > 0)
                        await DbContext.Entry(cat).Reference(x => x.ParentCat).LoadAsync();
                    else
                        break;
                cat = cat.ParentCat;
            }

            if (file.GameId > 0 && file.Game == null)
                await DbContext.Entry(file).Reference(x => x.Game).LoadAsync();
            if (file.DeveloperId > 0 && file.Developer == null)
                await DbContext.Entry(file).Reference(x => x.Developer).LoadAsync();
        }

        public async Task<string> ParentFilesUrl(FileCat fileCat)
        {
            var parent = await ParentEntityProvider.GetModelParent(fileCat);
            return ParentFilesUrl((dynamic)parent);
        }

        public async Task<string> ParentFilesUrl(File file)
        {
            var parent = await ParentEntityProvider.GetModelParent(file);
            return ParentFilesUrl((dynamic)parent);
        }

        public async Task<string> ParentFilesUrl<T>(T parentModel) where T : ParentModel
        {
            return await Task.FromResult(UrlHelper.Action<FilesController>(x => x.ParentFiles(parentModel.ParentUrl)));
        }

        public async Task<string> CatPublicUrl(FileCat cat, int page = 1)
        {
            await Poppulate(cat);
            var url = CatUrl(cat) + "/" + cat.Url;
            if (page > 1)
                url += $"/page/{page}";
            return GetUrl("Show", "Files",
                new {parentUrl = await ParentUrl(cat), url});
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

        private async Task Poppulate(FileCat cat)
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
    }
}