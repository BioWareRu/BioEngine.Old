using AutoMapper;
using BioEngine.Data.News.Commands;
using JetBrains.Annotations;

namespace BioEngine.Data.News
{
    [UsedImplicitly]
    internal class NewsMapperProfile : Profile
    {
        public NewsMapperProfile()
        {
            CreateMap<CreateNewsCommand, Common.Models.News>();
        }
    }
}