using Event.BaseEventClass;
using Game;
using Game.UnitClasses;
using SaveSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using EventHandler = Event.EventHandler;

namespace UI
{
    public class PauseMenu : GameEventBehaviour
    {
        private const string Path = "Prefab/UI/PauseMenu";
        
        #region Properties
        public static PauseMenu Instance { get; private set; }
        
        #endregion
        
        private void OnEnable() => Time.timeScale = 0;
        private void OnDisable() => Time.timeScale = 1;
        private void OnDestroy() => EventHandler.Main.RemoveEvent(this);
        public override void OnBegin(bool bFirstTime) => gameObject.SetActive(bFirstTime || gameObject.activeSelf);
        public override bool IsDone() => false;
        public void OnResume() => gameObject.SetActive(false);
        public void OnSave()
        {
            BlobDivisionMaze instance = BlobDivisionMaze.Instance;
            SaveSystem.SaveSystem.SaveProgression(new SaveFileData(instance.Tilemap,instance.TileLevel, instance.MeshFilter.mesh,Level.TurnOrder, PlayerCharacter.Instance));
        }
        public void OnBack()
        {
            EventHandler.RemoveAllEvent(EventHandler.Main);
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }


        public static GameObject CreatPauseMenu() => (GameObject)Resources.Load(Path);
        public static GameObject CreatAndInstantiatePauseMenu()
        {
            GameObject gameObject = Instantiate(CreatPauseMenu()) ; 
            Instance = gameObject.GetComponent<PauseMenu>();
            gameObject.SetActive(false);
            EventHandler.Main.PushEvent(Instance);
            return gameObject;
        }
    }
}