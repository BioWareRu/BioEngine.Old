using AutoMapper;
using BioEngine.Common.Models;
using BioEngine.Search.Models;

namespace BioEngine.Search
{
    public class SearchMapperProfile : Profile
    {
        public SearchMapperProfile()
        {
            CreateMap<NewsSearchModel, News>();
            CreateMap<News, NewsSearchModel>();
            CreateMap<GameSearchModel, Game>();
            CreateMap<Game, GameSearchModel>();
            CreateMap<ArticleSearchModel, Article>();
            CreateMap<Article, ArticleSearchModel>();
            CreateMap<ArticleCatSearchModel, ArticleCat>();
            CreateMap<ArticleCat, ArticleCatSearchModel>();
            CreateMap<FileSearchModel, File>();
            CreateMap<File, FileSearchModel>();
            CreateMap<FileCatSearchModel, FileCat>();
            CreateMap<FileCat, FileCatSearchModel>();
            CreateMap<GalleryCatSearchModel, GalleryCat>();
            CreateMap<GalleryCat, GalleryCatSearchModel>();
        }
    }
}