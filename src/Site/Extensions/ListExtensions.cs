using Microsoft.AspNetCore.Mvc.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Huyn.PluralNet;
using System.Globalization;

namespace BioEngine.Site.Extensions
{
    public static class ListExtensions
    {
        private static Random rng = new Random();

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static string Plural(this IViewLocalizer localizer, string key, int number)
        {
            var provider = Huyn.PluralNet.Utils.PluralHelper.GetPluralChooser(CultureInfo.CurrentCulture);
            var pluralType = provider.ComputePlural(number);
            string selectedSentence = null;
            try
            {
                switch (pluralType)
                {
                    case PluralTypeEnum.ZERO:
                        selectedSentence = localizer.GetString(key + "_Zero");
                        break;
                    case PluralTypeEnum.ONE:
                        selectedSentence = localizer.GetString(key + "_One");
                        break;
                    case PluralTypeEnum.OTHER:
                        selectedSentence = localizer.GetString(key + "_Other");
                        break;
                    case PluralTypeEnum.TWO:
                        selectedSentence = localizer.GetString(key + "_Two");
                        break;
                    case PluralTypeEnum.FEW:
                        selectedSentence = localizer.GetString(key + "_Few");
                        break;
                    case PluralTypeEnum.MANY:
                        selectedSentence = localizer.GetString(key + "_Many");
                        break;
                }
            }
            catch { }
            return selectedSentence != null ? string.Format(selectedSentence, number) : "";

        }
    }
}
