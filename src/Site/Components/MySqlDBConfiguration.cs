using BioEngine.Common.DB;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace BioEngine.Site.Components
{
    [UsedImplicitly]
    public class MySqlDBConfiguration : DBConfiguration
    {
        public override void Configure(DbContextOptionsBuilder optionsBuilder)
        {
            var mysqlConnBuilder = new MySqlConnectionStringBuilder
            {
                Server = Server,
                Port = Port,
                UserID = UserName,
                Password = Password,
                Database = DBName
            };
            optionsBuilder.UseMySql(mysqlConnBuilder.ConnectionString);
        }

        public MySqlDBConfiguration(IConfigurationRoot configuration) : base(configuration)
        {
        }
    }
}