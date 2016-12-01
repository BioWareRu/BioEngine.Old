using System;
using BioEngine.Common.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using MySQL.Data.Entity.Extensions;

namespace BioEngine.Common.DB
{
    public class BWContext : DbContext
    {
        private readonly DBConfiguration _configuration;

        public BWContext(DbContextOptions<BWContext> options, IOptions<DBConfiguration> configuration) : base(options)
        {
            Id = Guid.NewGuid();
            _configuration = configuration.Value;
        }

        public Guid Id { get; set; }

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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new MySqlConnectionStringBuilder
            {
                Server = _configuration.Host,
                Port = (uint) _configuration.Port,
                UserID = _configuration.Username,
                Password = _configuration.Password,
                Database = _configuration.Database
            };
            //_logger.Information($"PG: {builder.ToString()}");
            optionsBuilder.UseMySQL(builder.ToString());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Models.News.ConfigureDB(modelBuilder);
            User.ConfigureDB(modelBuilder);
            Developer.ConfigureDB(modelBuilder);
            Game.ConfigureDB(modelBuilder);
            Topic.ConfigureDB(modelBuilder);
            Menu.ConfigureDB(modelBuilder);
            Models.Settings.ConfigureDB(modelBuilder);
            Block.ConfigureDB(modelBuilder);
            Advertisement.ConfigureDB(modelBuilder);
            Poll.ConfigureDB(modelBuilder);
            PollWho.ConfigureDB(modelBuilder);
            Article.ConfigureDB(modelBuilder);
            ArticleCat.ConfigureDB(modelBuilder);
            File.ConfigureDB(modelBuilder);
            FileCat.ConfigureDB(modelBuilder);
            GalleryPic.ConfigureDB(modelBuilder);
            GalleryCat.ConfigureDB(modelBuilder);
        }
    }
}