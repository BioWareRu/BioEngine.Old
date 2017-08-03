using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BioEngine.Content.Helpers;

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

        public override string Title()
        {
            return $"Поиск - {Query}";
        }

        protected override async Task<string> GetDescription()
        {
            return await Task.FromResult($"Поиск по запросу {Query}");
        }
    }

    public class SearchBlock
    {
        public string Title { get; }

        public readonly List<SearchBlockItem> Items = new List<SearchBlockItem>();

        public int Count { get; private set; }
        public long TotalCount { get; }

        public Uri Url { get; }

        public SearchBlock(string title, Uri url, long totalCount)
        {
            Title = title;
            Url = url;
            TotalCount = totalCount;
        }

        public void AddItem(string title, Uri url, string text)
        {
            Items.Add(new SearchBlockItem(title, url, text));
            Count++;
        }
    }

    public struct SearchBlockItem
    {
        public string Title { get; }
        public Uri Url { get; }
        public string Text { get; }

        public SearchBlockItem(string title, Uri url, string text)
        {
            Title = title;
            Text = ContentHelper.GetDescription(text, 250);
            Url = url;
        }
    }
}