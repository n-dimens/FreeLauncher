namespace NDimens.Minecraft.FreeLauncher; 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NDimens.Minecraft.FreeLauncher.Core.Data;
using NDimens.Minecraft.FreeLauncher.Presenters;

internal class FormFactory {
    private readonly IUsersRepository _usersRepository; 

    public FormFactory(IUsersRepository usersRepository) {
        _usersRepository = usersRepository;
    }

    internal UserManagerForm CreateUserManagerForm() {
        var presenter = new UserManagerFormPresenter(_usersRepository);
        var form = new UserManagerForm(presenter);
        return form;
    }   
}
