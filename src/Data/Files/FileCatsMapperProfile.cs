using AutoMapper;
using BioEngine.Data.Files.Commands;
using JetBrains.Annotations;

namespace BioEngine.Data.Files
{
    [UsedImplicitly]
    internal class FileCatsMapperProfile : Profile
    {
        public FileCatsMapperProfile()
        {
            CreateMap<CreateFileCatCommand, Common.Models.FileCat>();
        }
    }
}