using DemoSecurityApp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DemoSecurityApp.Models
{
    public class InMemoryDB
    {
        public ICollection<User> Users
        {
            get
            {
                return User.GetAll();
            }
        }

        public ICollection<Role> Roles
        {
            get
            {
                return Role.GetAll();
            }
        }

        /*
        public ICollection<JunctionUserRole> JunctionUserRoles
        {
            get
            {
                return JunctionUserRole.GetAll();
            }
        }
         */
    }

    public class User
    {
        #region Properties

        public int ID { get; set; }
        public int RoleID { get; set; }
        public string Email { get; set; }
        public string password { get; set; }
        public string FullName { get; set; }
        public bool IsActive { get; set; }

        public Role Role
        {
            get
            {
                return Role.GetById(this.RoleID);
            }
            set
            {
                this.Role = value;
                this.RoleID = value.ID;
            }
        }
        public List<Feature> AllowedFeatures = new List<Feature>();

        #endregion

        #region Methods

        public static ICollection<User> GetAll()
        {
            List<User> userList = new List<User>();
            List<Feature> allFeatures = Feature.GetAll().ToList();

            for (int i = 1; i <= Constants.USERSCOUNT; i++)
            {
                int adminOrUserRole = new Random().Next(1, 2);
                //int precedingNumFeaturesToAssign = new Random().Next(0, Constants.FEATURESCOUNT + 1);

                User uT = new User();
                uT.ID = i;
                uT.RoleID = adminOrUserRole;
                uT.Email = "email_" + i + "@gmail.com";
                uT.FullName = "Name_" + i;
                uT.IsActive = true;

                //uT.AllowedFeatures.AddRange(allFeatures.Where(f => f.ID < precedingNumFeaturesToAssign));

                userList.Add(uT);
            }

            return userList;
        }

        #endregion
    }

    public enum RoleType
    {
        Admin = 1,
        User = 2
    }

    public class Role
    {
        #region Properties

        public int ID { get; set; }
        public string Name { get; set; }

        public IEnumerable<Feature> Features
        {
            get
            {
                var allFeatures = Feature.GetAll();
                if (ID == (int)RoleType.Admin)
                {
                    return allFeatures.Where(f => f.Name.StartsWith("a") || f.Name.StartsWith("b"));
                }
                else
                {
                    return allFeatures.Where(f => f.Name.StartsWith("u") || f.Name.StartsWith("b"));
                }
            }
        }

        #endregion

        #region Methods

        public static ICollection<Role> GetAll()
        {
            List<Role> roleList = new List<Role>();
            Role rT = new Role();
            rT.ID = (int)RoleType.Admin;
            rT.Name = "Admin";
            roleList.Add(rT);

            rT = new Role();
            rT.ID = (int)RoleType.User;
            rT.Name = "User";
            roleList.Add(rT);

            return roleList;
        }

        public static Role GetById(int id)
        {
            return Role.GetAll().Where(r => r.ID == id).FirstOrDefault();
        }

        #endregion
    }

    public class Feature
    {
        #region Properties

        public int ID { get; set; }
        public string Name { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }

        #endregion

        #region Methods

        public static ICollection<Feature> GetAll()
        {
            List<Feature> featureList = new List<Feature>();
            Feature rT = new Feature();
            rT.ID = 1;
            rT.Name = "editU";
            rT.ControllerName = "home";
            rT.ActionName = "edit";
            featureList.Add(rT);

            rT = new Feature();
            rT.ID = 2;
            rT.Name = "detailsU";
            rT.ControllerName = "home";
            rT.ActionName = "details";
            featureList.Add(rT);

            rT = new Feature();
            rT.ID = 3;
            rT.Name = "deleteU";
            rT.ControllerName = "home";
            rT.ActionName = "delete";
            featureList.Add(rT);

            return featureList;
        }

        public static Role GetById(int id)
        {
            return Role.GetAll().Where(r => r.ID == id).FirstOrDefault();
        }

        #endregion
    }

    /* JunctionUserRole
    public class JunctionUserRole
    {
        #region Properties

        public int ID { get; set; }
        public int UserID { get; set; }
        public int RoleID { get; set; }

        #endregion

        #region Methods

        public static ICollection<JunctionUserRole> GetAll()
        {
            List<JunctionUserRole> jURList = new List<JunctionUserRole>();

            int i = 1;
            foreach (var item in User.GetAll())
            {
                int adminOrUserRole = new Random().Next(1, 2);
                JunctionUserRole jURT = new JunctionUserRole();
                jURT.ID = i;
                jURT.UserID = item.ID;
                jURT.RoleID = adminOrUserRole;

                jURList.Add(jURT);
                i++;
            }

            return jURList;
        }

        #endregion
    }
    */
}