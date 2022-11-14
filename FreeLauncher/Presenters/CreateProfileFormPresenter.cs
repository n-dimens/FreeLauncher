namespace NDimens.Minecraft.FreeLauncher.Presenters; 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using dotMCLauncher.Core;

using NDimens.Minecraft.FreeLauncher.Core.Data;

internal class CreateProfileFormPresenter {
    protected readonly IProfilesRepository _repository;

    // TODO: VersionsRepository
    public IReadOnlyCollection<string> VersionList = new List<string>() { "1.7.10", "1.7.10-Forge10.13.4.1614-1.7.10" };

    public Profile Profile { get; protected set; }
        
    public CreateProfileFormPresenter(IProfilesRepository repository) {
        Profile = Profile.CreateDefault();
        _repository = repository;
    }

    public virtual string Save() {
        var pm = _repository.Read();
        if (pm.Profiles.ContainsKey(Profile.ProfileName)) {
            return "Профиль с именем " + Profile.ProfileName + " уже существует";
        }

        pm.Profiles.Add(Profile.ProfileName, Profile);
        pm.LastUsedProfile = Profile.ProfileName;
        _repository.Save(pm);
        return string.Empty;
    }
}
