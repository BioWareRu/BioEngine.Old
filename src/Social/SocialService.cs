using System.Threading.Tasks;
using BioEngine.Common.Models;

namespace BioEngine.Social
{
    public interface SocialServiceInterface
    {
        Task<bool> PublishNews(News news, bool forceUpdate = false);
        Task<bool> DeleteNews(News news);
    }

    public enum SocialOperationEnum
    {
        CreateOrUpdate,
        Delete
    }
}