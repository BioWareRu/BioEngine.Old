using AutoMapper;
using BioEngine.Data.Articles.Commands;
using JetBrains.Annotations;

namespace BioEngine.API.Models.Articles
{
    public class ArticleFormModel
    {
        public int? GameId { get; set; }
        public int? DeveloperId { get; set; }
        public int? TopicId { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Source { get; set; }
        public int? CatId { get; set; }
        public string Announce { get; set; }
        public string Text { get; set; }
    }

    [UsedImplicitly]
    internal class ArticleFormModelMapperProfile : Profile
    {
        public ArticleFormModelMapperProfile()
        {
            CreateMap<ArticleFormModel, CreateArticleCommand>();
            CreateMap<ArticleFormModel, UpdateArticleCommand>();
        }
    }
}