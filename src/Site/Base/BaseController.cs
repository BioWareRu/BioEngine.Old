﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using BioEngine.Common.Base;
using BioEngine.Common.DB;
using BioEngine.Common.Interfaces;
using BioEngine.Common.Models;
using BioEngine.Site.Components;
using BioEngine.Site.Components.Url;
using BioEngine.Site.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace BioEngine.Site.Base
{
    public abstract class BaseController : Controller
    {
        protected readonly UrlManager UrlManager;
        private readonly List<Settings> _settings;
        protected readonly BWContext Context;
        protected readonly ParentEntityProvider ParentEntityProvider;
        protected readonly BaseViewModelConfig ViewModelConfig;

        protected BaseController(BWContext context, ParentEntityProvider parentEntityProvider, UrlManager urlManager,
            IOptions<AppSettings> appSettingsOptions)
        {
            UrlManager = urlManager;
            Context = context;
            ParentEntityProvider = parentEntityProvider;
            _settings = context.Settings.ToList();
            ViewModelConfig = new BaseViewModelConfig(UrlManager, appSettingsOptions.Value, _settings, parentEntityProvider);
        }

        protected IEnumerable<Settings> Settings => _settings.AsReadOnly();

        private static readonly Regex CatchAllRegex = new Regex("(.*)/([a-zA-Z0-9_]+).html");
        private static readonly Regex CatchAllCatRegex = new Regex("(.*).html");
        private static readonly Regex CatchAllCatWithPageRegex = new Regex("(.*)/page/([0-9]+).html");

        protected bool ParseCatchAll(string url, out string parentUrl, out string pageUrl)
        {
            var match = CatchAllRegex.Match(url);
            if (match.Success)
            {
                parentUrl = match.Groups[1].Value;
                pageUrl = match.Groups[2].Value;
                return true;
            }
            parentUrl = null;
            pageUrl = null;
            return false;
        }

        protected bool ParseCatchAll(string url, out string catUrl, out int page)
        {
            if (url.IndexOf("/page/", StringComparison.Ordinal) > -1)
            {
                var match = CatchAllCatWithPageRegex.Match(url);
                if (match.Success)
                {
                    catUrl = match.Groups[1].Value;
                    page = int.Parse(match.Groups[2].Value);
                    return true;
                }
            }
            else
            {
                var match = CatchAllCatRegex.Match(url);
                if (match.Success)
                {
                    catUrl = match.Groups[1].Value;
                    page = 1;
                    return true;
                }
            }
            catUrl = null;
            page = 0;
            return false;
        }

        protected async Task<List<CatsTree<TCat, TEntity>>> LoadCatsTree<TCat, TEntity>(IParentModel parent, DbSet<TCat> dbSet,
            Func<ICat<TCat>, Task<List<TEntity>>> getLast)
            where TCat : class, ICat<TCat> where TEntity : IChildModel
        {
            var rootCatsQuery = dbSet.AsQueryable();
            switch (parent.Type)
            {
                case ParentType.Game:
                    rootCatsQuery = rootCatsQuery.Where(x => x.GameId == parent.Id);
                    break;
                case ParentType.Developer:
                    rootCatsQuery = rootCatsQuery.Where(x => x.DeveloperId == parent.Id);
                    break;
                case ParentType.Topic:
                    rootCatsQuery = rootCatsQuery.Where(x => x.TopicId == parent.Id);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var rootCats = await rootCatsQuery.ToListAsync();
            var catsTree = new List<CatsTree<TCat, TEntity>>();
            foreach(var rootCat in rootCats)
            {
                catsTree.Add(await LoadCatChildren(rootCat, getLast));
            }
            return catsTree;
        }

        private async Task<CatsTree<TCat, TEntity>> LoadCatChildren<TCat, TEntity>(TCat cat, Func<ICat<TCat>, Task<List<TEntity>>> getLast)
            where TCat : class, ICat<TCat> where TEntity : IChildModel
        {
            await Context.Entry(cat).Collection(x => x.Children).LoadAsync();
            var children = new List<CatsTree<TCat, TEntity>>();
            foreach(var child in cat.Children)
            {
                children.Add(await LoadCatChildren(child, getLast));
            }
            return new CatsTree<TCat, TEntity>(cat, await getLast(cat), children);
        }
    }
}