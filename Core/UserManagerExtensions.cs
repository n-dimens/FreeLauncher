namespace dotMCLauncher.Core {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class UserManagerExtensions {
        public static void AddUser(this UserManager userManager, User user) {
            if (userManager.Users.ContainsKey(user.Username)) {
                userManager.Users[user.Username] = user;
            }
            else {
                userManager.Users.Add(user.Username, user);
            }
        }
    }
}
