using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

public static class NavigationIndicatorHelper
{
    public static string MakeActiveClass(this IUrlHelper urlHelper, string[] controllers, string[] actions, string result)
    {
        try
        {
            string controllerName = urlHelper.ActionContext.RouteData.Values["controller"].ToString();
            string methodName = urlHelper.ActionContext.RouteData.Values["action"].ToString();
            if (string.IsNullOrEmpty(controllerName))
            {
                return null;
            }

            if (controllers.Any(c => c.Equals(controllerName, StringComparison.OrdinalIgnoreCase)) &&
                actions.Any(a => a.Equals(methodName, StringComparison.OrdinalIgnoreCase)))
            {
                return result;
            }

            return null;
        }
        catch (Exception)
        {
            return null;
        }
    }
}
