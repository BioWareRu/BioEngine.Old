using BioEngine.Common.Base;
using BioEngine.Common.Models;

namespace BioEngine.Search.Interfaces
{
    public interface ISearchModel
    {
    }


    public interface IParentSearchModel : ISearchModel
    {
    }

    public interface IChildSearchModel : ISearchModel
    {
        int? GameId { get; set; }

        int? DeveloperId { get; set; }

        int? TopicId { get; set; }

        Game Game { get; set; }

        Developer Developer { get; set; }

        Topic Topic { get; set; }
    }
}