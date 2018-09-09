using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace ANPositive
{
    public static class HMTLHelperExtensions
    {
        public static string IsSelected(this HtmlHelper html, string controller = null, string action = null, string cssClass = "active")
        {
            string currentAction = html.ViewContext.RouteData.Values["action"].ToString();
            string currentController = html.ViewContext.RouteData.Values["controller"].ToString();

            if (String.IsNullOrEmpty(controller))
                controller = currentController;

            if (String.IsNullOrEmpty(action))
                action = currentAction;

            return controller == currentController && action == currentAction ? cssClass : String.Empty;
        }

        public static string PageClass(this HtmlHelper html)
        {
            string currentAction = (string)html.ViewContext.RouteData.Values["action"];
            return currentAction;
        }

        public static string GetFirstCharacters(this HtmlHelper html, string htmlString, int length)
        {
            if (htmlString == null)
                return string.Empty;

            if (htmlString.Length < length)
                return htmlString;

            var separateRegex = new Regex("([^>][^<>]*[^<])|[\\S]{1}");
            var tagsRegex = new Regex("^<[^>]+>$");
            var matches = separateRegex.Matches(htmlString);
            var counter = 0;
            var sb = new StringBuilder();
            for (var i = 0; i < matches.Count; i++)
            {
                var m = matches[i].Value;
                if (tagsRegex.IsMatch(m))
                {
                    sb.Append(m);
                }
                else
                {
                    var lengthToCut = length - counter;

                    var sub = lengthToCut >= m.Length
                        ? m
                        : m.Substring(0, lengthToCut);

                    counter += sub.Length;
                    sb.Append(sub);
                }
            }

            return sb.ToString();
        }
    }
}
 