using System.ComponentModel.DataAnnotations;
using BioEngine.Common.Base;
using Microsoft.EntityFrameworkCore;

namespace BioEngine.Common.Models
{
    public class User : BaseModel
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public int GroupId { get; set; }
        public string EMail { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public string PhotoUrl { get; set; }

        public static void ConfigureDb(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("be_core_members");
            modelBuilder.Entity<User>().Property(x => x.Id).HasColumnName("member_id");
            modelBuilder.Entity<User>().Property(x => x.Name).HasColumnName("name");
            modelBuilder.Entity<User>().Property(x => x.GroupId).HasColumnName("member_group_id");
            modelBuilder.Entity<User>().Property(x => x.EMail).HasColumnName("email");
            modelBuilder.Entity<User>().Property(x => x.PasswordHash).HasColumnName("members_pass_hash");
            modelBuilder.Entity<User>().Property(x => x.PasswordSalt).HasColumnName("members_pass_salt");
            modelBuilder.Entity<User>().Property(x => x.PhotoUrl).HasColumnName("pp_thumb_photo");
        }

        public bool IsAdmin => GroupId == 4;

        public bool HasRight(UserRights right, SiteTeamMember siteTeam = null)
        {
            if (IsAdmin) return true;
            if (siteTeam == null) return false;
            switch (right)
            {
                case UserRights.Developers:
                case UserRights.AddDevelopers:
                    if (siteTeam.Developers > 0)
                    {
                        return true;
                    }
                    break;
                case UserRights.EditDevelopers:
                    if (siteTeam.Developers > 1)
                    {
                        return true;
                    }
                    break;
                case UserRights.Games:
                case UserRights.AddGames:
                    if (siteTeam.Games > 0)
                    {
                        return true;
                    }
                    break;
                case UserRights.EditGames:
                    if (siteTeam.Games > 1)
                    {
                        return true;
                    }
                    break;
                case UserRights.News:
                case UserRights.AddNews:
                    if (siteTeam.News > 0)
                    {
                        return true;
                    }
                    break;
                case UserRights.EditNews:
                    if (
                        siteTeam.News > 1)
                    {
                        return true;
                    }
                    break;
                case UserRights.PubNews:
                    if (
                        siteTeam.News > 2)
                    {
                        return true;
                    }
                    break;
                case UserRights.FullNews:
                    if (
                        siteTeam.News > 3)
                    {
                        return true;
                    }
                    break;
                case UserRights.Articles:
                case UserRights.AddArticles:
                    if (
                        siteTeam.Articles > 0)
                    {
                        return true;
                    }
                    break;
                case UserRights.EditArticles:
                    if (
                        siteTeam.Articles > 1)
                    {
                        return true;
                    }
                    break;
                case UserRights.PubArticles:
                    if (
                        siteTeam.Articles > 2)
                    {
                        return true;
                    }
                    break;
                case UserRights.FullArticles:
                    if (
                        siteTeam.Articles > 3)
                    {
                        return true;
                    }
                    break;
                case UserRights.Files:
                case UserRights.AddFiles:
                    if (
                        siteTeam.Files > 0)
                    {
                        return true;
                    }
                    break;
                case UserRights.EditFiles:
                    if (
                        siteTeam.Files > 1)
                    {
                        return true;
                    }
                    break;
                case UserRights.PubFile:
                    if (
                        siteTeam.Files > 2)
                    {
                        return true;
                    }
                    break;
                case UserRights.FullFiles:
                    if (
                        siteTeam.Files > 3)
                    {
                        return true;
                    }
                    break;
                case UserRights.Gallery:
                case UserRights.AddGallery:
                    if (
                        siteTeam.Gallery > 0)
                    {
                        return true;
                    }
                    break;
                case UserRights.EditGallery:
                    if (
                        siteTeam.Gallery > 1)
                    {
                        return true;
                    }
                    break;
                case UserRights.PubGallery:
                    if (
                        siteTeam.Gallery > 2)
                    {
                        return true;
                    }
                    break;
                case UserRights.FullGallery:
                    if (
                        siteTeam.Gallery > 3)
                    {
                        return true;
                    }
                    break;
                case UserRights.Polls:
                case UserRights.AddPolls:
                    if (
                        siteTeam.Polls > 0)
                    {
                        return true;
                    }
                    break;
                case UserRights.EditPolls:
                    if (
                        siteTeam.Polls > 1)
                    {
                        return true;
                    }
                    break;
                default:
                    return false;
            }
            return false;
        }
    }

    public enum UserRights
    {
        Developers,
        AddDevelopers,
        EditDevelopers,
        Games,
        AddGames,
        EditGames,
        News,
        AddNews,
        EditNews,
        PubNews,
        FullNews,
        Articles,
        AddArticles,
        EditArticles,
        PubArticles,
        FullArticles,
        Files,
        AddFiles,
        EditFiles,
        PubFile,
        FullFiles,
        Gallery,
        AddGallery,
        EditGallery,
        PubGallery,
        FullGallery,
        Polls,
        AddPolls,
        EditPolls
    }
}