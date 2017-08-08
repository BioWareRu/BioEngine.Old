using System;
using System.Linq;
using BioEngine.Common.Interfaces;
using BioEngine.Data.Core;

namespace BioEngine.Data.News.Queries
{
    public class GetNewsQuery : ModelsListQueryBase<Common.Models.News>
    {
        public bool WithUnPublishedNews { get; set; }
        public IParentModel Parent { get; set; }

        public long? DateStart { get; set; }
        public long? DateEnd { get; set; }

        public override Func<IQueryable<Common.Models.News>, IQueryable<Common.Models.News>> OrderByFunc { get; protected set; } =
            q => q.OrderByDescending(x => x.Sticky).ThenByDescending(x => x.Date);
    }
}