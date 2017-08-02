using System;
using System.Collections.Generic;
using System.Text;

namespace BioEngine.Common.Interfaces
{
    public interface IAppSettings
    {
        string AssetsDomain { get; set; }

        string IPBDomain { get; set; }
        string IPBUploadsDomain { get; set; }

        string GamesImagesPath { get; set; }
        string DevelopersImagesPath { get; set; }
        string TopicsImagesPath { get; set; }
        string Title { get; set; }
        string SiteDomain { get; set; }
        string ImagesDomain { get; set; }
        string SocialLogo { get; set; }
    }
}