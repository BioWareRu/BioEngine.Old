using AutoMapper;
using BioEngine.Data.Gallery.Commands;
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
        }
    }
}