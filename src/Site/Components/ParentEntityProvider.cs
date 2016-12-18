using System.Collections.Generic;
using System.Linq;
using BioEngine.Common.Base;
using BioEngine.Common.DB;
using BioEngine.Common.Models;
using JetBrains.Annotations;

namespace BioEngine.Site.Components
{
    [UsedImplicitly]
    public class ParentEntityProvider
    {
        public ParentEntityProvider(BWContext dbContext)
        {
            Games = dbContext.Games.ToList();
            Developers = dbContext.Developers.ToList();
            Topics = dbContext.Topics.ToList();
        }

        private List<Game> Games { get; }
        private List<Developer> Developers { get; }
        private List<Topic> Topics { get; }

        public ParentModel GetParenyByUrl(string url)
        {
            var game = Games.FirstOrDefault(x => x.Url == url);
            if (game != null) return game;
            var developer = Developers.FirstOrDefault(x => x.Url == url);
            if (developer != null) return developer;
            var topic = Topics.FirstOrDefault(x => x.Url == url);
            return topic;
        }
    }
}