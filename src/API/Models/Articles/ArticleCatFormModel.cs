using AutoMapper;
using BioEngine.Data.Articles.Commands;
using JetBrains.Annotations;

namespace BioEngine.API.Models.Articles
{
    public class ArticleCatFormModel
    {
        public int? GameId { get; set; }
        public int? DeveloperId { get; set; }
        public int? Pid { get; set; }
        public int? TopicId { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Descr { get; set; }
        [CanBeNull]
        public string Content { get; set; }
        public int? Articles { get; set; }
    }
    
    [UsedImplicitly]
    internal class ArticleCatFormModelMapperProfile : Profile
    {
        public ArticleCatFormModelMapperProfile()
        {
            CreateMap<ArticleCatFormModel, CreateArticleCatCommand>();
            CreateMap<ArticleCatFormModel, UpdateArticleCatCommand>();
        }
    }
}