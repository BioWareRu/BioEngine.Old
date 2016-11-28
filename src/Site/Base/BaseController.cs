using System.Collections.Generic;
using System.Linq;
using BioEngine.Common.DB;
using BioEngine.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace BioEngine.Site.Base
{
    public abstract class BaseController : Controller
    {
        private readonly List<Settings> _settings;
        protected readonly BWContext Context;

        protected BaseController(BWContext context)
        {
            Context = context;
            _settings = context.Settings.ToList();
        }

        protected IEnumerable<Settings> Settings => _settings.AsReadOnly();
    }
}