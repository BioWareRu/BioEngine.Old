using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using BioEngine.Common.Base;
using BioEngine.Common.DB;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BioEngine.Common.Models
{
    [Table("be_poll")]
    public class Poll: BaseModel<int>
    {
        private List<PollResultsEntry> _results;

        private bool _voted;

        [Key]
        [Column("poll_id")]
        public override int Id { get; set; }

        public string Question { get; set; }

        [Column("startdate")]
        public int StartDate { get; set; }

        [Column("options")]
        public string OptionsJson { get; set; }

        [Column("votes")]
        public string VotesJson { get; set; }

        public int NumChoices { get; set; }
        public int Multiple { get; set; }

        [Column("onoff")]
        public int OnOff { get; set; }

        public List<PollResultsEntry> Results
        {
            get
            {
                if (_results != null) return _results;
                _results = new List<PollResultsEntry>();
                var votes = Votes;
                var all = votes.Sum(vote => int.Parse(vote.Value));

                foreach (var option in Options)
                {
                    var optVotes = int.Parse(votes.FirstOrDefault(x => x.Key == "opt_" + option.Id).Value);
                    _results.Add(new PollResultsEntry
                    {
                        Id = option.Id,
                        Text = option.Text,
                        Result = all > 0 ? Math.Round(optVotes / (double) all, 4) : 0
                    });
                }
                return _results;
            }
        }

        public List<PollOption> Options
        {
            get { return JsonConvert.DeserializeObject<List<PollOption>>(OptionsJson); }
        }

        [NotMapped]
        public Dictionary<string, string> Votes
        {
            get { return JsonConvert.DeserializeObject<Dictionary<string, string>>(VotesJson); }
            set { VotesJson = JsonConvert.SerializeObject(value); }
        }

        public void SetVoted()
        {
            _voted = true;
        }

        public bool IsVoted()
        {
            return _voted;
        }

        public async Task<bool> GetIsVoted(BWContext dbContext, int userId, string ipAddress, string sessionId)
        {
            if (userId > 0)
            {
                return await
                    dbContext.PollVotes.AnyAsync(x => x.UserId == userId && x.PollId == Id);
            }
            else
            {
                return await
                    dbContext.PollVotes.AnyAsync(x => x.UserId == 0 && x.PollId == Id && x.Ip == ipAddress &&
                                                      x.SessionId == sessionId);
            }
        }

        public async Task Recount(BWContext dbContext)
        {
            var votes = new Dictionary<string, string>();
            foreach (var option in Options)
            {
                var voteCount =
                    await dbContext.PollVotes.CountAsync(x => x.PollId == Id && x.VoteOption == option.Id);
                votes.Add($"opt_{option.Id}", voteCount.ToString());
            }
            Votes = votes;
            await dbContext.SaveChangesAsync();
        }
    }

    public struct PollOption
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }

    public struct PollResultsEntry
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public double Result { get; set; }
    }
}