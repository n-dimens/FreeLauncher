namespace NDimens.Minecraft.FreeLauncher.Core.Data;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NDimens.Minecraft.FreeLauncher.Core;

using Newtonsoft.Json;

public class ProfilesRepository : IProfilesRepository {
    private readonly FileInfo _store;

    public ProfilesRepository(FileInfo store) {
        _store = store;
        InitStore();
    }

    public bool IsProfileExist(string profileName) {
        return Read().Profiles.ContainsKey(profileName);
    }

    public ProfileManager Read() {
        return JsonConvert.DeserializeObject<ProfileManager>(File.ReadAllText(_store.FullName));
    }

    public void Save(ProfileManager profileManager) {
        var json = JsonConvert.SerializeObject(profileManager, Formatting.Indented,
                new JsonSerializerSettings {
                    NullValueHandling = NullValueHandling.Ignore
                }
        );

        File.WriteAllText(_store.FullName, json);
    }

    private void InitStore() {
        if (!_store.Exists) {
            Save(ProfileManager.Default());
        }
    }
}
