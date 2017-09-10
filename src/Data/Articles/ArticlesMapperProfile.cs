using AutoMapper;
using BioEngine.Data.Articles.Commands;
using JetBrains.Annotations;

namespace BioEngine.Data.Articles
{
    [UsedImplicitly]
    internal class ArticlesMapperProfile : Profile
    {
        public ArticlesMapperProfile()
        {
            CreateMap<CreateArticleCommand, Common.Models.Article>();
            CreateMap<UpdateArticleCommand, Common.Models.Article>();
        }
    }
}