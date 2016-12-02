using System;
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
    }
}