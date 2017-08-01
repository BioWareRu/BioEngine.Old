namespace BioEngine.API.Extensions
{
    public static class StringExtensions
    {
        public static string ToCamelCase( this string s)
        {
            if (string.IsNullOrEmpty(s))
                return s;

            if (!char.IsUpper(s[0]))
                return s;

            var camelCase = char.ToLower(s[0]).ToString();
            if (s.Length > 1)
                camelCase += s.Substring(1);

            return camelCase;
        }
    }
}
