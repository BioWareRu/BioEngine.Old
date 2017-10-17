using System;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using BioEngine.Common.Base;
using BioEngine.Common.Interfaces;
using BioEngine.Common.Models;
using BioEngine.Site.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using BioEngine.Data.Base.Queries;
using BioEngine.Data.Users.Queries;
using MediatR;

namespace BioEngine.Site.Base
{
    public abstract class BaseController : Controller
    {
        protected readonly IMediator Mediator;
        protected readonly BaseViewModelConfig ViewModelConfig;

        protected BaseController(IMediator mediator, IOptions<AppSettings> appSettingsOptions,
            IContentHelperInterface contentHelper)
        {
            Mediator = mediator;
            var settings = mediator.Send(new GetSettingsQuery()).GetAwaiter().GetResult().models;
            ViewModelConfig = new BaseViewModelConfig(appSettingsOptions.Value, settings, contentHelper);
        }

        private static readonly Regex CatchAllRegex = new Regex("(.*)/([a-zA-Z0-9_-]+).html");
        private static readonly Regex CatchAllCatRegex = new Regex("(.*).html");
        private static readonly Regex CatchAllCatWithPageRegex = new Regex("(.*)/page/([0-9]+).html");

        protected bool ParseCatchAll(string url, out string parentUrl, out string pageUrl)
        {
            if (!string.IsNullOrEmpty(url))
            {
                var match = CatchAllRegex.Match(url);
                if (match.Success)
                {
                    parentUrl = match.Groups[1].Value;
                    pageUrl = match.Groups[2].Value;
                    return true;
                }
            }
            parentUrl = null;
            pageUrl = null;
            return false;
        }

        protected bool ParseCatchAll(string url, out string catUrl, out int page)
        {
            if (!string.IsNullOrEmpty(url))
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
            }
            catUrl = null;
            page = 0;
            return false;
        }

        /*protected async Task<List<CatsTree<TCat, TEntity>>> LoadCatsTree<TCat, TEntity>(IParentModel parent,
            DbSet<TCat> dbSet,
            Func<ICat<TCat>, Task<List<TEntity>>> getLast)
            where TCat : class, ICat<TCat> where TEntity : IChildModel
        {
            var rootCatsQuery = dbSet.AsQueryable();
            switch (parent.Type)
            {
                case ParentType.Game:
                    rootCatsQuery = rootCatsQuery.Where(x => x.GameId == (int) parent.GetId());
                    break;
                case ParentType.Developer:
                    rootCatsQuery = rootCatsQuery.Where(x => x.DeveloperId == (int) parent.GetId());
                    break;
                case ParentType.Topic:
                    rootCatsQuery = rootCatsQuery.Where(x => x.TopicId == (int) parent.GetId());
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var rootCats = await rootCatsQuery.ToListAsync();
            var catsTree = new List<CatsTree<TCat, TEntity>>();
            foreach (var rootCat in rootCats)
            {
                catsTree.Add(await LoadCatChildren(rootCat, getLast));
            }
            return catsTree;
        }

        private async Task<CatsTree<TCat, TEntity>> LoadCatChildren<TCat, TEntity>(TCat cat,
            Func<ICat<TCat>, Task<List<TEntity>>> getLast)
            where TCat : class, ICat<TCat> where TEntity : IChildModel
        {
            await Mediator.Send(new LoadCatChildrenRequest<TCat>(cat));
            var children = new List<CatsTree<TCat, TEntity>>();
            foreach (var child in cat.Children)
            {
                children.Add(await LoadCatChildren(child, getLast));
            }
            return new CatsTree<TCat, TEntity>(cat, await getLast(cat), children);
        }*/

        private User _user;

        private async Task<User> GetUser()
        {
            if (_user != null) return _user;
            if (!User.Identity.IsAuthenticated)
            {
                return null;
            }
            var userId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value);
            _user = await Mediator.Send(new GetUserByIdQuery(userId));
            return _user;
        }

        protected async Task<bool> HasRight(UserRights right)
        {
            var user = await GetUser();
            return user != null && user.HasRight(right, user.SiteTeamMember);
        }
    }
}