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
        public Guid Id { get; set; }

        private readonly DBConfiguration _configuration;

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

        public DbSet<Advertisement> Advertiesements { get; set; }

        public BWContext(DbContextOptions<BWContext> options, IOptions<DBConfiguration> configuration) : base(options)
        {
            Id = Guid.NewGuid();
            _configuration = configuration.Value;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new MySqlConnectionStringBuilder
            {
                Server = _configuration.Host,
                Port = (uint)_configuration.Port,
                UserID = _configuration.Username,
                Password = _configuration.Password,
                Database = _configuration.Database,
            };
            //_logger.Information($"PG: {builder.ToString()}");
            optionsBuilder.UseMySQL(builder.ToString());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Models.News.ConfigureDB(modelBuilder);
            Models.User.ConfigureDB(modelBuilder);
            Models.Developer.ConfigureDB(modelBuilder);
            Models.Game.ConfigureDB(modelBuilder);
            Models.Topic.ConfigureDB(modelBuilder);
            Models.Menu.ConfigureDB(modelBuilder);
            Models.Settings.ConfigureDB(modelBuilder);
            Models.Block.ConfigureDB(modelBuilder);
            Models.Advertisement.ConfigureDB(modelBuilder);
            Models.Poll.ConfigureDB(modelBuilder);
            Models.PollWho.ConfigureDB(modelBuilder);
        }
    }
}