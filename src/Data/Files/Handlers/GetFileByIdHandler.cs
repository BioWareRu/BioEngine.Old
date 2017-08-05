using System.Linq;
using System.Threading.Tasks;
using BioEngine.Common.DB;
using BioEngine.Common.Models;
using BioEngine.Data.Core;
using BioEngine.Data.Files.Queries;
using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BioEngine.Data.Files.Handlers
{
    [UsedImplicitly]
    internal class GetFileByIdHandler : QueryHandlerBase<GetFileByIdQuery, File>
    {
        public GetFileByIdHandler(IMediator mediator, BWContext dbContext, ILogger<GetFileByIdHandler> logger) : base(
            mediator, dbContext, logger)
        {
        }

        protected override async Task<File> RunQuery(GetFileByIdQuery message)
        {
            var file = await DBContext.Files
                .Where(x => x.Id == message.Id)
                .Include(x => x.Cat)
                .Include(x => x.Author)
                .Include(x => x.Game)
                .Include(x => x.Developer)
                .FirstOrDefaultAsync();
            if (file != null)
            {
                file.Cat =
                    await Mediator.Send(
                        new FileCategoryProcessQuery(file.Cat, new GetFilesCategoryQuery(file.Parent)));
            }
            return file;
        }
    }
}