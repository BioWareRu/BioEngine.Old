using AutoMapper;
using BioEngine.Data.Gallery.Commands;
using BioEngine.Data.Gallery.Queries;
using JetBrains.Annotations;

namespace BioEngine.Data.Gallery
{
    [UsedImplicitly]
    internal class GalleryCatsMapperProfile : Profile
    {
        public GalleryCatsMapperProfile()
        {
            CreateMap<CreateGalleryCatCommand, Common.Models.GalleryCat>();
            CreateMap<UpdateGalleryCatCommand, Common.Models.GalleryCat>();
            CreateMap<GetGalleryPicsQuery, GalleryPicsListQueryOptions>();
            CreateMap<GetGalleryCategoryQuery, GalleryCatsListQueryOptions>();
            CreateMap<GetGalleryCategoriesQuery, GalleryCatsListQueryOptions>();
        }
    }
}