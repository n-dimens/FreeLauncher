namespace dotMCLauncher.Core.Auth {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;

    public static class AuthService {
        // TODO: async?
        public static User? Login(string email, string password) {
            var auth = new AuthManager {
                Email = email,
                Password = password
            };

            try {
                auth.Login();
                return new User {
                    Username = email,
                    Type = auth.IsLegacy ? "legacy" : "mojang",
                    AccessToken = auth.AccessToken,
                    SessionToken = auth.SessionToken,
                    Uuid = auth.Uuid,
                    UserProperties = auth.UserProperties
                };
            }
            catch (WebException ex) {
                // TODO: обработка?
                return null;
            }
        }
    }
}
