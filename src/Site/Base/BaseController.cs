using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using BioEngine.Common.DB;
using BioEngine.Common.Models;
using BioEngine.Site.Components;
using BioEngine.Site.Components.Url;
using Microsoft.AspNetCore.Mvc;

namespace BioEngine.Site.Base
{
    public abstract class BaseController : Controller
    {
        protected readonly UrlManager UrlManager;
        private readonly List<Settings> _settings;
        protected readonly BWContext Context;
        protected readonly ParentEntityProvider ParentEntityProvider;

        protected BaseController(BWContext context, ParentEntityProvider parentEntityProvider, UrlManager urlManager)
        {
            UrlManager = urlManager;
            Context = context;
            ParentEntityProvider = parentEntityProvider;
            _settings = context.Settings.ToList();
        }

        protected IEnumerable<Settings> Settings => _settings.AsReadOnly();

        private static readonly Regex CatchAllRegex = new Regex("(.*)/([a-zA-Z0-9_]+).html");

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
    }
}