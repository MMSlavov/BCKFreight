﻿using System;
using System.Linq;

using Microsoft.AspNetCore.Mvc;

public static class NavigationIndicatorHelper
{
    public static string MakeActiveClass(this IUrlHelper urlHelper, string controller, string action, string result)
    {
        try
        {
            if (urlHelper.ActionContext.RouteData.Values.ContainsKey("page"))
            {
                return null;
            }

            string controllerName = urlHelper.ActionContext.RouteData.Values["controller"].ToString();
            string methodName = urlHelper.ActionContext.RouteData.Values["action"].ToString();
            if (string.IsNullOrEmpty(controllerName))
            {
                return null;
            }

            if (controllerName.Equals(controller, StringComparison.OrdinalIgnoreCase))
            {
                if (action == null || methodName.Equals(action, StringComparison.OrdinalIgnoreCase))
                {
                    return result;
                }
            }

            return null;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public static string MakeActiveClass(this IUrlHelper urlHelper, string[] controllers, string result)
    {
        try
        {
            if (urlHelper.ActionContext.RouteData.Values.Count() == 2)
            {
                return null;
            }

            string controllerName = urlHelper.ActionContext.RouteData.Values["controller"].ToString();
            string methodName = urlHelper.ActionContext.RouteData.Values["action"].ToString();
            if (string.IsNullOrEmpty(controllerName))
            {
                return null;
            }

            if (controllers.Any(c => controllerName.Equals(c, StringComparison.OrdinalIgnoreCase)))
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
