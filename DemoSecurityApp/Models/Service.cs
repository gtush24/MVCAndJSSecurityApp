using DemoSecurityApp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DemoSecurityApp.Models
{
    public class Service : DemoSecurityApp.Models.IService
    {
        InMemoryDB db = new InMemoryDB();

        public IEnumerable<User> GetAllUsers()
        {
            return db.Users;
        }

        public User GetUserAtIndex(int uID)
        {
            if (uID > Constants.USERSCOUNT)
                uID = 1;

            return db.Users.ElementAt(--uID);
        }

        public User ValidateUser(LoginModel loginModel)
        {
            return db.Users.Where(u => u.Email == loginModel.UserName
                && u.password == loginModel.Password
                && u.IsActive == true
                ).FirstOrDefault();
        }

        internal IEnumerable<Feature> SetFeaturesToLoginUser()
        {
            int precedingNumFeaturesToAssign = new Random().Next(0, Constants.FEATURESCOUNT + 1);

            return (Feature.GetAll().Where(f => f.ID <= precedingNumFeaturesToAssign));
        }

        internal List<Feature> GetAccessibleFeaturesForUser(User loginContact)
        {
            return (db.Users.Where(u => u.ID == loginContact.ID).FirstOrDefault().AllowedFeatures);
        }

        internal static List<string> GetFeatureControlID(List<Feature> list)
        {
            return list.Select(l => l.Name).ToList();
        }

        internal static bool isFeaturePresentInList(List<Feature> allFeatureList, string controllerName, string ActionName)
        {
            return allFeatureList.Where(f => f.ControllerName.Equals(controllerName, StringComparison.OrdinalIgnoreCase) && f.ActionName.Equals(ActionName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault() != null;
        }
    }
}