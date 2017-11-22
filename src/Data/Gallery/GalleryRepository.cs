using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BioEngine.Data.Base;
using BioEngine.Data.DB;
using BioEngine.Common.Interfaces;
using BioEngine.Common.Models;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace BioEngine.Data.Gallery
{
    [UsedImplicitly]
    public class GalleryRepository : CatBioRepository<GalleryCat, GalleryPic, int>
    {
        public GalleryRepository(BWContext context, ParentEntityProvider parentEntityProvider) : base(context,
            parentEntityProvider)
        {
        }

        public override async Task<GalleryPic> GetById(int id)
        {
            var pic =
                await GetBasePicQuery(new GalleryPicsListQueryOptions())
                    .Where(x => x.Id == id)
                    .FirstOrDefaultAsync();
            if (pic != null)
            {
                await ProccessPicAsync(pic);
            }

            return pic;
        }

        public override async Task<GalleryCat> GetCatById(int id)
        {
            var cat = await GetBaseCatQuery(new GalleryCatsListQueryOptions())
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
            if (cat != null)
            {
                await ProccessCatAsync(cat, new GalleryCatsListQueryOptions());
            }
            return cat;
        }

        public async Task<GalleryCat> GetCat(GalleryCatsListQueryOptions options)
        {
            var query = GetBaseCatQuery(options);

            var cat = await query.FirstOrDefaultAsync();
            if (cat != null)
            {
                await ProccessCatAsync(cat, options);
            }
            return cat;
        }

        public async Task<(IEnumerable<GalleryCat>, int)> GetCats(GalleryCatsListQueryOptions options)
        {
            var query = GetBaseCatQuery(options);

            var data = await GetDataAsync(query, options);
            foreach (var cat in data.models)
            {
                await ProccessCatAsync(cat, options);
            }

            return data;
        }

        public async Task<(IEnumerable<GalleryPic> models, int totalCount)> GetPics(GalleryPicsListQueryOptions options)
        {
            var query = GetBasePicQuery(options);

            if (options.Parent != null)
            {
                query = ApplyParentCondition(query, options.Parent);
            }
            if (options.Cat != null)
            {
                query = query.Where(x => x.CatId == options.Cat.Id);
            }

            var data = await GetDataAsync(query, options);

            foreach (var pic in data.models)
            {
                await ProccessPicAsync(pic, options.LoadPicPosition);
            }

            return data;
        }

        protected override async Task<IEnumerable<GalleryPic>> GetCatItemsAsync(GalleryCat cat,
            CatsListQueryOptions<GalleryCat> options)
        {
            return (await GetPics(
                    new GalleryPicsListQueryOptions()
                    {
                        Cat = cat,
                        Page = 1,
                        PageSize = options.LoadLastItems ?? options.PageSize
                    }))
                .models;
        }

        private async Task<int> GetPicPositionAsync(GalleryPic picture)
        {
            return
                await Context.GalleryPics.Where(x => x.CatId == picture.CatId && x.Pub == 1 && x.Id >= picture.Id)
                    .CountAsync();
        }

        private IQueryable<GalleryCat> GetBaseCatQuery(GalleryCatsListQueryOptions options)
        {
            var catQuery = Context.GalleryCats
                .Include(x => x.Game)
                .Include(x => x.Developer)
                .Include(x => x.ParentCat).AsQueryable();

            if (!string.IsNullOrEmpty(options.Url))
            {
                catQuery = catQuery.Where(x => x.Url == options.Url);
            }

            if (options.Parent != null)
            {
                catQuery = ApplyParentCondition(catQuery, options.Parent);
            }

            if (options.ParentCat != null)
            {
                catQuery = catQuery.Where(x => x.CatId == options.ParentCat.Id);
            }
            else if (options.OnlyRoot)
            {
                catQuery = catQuery.Where(x => x.CatId == null);
            }

            return catQuery;
        }

        private async Task<bool> ProccessPicAsync(GalleryPic pic, bool loadPosition = false)
        {
            if (pic.Cat != null)
            {
                await ProccessCatAsync(pic.Cat, new GalleryCatsListQueryOptions());

                if (loadPosition)
                {
                    pic.Position = await GetPicPositionAsync(pic);
                }
            }

            return true;
        }

        private IQueryable<GalleryPic> GetBasePicQuery(GalleryPicsListQueryOptions options)
        {
            var query = Context.GalleryPics.Include(x => x.Game)
                .Include(x => x.Developer)
                .Include(x => x.Cat).AsQueryable();
            if (!options.WithUnPublishedPictures)
                query = query.Where(x => x.Pub == 1);

            return query;
        }
    }

    public class GalleryPicsListQueryOptions : ModelsListQueryOptions<GalleryPic>
    {
        public bool WithUnPublishedPictures { get; set; }
        public IParentModel Parent { get; set; }
        public GalleryCat Cat { get; set; }
        public bool LoadPicPosition { get; set; }
    }

    public class GalleryCatsListQueryOptions : CatsListQueryOptions<GalleryCat>
    {
    }
}