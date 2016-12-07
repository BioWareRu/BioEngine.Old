using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BioEngine.Common.Base;
using BioEngine.Common.Models;

namespace BioEngine.Common.Interfaces
{
    public interface IChildModel
    {
        int? GameId { get; set; }
        int? DeveloperId { get; set; }
        int? TopicId { get; set; }

        Game Game { get; set; }

        Developer Developer { get; set; }

        Topic Topic { get; set; }

        ParentModel Parent { get; set; }
    }
}