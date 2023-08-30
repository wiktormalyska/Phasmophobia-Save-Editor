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
        //Paths
        string appDataFolder;
        string gameSavePath;
        string programSavePath;
        public string decryptedPath;
        string encryptedPath;

        //Data
        byte[] encryptedData;
        byte[] decryptedData;

        //Json

        //Utils
        Cryptography cryptography;
        public DataManager() {
            appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\..\\LocalLow";
            gameSavePath = Path.Combine(appDataFolder, "Kinetic Games", "Phasmophobia", "SaveFile.txt");
            programSavePath = Path.Combine(appDataFolder, "Wiktor Malyska", "PhasmophobiaSaveEditor");
            decryptedPath = Path.Combine(programSavePath, "decryptedSave.txt");
            encryptedPath = Path.Combine(programSavePath, "encryptedSave.txt");

            cryptography = new Cryptography();

        }
        public void init() 
        {
            initFiles();
            encryptedData = File.ReadAllBytes(gameSavePath);
            decryptedData = cryptography.decryptFile(encryptedData);
            File.WriteAllBytes(decryptedPath, decryptedData);
        }

        private void initFiles() {
            createAndCleanFile(decryptedPath);
            createAndCleanFile(encryptedPath);
        }

        private static void createAndCleanFile(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            using (File.Create(path)) { }
        }

    }

}
