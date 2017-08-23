using System.Threading.Tasks;

namespace BioEngine.Common.Interfaces
{
    public interface IContentHelperInterface
    {
        Task<string> ReplacePlaceholdersAsync(string text);
        string StripTags(string html);
    }
}