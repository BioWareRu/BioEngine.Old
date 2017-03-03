using System.Linq;
using BioEngine.Common.Models;
using JsonApiDotNetCore.Data;
using JsonApiDotNetCore.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BioEngine.API.Data
{
    public class NewsRepository : DefaultEntityRepository<News>
    {
        public NewsRepository(DbContext context, ILoggerFactory loggerFactory, IJsonApiContext jsonApiContext) : base(
            context, loggerFactory, jsonApiContext)
        {
        }

        public override IQueryable<News> Get()
        {
            var query = base.Get();
            return query;
        }
    }
}