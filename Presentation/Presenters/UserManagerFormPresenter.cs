namespace NDimens.Minecraft.FreeLauncher.Presenters;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NDimens.Minecraft.FreeLauncher.Core;
using NDimens.Minecraft.FreeLauncher.Core.Data;

public class UserManagerFormPresenter {
    private readonly IUsersRepository _usersRepository;

    public UserManagerFormPresenter(IUsersRepository usersRepository) {
        _usersRepository = usersRepository;
    }

    public IEnumerable<string> GetUserNames() {
        return _usersRepository.Read().Users.Keys;
    }

    public void AddUser(string userName) {
        _usersRepository.Add(new User {
            Username = userName,
            Type = "offline"
        });
    }

    public void DeleteUser(string userName) {
        var userManager = _usersRepository.Read();
        userManager.Users.Remove(userName);
        _usersRepository.Save(userManager);
    }
}
