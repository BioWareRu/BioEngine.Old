using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BioEngine.Common.DB;
using BioEngine.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace BioEngine.Site.Components.Url
{
    public class TopicUrlManager:EntityUrlManager
    {
        public TopicUrlManager(AppSettings settings, BWContext dbContext, IUrlHelper urlHelper) : base(settings, dbContext, urlHelper)
        {
        }

        public string PublicUrl(Topic topic)
        {
            return "#";
        }
    }
}
