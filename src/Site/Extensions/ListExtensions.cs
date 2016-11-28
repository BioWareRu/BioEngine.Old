using System;
using System.Collections.Generic;
using System.Globalization;
using Huyn.PluralNet;
using Huyn.PluralNet.Utils;
using Microsoft.AspNetCore.Mvc.Localization;

namespace BioEngine.Site.Extensions
{
    public static class ListExtensions
    {
        private static readonly Random Rng = new Random();

        public static void Shuffle<T>(this IList<T> list)
        {
            var n = list.Count;
            while (n > 1)
            {
                n--;
                var k = Rng.Next(n + 1);
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static string Plural(this IViewLocalizer localizer, string key, int number)
        {
            var provider = PluralHelper.GetPluralChooser(CultureInfo.CurrentCulture);
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
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            catch
            {
                // ignored
            }
            return selectedSentence != null ? string.Format(selectedSentence, number) : "";
        }
    }
}