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

        public DbSet<Advertisement> Advertiesements { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Models.News.ConfigureDb(modelBuilder);
            User.ConfigureDb(modelBuilder);
            Developer.ConfigureDb(modelBuilder);
            Game.ConfigureDb(modelBuilder);
            Topic.ConfigureDb(modelBuilder);
            Menu.ConfigureDb(modelBuilder);
            Models.Settings.ConfigureDb(modelBuilder);
            Block.ConfigureDb(modelBuilder);
            Advertisement.ConfigureDb(modelBuilder);
            Poll.ConfigureDb(modelBuilder);
            PollWho.ConfigureDb(modelBuilder);
            Article.ConfigureDb(modelBuilder);
            ArticleCat.ConfigureDb(modelBuilder);
            File.ConfigureDb(modelBuilder);
            FileCat.ConfigureDb(modelBuilder);
            GalleryPic.ConfigureDb(modelBuilder);
            GalleryCat.ConfigureDb(modelBuilder);
        }
    }
}