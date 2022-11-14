namespace NDimens.Minecraft.FreeLauncher; 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using dotMCLauncher.Core;

using Launcher.Forms;

using NDimens.Minecraft.FreeLauncher.Core.Data;
using NDimens.Minecraft.FreeLauncher.Presenters;

internal class FormFactory {
    private readonly IUsersRepository _usersRepository;
    private readonly IProfilesRepository _profilesRepository;

    public FormFactory(IUsersRepository usersRepository, IProfilesRepository profilesRepository) {
        _usersRepository = usersRepository;
        _profilesRepository = profilesRepository;
    }

    internal UserManagerForm CreateUserManagerForm() {
        var presenter = new UserManagerFormPresenter(_usersRepository);
        var form = new UserManagerForm(presenter);
        return form;
    }

    internal ProfileManagerForm CreateNewProfileManagerForm() {
        var presenter = new CreateProfileFormPresenter(_profilesRepository);
        var form = new ProfileManagerForm(presenter);
        return form;
    }

    internal ProfileManagerForm CreateEditProfileManagerForm(Profile profile) {
        var presenter = new EditProfileFormPresenter(_profilesRepository, profile);
        var form = new ProfileManagerForm(presenter);
        return form;
    }
}
