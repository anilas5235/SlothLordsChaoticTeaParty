using System.IO;
using System.Xml.Serialization;
using UnityEngine;

namespace Project.Scripts.General
{
    public class SaveSystem : Singleton<SaveSystem>
    {
        private SaveData activeSave;

        protected override void Awake()
        {
            base.Awake();
            if(activeSave == null) Load();
        }

        private void OnApplicationQuit()
        {
            Save();
        }

        public void Save()
        {
            string path = Application.persistentDataPath + "/" + "PlayerSave.hotf";

            var serializer = new XmlSerializer(typeof(SaveData));
            var stream = new FileStream(path, FileMode.Create);
            serializer.Serialize(stream, activeSave);
            stream.Close();

            Debug.Log("Save was successful");
        }

        public void Load()
        {
            string path = Application.persistentDataPath + "/" + "PlayerSave.hotf";
            print(path);

            if (File.Exists(path))
            {
                var serializer = new XmlSerializer(typeof(SaveData));
                var stream = new FileStream(path, FileMode.Open);
                activeSave = serializer.Deserialize(stream) as SaveData;
                stream.Close();

                Debug.Log("Load was successful");
            }
            else
            {
                activeSave = new SaveData();

                for (int i = 0; i < activeSave.audioOptions.Length; i++)
                {
                    activeSave.audioOptions[i] = -2f;
                }

                for (int i = 0; i < activeSave.levelsUnlocked.Length; i++)
                {
                    activeSave.levelsUnlocked[i] = false;
                }
                activeSave.levelsUnlocked[0] = true;
            }
        }

        public void DeleteSaveData()
        {
            string path = Application.persistentDataPath + "/" + "PlayerSave.save";

            if (File.Exists(path)) File.Delete(path);
        }

        public SaveData GetActiveSave() { return activeSave; }
    }

    [System.Serializable]
    public class SaveData
    {
        public bool firstTimeTutorialDone;
        public float[] audioOptions = new float[10];
        public bool[] levelsUnlocked = new bool[10];
        public float[] highScoresForLevels = new float[10];
        public bool[] unlockedSilverCrowns = new bool[10];
        public bool[] unlockedGoldCrowns = new bool[10];
        public bool[] unlockedEndings = new bool[4];
    }
}