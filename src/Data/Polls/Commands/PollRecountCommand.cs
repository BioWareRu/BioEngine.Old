using BioEngine.Common.Models;
using BioEngine.Data.Core;

namespace BioEngine.Data.Polls.Commands
{
    public class PollRecountCommand : CommandBase
    {
        public PollRecountCommand(Poll poll)
        {
            Poll = poll;
        }

        public Poll Poll { get; }
    }
}