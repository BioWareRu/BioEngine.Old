using System.Linq;
using System.Threading.Tasks;
using BioEngine.Common.DB;
using BioEngine.Common.Models;
using BioEngine.Data.Core;
using BioEngine.Data.Files.Queries;
using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BioEngine.Data.Files.Handlers
{
    [UsedImplicitly]
    internal class GetFilesCategoryHandler : QueryHandlerBase<GetFilesCategoryQuery, FileCat>
    {
        public GetFilesCategoryHandler(IMediator mediator, BWContext dbContext) : base(mediator, dbContext)
        {
        }

        public override async Task<FileCat> Handle(GetFilesCategoryQuery message)
        {
            var catQuery = DBContext.FileCats.AsQueryable();

            if (!string.IsNullOrEmpty(message.Url))
            {
                catQuery = catQuery.Where(x => x.Url == message.Url);
            }

            if (message.Parent != null)
            {
                catQuery = ApplyParentCondition(catQuery, message.Parent);
            }

            if (message.ParentCat != null)
            {
                catQuery = catQuery.Where(x => x.Pid == message.ParentCat.Id);
            }
            else
            {
                catQuery = catQuery.Include(x => x.ParentCat);
            }

            var cat = await catQuery.FirstOrDefaultAsync();
            if (cat != null)
            {
                cat = await Mediator.Send(new FileCategoryProcessQuery(cat, message));
            }
            return cat;
        }
    }
}