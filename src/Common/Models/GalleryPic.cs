using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BioEngine.Common.Base;
using BioEngine.Common.Helpers;
using BioEngine.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BioEngine.Common.Models
{
    public class GalleryPic : IChildModel
    {
        [Key]
        public int Id { get; set; }

        public int CatId { get; set; }
        public string GameOld { get; set; }
        public string FilesJson { get; set; }
        public string Desc { get; set; }
        public int AuthorId { get; set; }
        public int Count { get; set; }
        public int Date { get; set; }
        public int Pub { get; set; }

        [ForeignKey(nameof(AuthorId))]
        public User Author { get; set; }

        [ForeignKey(nameof(CatId))]
        public GalleryCat Cat { get; set; }

        public int? GameId { get; set; }
        public int? DeveloperId { get; set; }

        [NotMapped]
        public int? TopicId { get; set; }

        [ForeignKey(nameof(GameId))]
        public Game Game { get; set; }

        [ForeignKey(nameof(DeveloperId))]
        public Developer Developer { get; set; }

        [NotMapped]
        public Topic Topic { get; set; }

        [NotMapped]
        public ParentModel Parent
        {
            get { return ParentModel.GetParent(this); }
            set { ParentModel.SetParent(this, value); }
        }

        private List<GalleryPicFile> _files;

        public List<GalleryPicFile> Files
        {
            get
            {
                if (_files == null)
                {
                    _files = new List<GalleryPicFile>();
                    var files = PhpHelper.Deserialize(FilesJson) as ArrayList;
                    if (files != null)
                    {
                        foreach (Hashtable file in files)
                        {
                            var picFile = new GalleryPicFile(file["name"].ToString(),
                                long.Parse(file["size"].ToString()), file["res"].ToString());
                            _files.Add(picFile);
                        }
                    }
                }
                return _files;
            }
        }

        public static void ConfigureDb(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GalleryPic>().ToTable("be_gallery");
            modelBuilder.Entity<GalleryPic>().Property(x => x.Id).HasColumnName("id");
            modelBuilder.Entity<GalleryPic>().Property(x => x.GameId).HasColumnName("game_id");
            modelBuilder.Entity<GalleryPic>().Property(x => x.DeveloperId).HasColumnName("developer_id");
            modelBuilder.Entity<GalleryPic>().Property(x => x.GameOld).HasColumnName("game_old");
            modelBuilder.Entity<GalleryPic>().Property(x => x.FilesJson).HasColumnName("files");
            modelBuilder.Entity<GalleryPic>().Property(x => x.CatId).HasColumnName("cat_id");
            modelBuilder.Entity<GalleryPic>().Property(x => x.Desc).HasColumnName("desc");
            modelBuilder.Entity<GalleryPic>().Property(x => x.AuthorId).HasColumnName("author_id");
            modelBuilder.Entity<GalleryPic>().Property(x => x.Count).HasColumnName("count");
            modelBuilder.Entity<GalleryPic>().Property(x => x.Pub).HasColumnName("pub");
            modelBuilder.Entity<GalleryPic>().Property(x => x.Date).HasColumnName("date");
        }
    }

    public struct GalleryPicFile
    {
        public string Name;
        public long Size;
        public string Resolution;

        public GalleryPicFile(string name, long size, string resolution)
        {
            Name = name;
            Size = size;
            Resolution = resolution;
        }
    }
}