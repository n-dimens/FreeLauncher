namespace dotMCLauncher.Core {
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Newtonsoft.Json;

    public class UsersRepository {
        // applicationContext.LauncherUsers == ConnectionString - информация об источнике данных, куда "подключаться"
        // т.е. сюда нужно передавать "БД" = корневой папке, а где в этой папке нужная "таблица"=файл - репозиторий должен знать сам.
        // Ну или нужно отдельное метаописание "БД" как папки с файлами
        private readonly GameFileStructure _gameFiles;

        public UsersRepository(GameFileStructure gameFiles) {
            _gameFiles = gameFiles;
        }

        public UserManager Read() {
            if (File.Exists(_gameFiles.LauncherUsers)) {
                var um = JsonConvert.DeserializeObject<UserManager>(File.ReadAllText(_gameFiles.LauncherUsers));
                if (um != null) {
                    return um;
                }
            }

            var newUserManager = new UserManager();
            Save(newUserManager);
            return newUserManager;
        }

        public void Save(UserManager userManager) {
            var json = JsonConvert.SerializeObject(userManager, Formatting.Indented,
                    new JsonSerializerSettings {
                        NullValueHandling = NullValueHandling.Ignore
                    }
            );

            File.WriteAllText(_gameFiles.LauncherUsers, json);
        }
    }
}
