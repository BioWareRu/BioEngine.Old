using System.Linq;
using System.Threading.Tasks;
using BioEngine.Common.DB;
using BioEngine.Common.Models;
using BioEngine.Data.Core;
using BioEngine.Data.Files.Requests;
using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BioEngine.Data.Files.Handlers
{
    [UsedImplicitly]
    public class GetFileByIdHandler : RequestHandlerBase<GetFileByIdRequest, File>
    {
        public GetFileByIdHandler(IMediator mediator, BWContext dbContext) : base(mediator, dbContext)
        {
        }

        public override async Task<File> Handle(GetFileByIdRequest message)
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
                        new FileCategoryProcessRequest(file.Cat, new GetFilesCategoryRequest(file.Parent)));
            }
            return file;
        }
    }
}