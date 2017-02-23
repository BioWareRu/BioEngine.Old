using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BioEngine.Common.Base;
using BioEngine.Common.DB;
using BioEngine.Common.Models;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace BioEngine.Site.Components
{
    [UsedImplicitly]
    public class ParentEntityProvider
    {
        private readonly BWContext _dbContext;

        public ParentEntityProvider(BWContext dbContext)
        {
            _dbContext = dbContext;
        }

        private async Task<List<Game>> GetGames()
        {
            return await GetParentModels<Game>();
        }

        private async Task<List<Developer>> GetDevelopers()
        {
            return await GetParentModels<Developer>();
        }

        private async Task<List<Topic>> GetTopics()
        {
            return await GetParentModels<Topic>();
        }

        private Dictionary<Type, List<ParentModel>> _parentModels = new Dictionary<Type, List<ParentModel>>();

        private async Task<List<T>> GetParentModels<T>() where T : ParentModel
        {
            if (!_parentModels.ContainsKey(typeof(T)))
            {
                _parentModels.Add(typeof(T), await _dbContext.Set<T>().Cast<ParentModel>().ToListAsync());
            }
            return _parentModels[typeof(T)].Cast<T>().ToList();
        }

        public async Task<ParentModel> GetParenyByUrl(string url)
        {
            var game = (await GetGames()).FirstOrDefault(x => x.Url == url);
            if (game != null) return game;
            var developer = (await GetDevelopers()).FirstOrDefault(x => x.Url == url);
            if (developer != null) return developer;
            var topic = (await GetTopics()).FirstOrDefault(x => x.Url == url);
            return topic;
        }
    }
}