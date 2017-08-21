using System.Collections.Generic;
using System.Net.Http;
using Xunit;

namespace BioEngine.Site.Tests
{
    public class PagesTests
    {
        [Fact]
        public void TestPages()
        {
            var pages = new List<string>
            {
                "http://localhost:5000",
                "http://localhost:5000/2017/02/28/mass_effect_andromeda_pervyi_vzglyad_na_koncepty_iz_artbuka_i.html",
                "http://localhost:5000/dragon_age_2.html",
                "http://localhost:5000/mass_effect_andromeda/news.html",
                "http://localhost:5000/mass_effect_andromeda/articles.html",
                "http://localhost:5000/mass_effect_andromeda/gallery.html",
                "http://localhost:5000/mass_effect_andromeda/articles/other/what_wewant_from_meandromeda.html",
                "http://localhost:5000/mass_effect_andromeda/gallery/preview/page/2.html#nanogallery/nanoGallery/0/6513",
                "http://localhost:5000/dragon_age/files.html",
                "http://localhost:5000/dragon_age/files/video/video.html",
                "http://localhost:5000/dragon_age/files/video/trailers/trailers.html",
                "http://localhost:5000/dragon_age/files/video/trailers/witch_hunt_trailer.html",
                "http://localhost:5000/dragon_age/download/other/witch_hunt_rus.html",
                "http://localhost:5000/biowareru/articles.html",
                "http://localhost:5000/biowareru/articles/brc_interview/brc_interview.html",
                "http://localhost:5000/bioware/gallery/brc_10.html"
            };
            var client = new HttpClient();
            foreach (var page in pages)
            {
                var response = client.GetAsync(page).Result;
                Assert.True(response.IsSuccessStatusCode);
            }
        }
    }
}