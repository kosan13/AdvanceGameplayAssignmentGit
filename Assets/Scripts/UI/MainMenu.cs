using Game;
using Librarys.EventSystem;
using Librarys.EventSystem.BaseEventClass;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class MainMenu : GameEventBehaviour
    {
        [SerializeField] private GameObject lodeGameButton;
        private void OnEnable()
        {
            // lodeGameButton.SetActive(SaveDataFileExists);
            EventHandler.Main.PushEvent(this);
        }

        private void OnDisable() => EventHandler.Main.RemoveEvent(this);
        public override bool IsDone() => false;
        public void OnStartGame()
        {
            EventHandler.Main.RemoveEvent(this);
            BlobDivisionMaze.LodeGameBole = false;
            SceneManager.LoadScene("Level", LoadSceneMode.Single);
        }
        public void OnLodeGame()
        {
            // LoadProgression();
            BlobDivisionMaze.LodeGameBole = true;
            EventHandler.Main.RemoveEvent(this);
            SceneManager.LoadScene("Level", LoadSceneMode.Single);
        }
        public void OnQuit() => Application.Quit();
    }
}