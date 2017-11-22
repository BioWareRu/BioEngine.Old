using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BioEngine.Data.DB
{
    public abstract class DBConfiguration
    {
        protected DBConfiguration(IConfiguration configuration)
        {
            UserName = configuration["BE_DB_USERNAME"];
            DBName = configuration["BE_DB_NAME"];
            Password = configuration["BE_DB_PASSWORD"];
            Server = configuration["BE_DB_SERVER"];
            Port = uint.Parse(configuration["BE_DB_PORT"]);
        }

        protected string UserName { get; }

        protected string DBName { get; }

        protected string Password { get; }

        protected string Server { get; }

        protected uint Port { get; }

        public abstract void Configure(DbContextOptionsBuilder optionsBuilder);
    }
}