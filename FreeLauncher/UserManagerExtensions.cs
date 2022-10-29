namespace FreeLauncher {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal static class UserManagerExtensions {
        public static void AddUser(this UserManager userManager, User user) {
            if (userManager.Accounts.ContainsKey(user.Username)) {
                userManager.Accounts[user.Username] = user;
            }
            else {
                userManager.Accounts.Add(user.Username, user);
            }
        }
    }
}
