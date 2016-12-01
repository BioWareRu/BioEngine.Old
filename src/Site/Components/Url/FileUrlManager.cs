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
            return GetUrl("Show", "Files",
                new {parentUrl = ParentUrl(file), catUrl = CatUrl(file), fileUrl = file.Url});
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
                _dbContext.Entry(file).Reference(x => x.Cat).Load();
            var cat = file.Cat;
            while (cat != null)
            {
                if (cat.ParentCat == null)
                    if (cat.Pid > 0)
                        _dbContext.Entry(cat).Reference(x => x.ParentCat).Load();
                    else
                        break;
                cat = cat.ParentCat;
            }

            if ((file.GameId > 0) && (file.Game == null))
                _dbContext.Entry(file).Reference(x => x.Game).Load();
            if ((file.DeveloperId > 0) && (file.Developer == null))
                _dbContext.Entry(file).Reference(x => x.Developer).Load();
        }
    }
}