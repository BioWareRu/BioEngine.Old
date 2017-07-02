using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Threading.Tasks;
using BioEngine.Common.Interfaces;
using BioEngine.Common.Models;

namespace BioEngine.Common.Base
{
    public class ChildModel<TPkType> : BaseModel<TPkType>, IChildModel
    {
        public virtual int? GameId { get; set; }
        public virtual int? DeveloperId { get; set; }
        public virtual int? TopicId { get; set; }

        [ForeignKey(nameof(GameId))]
        public virtual Game Game { get; set; }
        [ForeignKey(nameof(DeveloperId))]
        public virtual Developer Developer { get; set; }
        [ForeignKey(nameof(TopicId))]
        public virtual Topic Topic { get; set; }

        public async Task<IParentModel> Parent(ParentEntityProvider parentEntityProvider)
        {
            return await parentEntityProvider.GetModelParent(this);
        }
    }
}