using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace BioEngine.Site.ViewComponents
{
    public class GamesCarouselViewComponent : ViewComponent
    {
        private static List<GameItem> Games = new List<GameItem>() {
            new GameItem("sonic","Sonic Chronicles","sonic"),
            new GameItem("bg","Baldur's Gate","baldurs_gate"),
            new GameItem("da","Dragon Age: Начало","dragon_age"),
            new GameItem("tor","Star Wars: The Old Republic","the_old_republic"),
            new GameItem("da2","Dragon Age II","dragon_age_2"),
            new GameItem("dai","Dragon Age: Инквизиция","dragon_age_inquisition"),
            new GameItem("mea","Mass Effect: Andromeda","mass_effect_andromeda"),
            new GameItem("me3","Mass Effect 3","mass_effect_3"),
            new GameItem("kotor","Stat Wars: Knights of the Old Republic","kotor"),
            new GameItem("me","Mass Effect","mass_effect"),
            new GameItem("nwn","NeverWinter Nights","neverwinter_nights"),
            new GameItem("me2","Mass Effect 2","mass_effect_2"),
            new GameItem("nno","NeverWinter Online","neverwinter"),
            new GameItem("je","Jade Empire","jade_empire"),
        };

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return await Task.Run(() =>
            {
                return View(Games.AsReadOnly());
            });
        }
    }

    public struct GameItem
    {
        public string Title { get; }
        public string Key { get; }
        public string Url { get; }

        public GameItem(string key, string title, string url)
        {
            Title = title;
            Url = url;
            Key = key;
        }
    }
}
