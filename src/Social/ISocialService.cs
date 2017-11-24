using System.Threading.Tasks;
using BioEngine.Common.Models;

namespace BioEngine.Social
{
    public interface ISocialService
    {
        Task<bool> PublishNews(News news, bool forceUpdate = false);
        Task<bool> DeleteNews(News news);
    }
}