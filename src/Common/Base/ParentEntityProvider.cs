using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BioEngine.Common.DB;
using BioEngine.Common.Interfaces;
using BioEngine.Common.Models;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace BioEngine.Common.Base
{
    [UsedImplicitly]
    public class ParentEntityProvider
    {
        private readonly BWContext _dbContext;

        public ParentEntityProvider(BWContext dbContext)
        {
            _dbContext = dbContext;
        }

        private async Task<List<Game>> GetGamesAsync()
        {
            return await GetParentModelsAsync<Game>();
        }

        private async Task<List<Developer>> GetDevelopersAsync()
        {
            return await GetParentModelsAsync<Developer>();
        }

        private async Task<List<Topic>> GetTopicsAsync()
        {
            return await GetParentModelsAsync<Topic>();
        }

        private readonly Dictionary<Type, List<IParentModel>> _parentModels = new Dictionary<Type, List<IParentModel>>();

        private async Task<List<T>> GetParentModelsAsync<T>() where T : class, IParentModel
        {
            if (!_parentModels.ContainsKey(typeof(T)))
            {
                _parentModels.Add(typeof(T), await _dbContext.Set<T>().Cast<IParentModel>().ToListAsync());
            }
            return _parentModels[typeof(T)].Cast<T>().ToList();
        }

        public async Task<IParentModel> GetParenyByUrlAsync(string url)
        {
            var game = (await GetGamesAsync()).FirstOrDefault(x => x.Url == url);
            if (game != null) return game;
            var developer = (await GetDevelopersAsync()).FirstOrDefault(x => x.Url == url);
            if (developer != null) return developer;
            var topic = (await GetTopicsAsync()).FirstOrDefault(x => x.Url == url);
            return topic;
        }

        public async Task<IParentModel> GetModelParentAsync(IChildModel model)
        {
            IParentModel parent = null;
            if (model.GameId > 0)
            {
                parent = (await GetGamesAsync()).FirstOrDefault(x => x.Id == model.GameId);
            }
            if (model.DeveloperId > 0)
            {
                parent = (await GetDevelopersAsync()).FirstOrDefault(x => x.Id == model.DeveloperId);
            }
            if (model.TopicId > 0)
            {
                parent = (await GetTopicsAsync()).FirstOrDefault(x => x.Id == model.TopicId);
            }
            if (parent == null)
            {
                throw new Exception("No parent!");
            }
            return parent;
        }
    }
}