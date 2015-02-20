using System;
namespace DemoSecurityApp.Models
{
    interface IService
    {
        System.Collections.Generic.IEnumerable<User> GetAllUsers();
        User GetUserAtIndex(int uID);
        User ValidateUser(LoginModel loginModel);
    }
}
