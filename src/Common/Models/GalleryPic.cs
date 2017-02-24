using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;
using BioEngine.Common.Base;
using BioEngine.Common.Helpers;
using BioEngine.Common.Interfaces;

namespace BioEngine.Common.Models
{
    [Table("be_gallery")]
    public class GalleryPic : IChildModel
    {
        [Key]

        public int Id { get; set; }


        public int CatId { get; set; }

        [Column("files")]
        public string FilesJson { get; set; }

        public string Desc { get; set; }

        public int Pub { get; set; }

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

        public async Task<ParentModel> Parent(ParentEntityProvider parentEntityProvider)
        {
            return await parentEntityProvider.GetModelParent(this);
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