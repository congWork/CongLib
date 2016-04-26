using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace CongLib.MvcHelpers
{
    public static class CommonHtmlHelperExtensions
    {
        public static string IsActive(this HtmlHelper html,
                                 string control,
                                 string action)
        {
            var routeData = html.ViewContext.RouteData;

            var routeAction = (string)routeData.Values["action"];
            var routeControl = (string)routeData.Values["controller"];

            // both must match
            var returnActive = control == routeControl &&
                               action == routeAction;

            return returnActive ? "active" : "";
        }
        public static MvcHtmlString MenuLink(this HtmlHelper html, string linkText, string actionName, string controllerName)
        {
            var currentAction = html.ViewContext.RouteData.GetRequiredString("action");
            var currentController = html.ViewContext.RouteData.GetRequiredString("controller");

            var builder = new TagBuilder("li")
            {
                InnerHtml = html.ActionLink(linkText, actionName, controllerName).ToHtmlString()
            };

            if (controllerName == currentController && actionName == currentAction)
                builder.AddCssClass("active");

            return new MvcHtmlString(builder.ToString());
        }
    }
}