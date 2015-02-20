using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DemoSecurityApp.Models;

namespace DemoSecurityApp.Filters
{
    public class PermissionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            /*For Thumbnail generating action*/
            //if (filterContext.ActionDescriptor.GetCustomAttributes(typeof(DontValidateAttribute),true).Any())
            //{
            //    // The controller action is decorated with the [DontValidate]
            //    // custom attribute => don't do anything.
            //    return;
            //}

            base.OnActionExecuting(filterContext);

            HttpRequestBase request = filterContext.HttpContext.Request;
            HttpResponseBase response = filterContext.HttpContext.Response;

            if ((request.AcceptTypes.Contains("application/json")) == false)
            {
                if (request.IsAjaxRequest())
                {
                    #region Preventing caching of ajax request in IE browser
                    response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
                    response.Cache.SetValidUntilExpires(false);
                    response.Cache.SetCacheability(HttpCacheability.NoCache);
                    response.Cache.SetNoStore();
                    #endregion
                }

                string currentActionName = filterContext.ActionDescriptor.ActionName;
                string currentControllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
                int eventInstanceID;
                if (int.TryParse(Convert.ToString(filterContext.RouteData.Values["id"]), out eventInstanceID))
                {
                    // It was assigned.
                }

                //get LoggedInUser Permissions
                PermissionManager userPermissions = PermissionManager.getPermissions();

                //All Features are allowed for SuperAdmin - disablePermissioning
                if (userPermissions != null)
                {
                    //For logout page
                    filterContext.Controller.ViewBag.LoggedInAdministrator = PermissionManager.GetLoggedInUser();

                    if (PermissionManager.enablePermissioningSystem == true)
                    {
                        //For getting accessible features per event for user  - Used by permission checker javascript
                        filterContext.Controller.ViewBag.EventInstanceIDForPermission = eventInstanceID;

                        //Not all actions are feature in the application
                        bool isCurrentActionAFeature = Service.isFeaturePresentInList(userPermissions.allFeatures, currentControllerName, currentActionName);

                        if (isCurrentActionAFeature)
                        {
                            bool hasPermission = false;

                            hasPermission = Service.isFeaturePresentInList(userPermissions.accessibleFeatures, currentControllerName, currentActionName);

                            if (!hasPermission)
                            {
                                //return 'not authorized' content
                                filterContext.Result = new ViewResult
                                {
                                    ViewName = "~/Views/shared/unauthorizedactivity.cshtml"
                                };
                            }
                        }
                    }
                }
                //Redirect to login Page
                else
                {
                    filterContext.Controller.TempData["lgnFailedMessage"] = "Please login to continue.";
                    filterContext.Result = new RedirectToRouteResult(
                                new System.Web.Routing.RouteValueDictionary { { "controller", "home" }, { "action", "login" }, { "Area", "" } });
                }
            }
        }
    }
}