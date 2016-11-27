using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BioEngine.Common.DB;
using Microsoft.AspNetCore.Mvc;
using BioEngine.Common.Models;

namespace BioEngine.Site.Base
{
    public abstract class BaseController : Controller
    {
        protected readonly BWContext Context;
        private readonly List<Settings> _settings;

        public BaseController(BWContext context)
        {
            Context = context;
            _settings = context.Settings.ToList();
        }

        protected IEnumerable<Settings> Settings { get { return _settings.AsReadOnly(); } }
    }
}