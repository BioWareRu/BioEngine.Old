using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using BioEngine.Common.Base;
using BioEngine.Common.Helpers;
using Newtonsoft.Json;

namespace BioEngine.Common.Models
{
    [Table("be_gallery")]
    public class GalleryPic : ChildModel<int>
    {
        [JsonProperty]
        public int CatId { get; set; }

        [Column("files")]
        [JsonProperty]
        public string FilesJson { get; set; }

        [JsonProperty]
        public string Desc { get; set; }

        [JsonProperty]
        public int Pub { get; set; }

        [ForeignKey(nameof(CatId))]
        public GalleryCat Cat { get; set; }

        [NotMapped]
        public override int? TopicId { get; set; }

        [NotMapped]
        public override Topic Topic { get; set; }

        private List<GalleryPicFile> _files;

        [JsonProperty]
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
                            var name = "n/a";
                            if (file.ContainsKey("name"))
                            {
                                name = file["name"].ToString();
                            }
                            long size = 0;
                            if (file.ContainsKey("size"))
                            {
                                size = long.Parse(file["size"].ToString());
                            }
                            var res = "0x0";
                            if (file.ContainsKey("res"))
                            {
                                res = file["res"].ToString();
                            }
                            var picFile = new GalleryPicFile(name, size, res);
                            _files.Add(picFile);
                        }
                    }
                    if (!_files.Any())
                    {
                        throw new Exception($"Picture {Id} has no files");
                    }
                }
                return _files;
            }
        }
    }

    public struct GalleryPicFile
    {
        [JsonProperty] public string Name;
        [JsonProperty] public long Size;
        [JsonProperty] public string Resolution;

        public GalleryPicFile(string name, long size, string resolution)
        {
            Name = name;
            Size = size;
            Resolution = resolution;
        }
    }
}