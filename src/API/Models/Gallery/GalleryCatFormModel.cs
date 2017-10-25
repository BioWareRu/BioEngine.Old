using AutoMapper;
using BioEngine.Data.Gallery.Commands;
using JetBrains.Annotations;

namespace BioEngine.API.Models.Gallery
{
    public class GalleryCatFormModel
    {
        public int? GameId { get; set; }
        public int? DeveloperId { get; set; }
        public int? CatId { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Descr { get; set; }
    }
    
    [UsedImplicitly]
    internal class GalleryFormModelMapperProfile : Profile
    {
        public GalleryFormModelMapperProfile()
        {
            CreateMap<GalleryCatFormModel, CreateGalleryCatCommand>();
            CreateMap<GalleryCatFormModel, UpdateGalleryCatCommand>();
        }
    }
}