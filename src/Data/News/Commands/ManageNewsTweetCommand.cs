using BioEngine.Data.Core;
using BioEngine.Social;

namespace BioEngine.Data.News.Commands
{
    public class ManageNewsTweetCommand : QueryBase<bool>
    {
        public ManageNewsTweetCommand(Common.Models.News news, TwitterOperationEnum operation)
        {
            News = news;
            Operation = operation;
        }

        public Common.Models.News News { get; }
        public TwitterOperationEnum Operation { get; }
    }
}