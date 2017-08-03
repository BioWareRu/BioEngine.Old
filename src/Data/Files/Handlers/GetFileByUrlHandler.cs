using System.Linq;
using System.Threading.Tasks;
using BioEngine.Common.DB;
using BioEngine.Common.Models;
using BioEngine.Data.Core;
using BioEngine.Data.Files.Requests;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BioEngine.Data.Files.Handlers
{
    public class GetFileByUrlHandler : RequestHandlerBase<GetFileByUrlRequest, File>
    {
        public GetFileByUrlHandler(IMediator mediator, BWContext dbContext) : base(mediator, dbContext)
        {
        }

        public override async Task<File> Handle(GetFileByUrlRequest message)
        {
            var query = DBContext.Files.Include(x => x.Cat).Include(x => x.Author).AsQueryable();
            query = query.Where(x => x.Url == message.Url);
            query = ApplyParentCondition(query, message.Parent);
            var files = await query.ToListAsync();
            if (files.Any())
            {
                File file = null;
                if (files.Count > 1)
                    foreach (var candidate in files)
                    {
                        if (candidate.Cat.Url != message.CatUrl) continue;
                        file = candidate;
                        break;
                    }
                else
                    file = files[0];
                if (file != null)
                {
                    file.Cat =
                        await Mediator.Send(
                            new FileCategoryProcessRequest(file.Cat, new GetFilesCategoryRequest(message.Parent)));
                    return file;
                }
            }

            return null;
        }
    }
}