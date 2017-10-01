using JetBrains.Annotations;
using AutoMapper;
using BioEngine.Data.Files.Commands;

namespace BioEngine.API.Models.Files
{
    public class FileCatFormModel
    {
        public int? GameId { get; set; }
        public int? DeveloperId { get; set; }
        public int? Pid { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Descr { get; set; }
    }
    
    [UsedImplicitly]
    internal class ArticleFormModelMapperProfile : Profile
    {
        public ArticleFormModelMapperProfile()
        {
            CreateMap<FileCatFormModel, CreateFileCatCommand>();
        }
    }
}