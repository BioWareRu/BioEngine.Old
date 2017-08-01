using System.Threading.Tasks;

namespace BioEngine.Common.Interfaces
{
    public interface IContentHelperInterface
    {
        Task<string> ReplacePlaceholders(string text);
    }
}