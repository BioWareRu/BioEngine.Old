using System.Collections.Generic;
using System.Threading.Tasks;
using BioEngine.Common.Base;
using BioEngine.Common.DB;
using BioEngine.Common.Models;
using BioEngine.Data.Core;
using BioEngine.Data.Files.Queries;
using BioEngine.Data.Gallery.Handlers;
using JetBrains.Annotations;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BioEngine.Data.Files.Handlers
{
    [UsedImplicitly]
    internal class FilesCategoryProcessHandler : CategoryProcessHandlerBase<FileCategoryProcessQuery, FileCat, File>
    {
        public FilesCategoryProcessHandler(IMediator mediator, BWContext dbContext,
            ILogger<GalleryCategoryProcessHandler> logger,
            ParentEntityProvider parentEntityProvider) : base(mediator, dbContext, logger, parentEntityProvider)
        {
        }

        protected override async Task<IEnumerable<File>> GetCatItems(FileCat cat, int count)
        {
            return (await Mediator.Send(new GetCategoryFilesQuery(cat) {PageSize = count})).models;
        }
    }
}