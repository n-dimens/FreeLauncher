namespace dotMCLauncher.Core.Data {
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;

    using Newtonsoft.Json;
    
    public sealed class VersionsService {
        private readonly ILauncherLogger _logger;
        private readonly GameFileStructure _gameFiles;

        public VersionRegistry VersionRegistry { get; }

        public VersionsService(ILauncherLogger logger, GameFileStructure gameFiles) {
            _logger = logger;
            _gameFiles = gameFiles;
            VersionRegistry = UpdateVersionRegistry();
        }

        public VersionRegistryItem GetVersionInfo(string versionId) {
            return VersionRegistry.Versions.FirstOrDefault(vi => vi.Id == versionId);
        }

        /// <summary>
        /// Обновление локального файла versions.json при сравнении с файлом в облаке
        /// </summary>
        private VersionRegistry UpdateVersionRegistry() {
            _logger.Info("Checking version.json...");
            if (!Directory.Exists(_gameFiles.McVersions)) {
                Directory.CreateDirectory(_gameFiles.McVersions);
            }

            // Скачиваем новый файл
            var jsonVersionList = new WebClient().DownloadString(new Uri(GameFileStructure.VersionsFileUrl));
            var newVersionsData = JsonConvert.DeserializeObject<VersionRegistry>(jsonVersionList);

            // Если локального файла не существует, сохраняем и выходим
            if (!File.Exists(_gameFiles.McVersionsFile)) {
                File.WriteAllText(_gameFiles.McVersionsFile, jsonVersionList);
                _logger.Info("File downloaded and saved.");
                return newVersionsData;
            }

            // Если локальный файл существует, сравниваем со скаченным
            string remoteSnapshotVersion = newVersionsData.Latest.Snapshot;
            string remoteReleaseVersion = newVersionsData.Latest.Release;
            _logger.Info("Latest snapshot: " + remoteSnapshotVersion);
            _logger.Info("Latest release: " + remoteReleaseVersion);

            var currentVersions = JsonConvert.DeserializeObject<VersionRegistry>(File.ReadAllText(_gameFiles.McVersionsFile));
            string localSnapshotVersion = currentVersions.Latest.Snapshot;
            string localReleaseVersion = currentVersions.Latest.Release;

            bool isVersionsCountEqual = newVersionsData.Versions.Count == currentVersions.Versions.Count;
            bool isEqualVersions = remoteReleaseVersion == localReleaseVersion && remoteSnapshotVersion == localSnapshotVersion;
            _logger.Info($"Local versions: {currentVersions.Versions.Count}. Remote versions: {newVersionsData.Versions.Count}");

            if (isVersionsCountEqual && isEqualVersions) {
                // Изменений нет, выходим
                _logger.Info("No update found.");
                return currentVersions;
            }

            // Найдены изменения, обновляем локальный файл
            _logger.Info("Writting new list... ");
            File.WriteAllText(_gameFiles.McVersionsFile, jsonVersionList);
            return newVersionsData;
        }
    }
}
