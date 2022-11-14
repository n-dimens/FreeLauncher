namespace NDimens.Minecraft.FreeLauncher.Presenters; 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using dotMCLauncher.Core;

using NDimens.Minecraft.FreeLauncher.Core.Data;

internal class EditProfileFormPresenter : CreateProfileFormPresenter {
    private readonly string _editingProfileName;

    public EditProfileFormPresenter(IProfilesRepository repository, Profile editingProfile) : base(repository) {
        Profile = editingProfile;
        _editingProfileName = editingProfile.ProfileName;
    }

    public override string Save() {
        var pm = _repository.Read();
        var profile = pm.Profiles[_editingProfileName];
        profile.ProfileName = Profile.ProfileName;
        profile.WorkingDirectory = Profile.WorkingDirectory;
        profile.SelectedVersion = Profile.SelectedVersion;
       
        if (_editingProfileName != profile.ProfileName) {
            pm.Profiles.Remove(_editingProfileName);
            pm.Profiles.Add(profile.ProfileName, profile);
        }

        pm.LastUsedProfile = Profile.ProfileName;       
        _repository.Save(pm);
        return string.Empty;
    }
}
