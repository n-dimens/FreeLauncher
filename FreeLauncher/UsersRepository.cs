namespace FreeLauncher {
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Newtonsoft.Json;

    internal class UsersRepository {
        // applicationContext.LauncherUsers == ConnectionString - информация об источнике данных, куда "подключаться"
        // т.е. сюда нужно передавать "БД" = корневой папке, а где в этой папке нужная "таблица"=файл - репозиторий должен знать сам.
        // Ну или нужно отдельное метаописание "БД" как папки с файлами
        private readonly ApplicationContext _applicationContext; 

        public UsersRepository(ApplicationContext appContext) {
            _applicationContext = appContext;
        }

        public UserManager Read() {
            return File.Exists(_applicationContext.LauncherUsers)
                    ? JsonConvert.DeserializeObject<UserManager>(File.ReadAllText(_applicationContext.LauncherUsers))
                    : new UserManager();
        }

        public void Save(UserManager userManager) {
            var json = JsonConvert.SerializeObject(userManager, Formatting.Indented,
                    new JsonSerializerSettings {
                        NullValueHandling = NullValueHandling.Ignore
                    });

            File.WriteAllText(_applicationContext.LauncherUsers, json);
        }
    }
}
