using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BioEngine.Site.Components
{
    public class AppSettings
    {
        public string AssetsDomain { get; set; }

        public string IPBDomain { get; set; }
        public string IPBUploadsDomain { get; set; }

        public string GamesImagesPath { get; set; }
        public string DevelopersImagesPath { get; set; }
        public string TopicsImagesPath { get; set; }
    }
}
