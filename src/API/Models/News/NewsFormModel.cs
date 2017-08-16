using AutoMapper;
using BioEngine.Data.News.Commands;
using JetBrains.Annotations;

namespace BioEngine.API.Models.News
{
    public class NewsFormModel
    {
        public int? GameId { get; set; }
        public int? DeveloperId { get; set; }
        public int? TopicId { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Source { get; set; }
        public string ShortText { get; set; }
        public string AddText { get; set; }
    }

    [UsedImplicitly]
    internal class NewsFromModelMapperProfile : Profile
    {
        public NewsFromModelMapperProfile()
        {
            CreateMap<NewsFormModel, CreateNewsCommand>();
            CreateMap<NewsFormModel, UpdateNewsCommand>();
        }
    }
}