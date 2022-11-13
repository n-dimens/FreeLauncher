namespace NDimens.Minecraft.FreeLauncher.Core.Data; 

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using dotMCLauncher.Core;

public interface IUsersRepository {
    ImmutableList<string> GetUserList();

    UserManager Read();

    User Find(string userName);

    void Add(User user);

    void Save(UserManager userManager);
}
