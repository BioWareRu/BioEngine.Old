using System.Collections.Generic;
using System.Threading.Tasks;

namespace BioEngine.Site.ViewModels.Search
{
    public class SearchViewModel : BaseViewModel
    {
        public string Query { get; }

        public SearchViewModel(BaseViewModelConfig config, string query) : base(config)
        {
            Query = query;
        }

        public readonly List<SearchBlock> Blocks = new List<SearchBlock>();

        public void AddBlock(SearchBlock block)
        {
            Blocks.Add(block);
        }

        public override Task<string> Title()
        {
            return Task.FromResult($"Поиск - {Query}");
        }
    }

    public class SearchBlock
    {
        public string Title { get; }

        public readonly List<SearchBlockItem> Items = new List<SearchBlockItem>();

        public int Count { get; private set; }
        public long TotalCount { get; }

        public string Url { get; }

        public SearchBlock(string title, string url, long totalCount)
        {
            Title = title;
            Url = url;
            TotalCount = totalCount;
        }

        public void AddItem(string title, string url, string text)
        {
            Items.Add(new SearchBlockItem(title, url, text));
            Count++;
        }
    }

    public struct SearchBlockItem
    {
        public string Title { get; }
        public string Url { get; }
        public string Text { get; }

        public SearchBlockItem(string title, string url, string text)
        {
            Title = title;
            Text = !string.IsNullOrEmpty(text) && text.Length > 250 ? text.Substring(0, 250) : text;
            Url = url;
        }
    }
}