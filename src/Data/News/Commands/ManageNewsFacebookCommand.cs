using BioEngine.Data.Core;
using BioEngine.Social;

namespace BioEngine.Data.News.Commands
{
    public class ManageNewsFacebookCommand : QueryBase<bool>
    {
        public ManageNewsFacebookCommand(Common.Models.News news, FacebookOperationEnum operation)
        {
            News = news;
            Operation = operation;
        }

        public Common.Models.News News { get; }
        public FacebookOperationEnum Operation { get; }
    }
}