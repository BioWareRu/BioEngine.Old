using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BioEngine.Common.Base;
using Microsoft.EntityFrameworkCore;

namespace BioEngine.Common.Models
{
    public class ArticleCat : ChildModel
    {
        [Key]
        public int Id { get; set; }

        public int Pid { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Descr { get; set; }
        public string GameOld { get; set; }
        public string Content { get; set; }
        public int Articles { get; set; }

        [ForeignKey(nameof(Pid))]
        public ArticleCat ParentCat { get; set; }

        public int TopicId { get; set; }

        [ForeignKey(nameof(TopicId))]
        public Topic Topic { get; set; }

        public override ParentModel Parent
        {
            get
            {
                if (GameId > 0)
                    return Game;
                if (DeveloperId > 0)
                    return Developer;
                if (TopicId > 0)
                    return Topic;

                throw new Exception("No parent!");
            }
        }

        public static void ConfigureDB(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ArticleCat>().ToTable("be_articles_cats");
            modelBuilder.Entity<ArticleCat>().Property(x => x.Id).HasColumnName("id");
            modelBuilder.Entity<ArticleCat>().Property(x => x.GameId).HasColumnName("game_id");
            modelBuilder.Entity<ArticleCat>().Property(x => x.DeveloperId).HasColumnName("developer_id");
            modelBuilder.Entity<ArticleCat>().Property(x => x.TopicId).HasColumnName("topic_id");
            modelBuilder.Entity<ArticleCat>().Property(x => x.Url).HasColumnName("url");
            modelBuilder.Entity<ArticleCat>().Property(x => x.Pid).HasColumnName("pid");
            modelBuilder.Entity<ArticleCat>().Property(x => x.Descr).HasColumnName("descr");
            modelBuilder.Entity<ArticleCat>().Property(x => x.Title).HasColumnName("title");
            modelBuilder.Entity<ArticleCat>().Property(x => x.Content).HasColumnName("content");
            modelBuilder.Entity<ArticleCat>().Property(x => x.Articles).HasColumnName("articles");
            modelBuilder.Entity<ArticleCat>().Property(x => x.GameOld).HasColumnName("game_old");
        }
    }
}