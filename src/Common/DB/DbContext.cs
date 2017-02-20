using System.Linq;
using BioEngine.Common.Models;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace BioEngine.Common.DB
{
    [UsedImplicitly]
    public class BWContext : DbContext
    {
        public BWContext(DbContextOptions<BWContext> options) : base(options)
        {
        }

        public DbSet<News> News { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Developer> Developers { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Settings> Settings { get; set; }
        public DbSet<Block> Blocks { get; set; }
        public DbSet<Poll> Polls { get; set; }
        public DbSet<PollWho> PollVotes { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<ArticleCat> ArticleCats { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<FileCat> FileCats { get; set; }
        public DbSet<GalleryPic> GalleryPics { get; set; }
        public DbSet<GalleryCat> GalleryCats { get; set; }
        public DbSet<SiteTeamMember> SiteTeam { get; set; }

        public DbSet<Advertisement> Advertiesements { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.SetSimpleUnderscoreTableNameConvention();
        }
    }

    public static class ModelBuilderExtensions
    {
        public static void SetSimpleUnderscoreTableNameConvention(this ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var mutableProperty in entity.GetProperties())
                {
                    if (mutableProperty.FindAnnotation("Relational:ColumnName") != null) continue;
                    mutableProperty.Relational().ColumnName = string
                        .Concat(mutableProperty.Name.Select(
                            (x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString()))
                        .ToLower();
                }
            }
        }
    }
}