using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhasmoSaveEditor
{
    internal class DataManager
    {
        string saveData;
        public dynamic PhasmoData;
        Cryptography cryptography;
        public DataManager() {
            string appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\..\\LocalLow";
            string gameSavePath = Path.Combine(appDataFolder, "Kinetic Games", "Phasmophobia", "SaveFile.txt");
            byte[] encryptedSave = File.ReadAllBytes(gameSavePath);
            string tempPath = Path.Combine(appDataFolder, "Wiktor Malyska", "PhasmophobiaSaveEditor");
            cryptography = new Cryptography(tempPath);
            cryptography.decryptFile(encryptedSave);

            if (!Directory.Exists(tempPath))
                Directory.CreateDirectory(tempPath);
            if (!File.Exists(Path.Combine(appDataFolder, "Wiktor Malyska", "PhasmophobiaSaveEditor", "decrypted-SaveFile.txt")))
            {
                File.CreateText(Path.Combine(appDataFolder, "Wiktor Malyska", "PhasmophobiaSaveEditor", "decrypted-SaveFile.txt"));
            }
            if (!File.Exists(Path.Combine(appDataFolder, "Wiktor Malyska", "PhasmophobiaSaveEditor", "encrypted-SaveFile.txt")))
            {
                File.CreateText(Path.Combine(appDataFolder, "Wiktor Malyska", "PhasmophobiaSaveEditor", "encrypted-SaveFile.txt"));
            }

            saveData = File.ReadAllText(Path.Combine(tempPath,"decrypted-SaveFile.txt"));
            PhasmoData = JsonConvert.DeserializeObject(saveData);
        }

        public void save()
        {
            string appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\..\\LocalLow";
            string gameSavePath = Path.Combine(appDataFolder, "Kinetic Games", "Phasmophobia", "SaveFile.txt");
            string tempPath = Path.Combine(appDataFolder, "Wiktor Malyska", "PhasmophobiaSaveEditor");

            saveData = JsonConvert.SerializeObject(saveData, Formatting.Indented);
            byte[] decryptedSave = Encoding.UTF8.GetBytes(saveData);
            cryptography.encryptFile(decryptedSave);
            File.Replace(Path.Combine(tempPath, "encrypted-SaveFile.txt"),gameSavePath,null);
        }

    }

}
