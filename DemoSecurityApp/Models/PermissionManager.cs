using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DemoSecurityApp.Models
{
    public class PermissionManager
    {
        static readonly string PERMISSION_SESSIONNAME = "UserPermissions";
        User _loggedInUserContact;
        Role _assignedRole = new Role();
        List<Feature> _accessibleFeatures = new List<Feature>();
        List<Feature> _allFeatures = new List<Feature>();

        public static bool enablePermissioningSystem
        {
            get { return true; }
        }

        public User loggedInUserContact
        {
            get { return _loggedInUserContact; }
            set { _loggedInUserContact = value; }
        }

        public Role assignedRole
        {
            get { return _assignedRole; }
            set { _assignedRole = value; }
        }

        public List<Feature> allFeatures
        {
            get { return Feature.GetAll().ToList(); }
            set { _allFeatures = value; }
        }

        public List<Feature> accessibleFeatures
        {
            get { return _accessibleFeatures; }
            set { _accessibleFeatures = value; }
        }

        public bool IsInAdminRole
        {
            get
            {
                return (assignedRole.ID == (int)RoleType.Admin);
            }
        }

        public bool IsInUserRole
        {
            get
            {
                return (assignedRole.ID == (int)RoleType.User);
            }
        }

        public RoleType GetUserRole()
        {
            RoleType uRole = RoleType.User;
            if (IsInAdminRole)
                uRole = RoleType.Admin;
            else if (IsInUserRole)
                uRole = RoleType.User;

            return uRole;
        }

        public static PermissionManager getPermissions()
        {
            PermissionManager userPermissions = SessionManager.getSession<PermissionManager>(PERMISSION_SESSIONNAME);
            return userPermissions;
        }

        public static void setPermissions(User loggedInUser)
        {
            PermissionManager userPermissions = new PermissionManager();
            Service service = new Service();
            User loginContact = loggedInUser;

            userPermissions.loggedInUserContact = loginContact;
            userPermissions.assignedRole = loginContact.Role;
            //userPermissions.accessibleFeatures = service.GetAccessibleFeaturesForUser(loginContact);
            userPermissions.accessibleFeatures = service.SetFeaturesToLoginUser().ToList();

            SessionManager.setSession<PermissionManager>(PERMISSION_SESSIONNAME, userPermissions);
        }

        public static User GetLoggedInUser()
        {
            PermissionManager userPermissions = PermissionManager.getPermissions();
            return userPermissions != null ? userPermissions.loggedInUserContact : null;
        }

        public static List<string> getAccessibleFeatureControlID()
        {
            PermissionManager userPermissions = PermissionManager.getPermissions();
            return Service.GetFeatureControlID(userPermissions != null ? userPermissions.accessibleFeatures : new List<Feature>());
        }

        public static void logout()
        {
            SessionManager.setSession<PermissionManager>(PERMISSION_SESSIONNAME, null);
        }
    }

    public class JsonPermissionManager
    {
        bool _enablePerm;
        List<string> _controlIDS = new List<string>();

        public bool enablePerm
        {
            get { return _enablePerm; }
            set { _enablePerm = value; }
        }
        public List<string> controlIDS
        {
            get { return _controlIDS; }
            set { _controlIDS = value; }
        }
    }
}