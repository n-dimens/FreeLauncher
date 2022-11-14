namespace NDimens.Minecraft.FreeLauncher; 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IProgressView {
    void UpdateStageText(string text, string methodName = null);

    void SetProgressVisibility(bool b);

    void IncProgressValue();

    void SetProgressValue(int value);

    void SetMaxProgressValue(int value);
}
