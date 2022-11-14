namespace NDimens.Minecraft.FreeLauncher.Core.Data;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NDimens.Minecraft.FreeLauncher.Core;

public interface IProfilesRepository {
    ProfileManager Read();

    bool IsProfileExist(string profileName);

    void Save(ProfileManager profileManager);
}
