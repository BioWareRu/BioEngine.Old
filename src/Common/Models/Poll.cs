using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BioEngine.Common.Models
{
    public class Poll
    {
        private List<PollResultsEntry> _results;

        private bool voted;
        public int PollId { get; set; }

        public string Question { get; set; }
        public int StartDate { get; set; }

        public string OptionsJson { get; set; }
        public string VotesJson { get; set; }

        public int NumChoises { get; set; }
        public int Multiple { get; set; }
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
                        Result = all > 0 ? Math.Round(optVotes/(double) all, 4) : 0
                    });
                }
                return _results;
            }
        }

        public List<PollOption> Options
        {
            get { return JsonConvert.DeserializeObject<List<PollOption>>(OptionsJson); }
        }

        public Dictionary<string, string> Votes
        {
            get { return JsonConvert.DeserializeObject<Dictionary<string, string>>(VotesJson); }
        }

        public void SetVoted()
        {
            voted = true;
        }

        public bool IsVoted()
        {
            return voted;
        }

        public static void ConfigureDB(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Poll>().ToTable("be_poll");
            modelBuilder.Entity<Poll>().HasKey(nameof(PollId));
            modelBuilder.Entity<Poll>().Property(x => x.PollId).HasColumnName("poll_id");
            modelBuilder.Entity<Poll>().Property(x => x.Question).HasColumnName("question");
            modelBuilder.Entity<Poll>().Property(x => x.StartDate).HasColumnName("startdate");
            modelBuilder.Entity<Poll>().Property(x => x.OptionsJson).HasColumnName("options");
            modelBuilder.Entity<Poll>().Property(x => x.VotesJson).HasColumnName("votes");
            modelBuilder.Entity<Poll>().Property(x => x.NumChoises).HasColumnName("num_choices");
            modelBuilder.Entity<Poll>().Property(x => x.Multiple).HasColumnName("multiple");
            modelBuilder.Entity<Poll>().Property(x => x.OnOff).HasColumnName("onoff");
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