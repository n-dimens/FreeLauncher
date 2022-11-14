namespace NDimens.Minecraft.FreeLauncher.Core.Data;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

public class UsersRepository : IUsersRepository {
    private readonly FileInfo _store;

    public UsersRepository(FileInfo store) {
        _store = store;
        InitStore();
    }

    public ImmutableList<string> GetUserList() {
        return Read().Users.Keys.ToImmutableList();
    }

    public UserManager Read() {
        return JsonConvert.DeserializeObject<UserManager>(File.ReadAllText(_store.FullName));
    }

    public User Find(string userName) {
        return Read().Users[userName];
    }

    public void Add(User user) {
        var um = Read();
        if (um.Users.ContainsKey(user.Username)) {
            um.Users[user.Username] = user;
        }
        else {
            um.Users.Add(user.Username, user);
        }

        if (string.IsNullOrEmpty(um.SelectedUsername)) {
            um.SelectedUsername = user.Username;
        }

        Save(um);
    }

    public void Save(UserManager userManager) {
        var json = JsonConvert.SerializeObject(userManager, Formatting.Indented,
                new JsonSerializerSettings {
                    NullValueHandling = NullValueHandling.Ignore
                }
        );

        File.WriteAllText(_store.FullName, json);
    }

    private void InitStore() {
        if (!_store.Exists) {
            Save(new UserManager());
        }
    }
}
