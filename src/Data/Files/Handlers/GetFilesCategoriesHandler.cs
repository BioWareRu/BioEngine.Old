using System.Collections.Generic;
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
    internal class GetFilesCategoriesHandler : QueryHandlerBase<GetFilesCategoriesQuery, IEnumerable<FileCat>>
    {
        public GetFilesCategoriesHandler(IMediator mediator, BWContext dbContext) : base(mediator, dbContext)
        {
        }

        public override async Task<IEnumerable<FileCat>> Handle(GetFilesCategoriesQuery message)
        {
            var query = DBContext.FileCats.AsQueryable();
            if (message.Parent != null)
            {
                query = ApplyParentCondition(query, message.Parent);
            }

            if (message.ParentCat != null)
            {
                query = query.Where(x => x.Pid == message.ParentCat.Id);
            }
            else if (message.OnlyRoot)
            {
                query = query.Where(x => x.Pid == null);
            }


            var cats = await query.ToListAsync();
            foreach (var cat in cats)
            {
                await Mediator.Send(new FileCategoryProcessQuery(cat, message));
            }

            return cats;
        }
    }
}