using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DemoSecurityApp.Models
{
    public static class SessionManager
    {
        public static T getSession<T>(string sessionName)
        {
            return (T)System.Web.HttpContext.Current.Session[sessionName];
        }

        public static void setSession<T>(string sessionName, T value)
        {
            System.Web.HttpContext.Current.Session[sessionName] = value;
        }

        public static bool checkIfSessionExist(string sessionName)
        {
            return (System.Web.HttpContext.Current.Session[sessionName] != null) ? true : false;
        }
    }
}