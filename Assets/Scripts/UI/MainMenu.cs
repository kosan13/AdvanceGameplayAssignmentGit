using Events;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class MainMenu : GameEventBehaviour
    {
        private void OnEnable() => EventHandler.Main.PushEvent(this);
        private void OnDisable() => EventHandler.Main.RemoveEvent(this);
        public override bool IsDone() => false;
        public void OnStartGame()
        {
            EventHandler.Main.RemoveEvent(this);
            SceneManager.LoadScene("Level", LoadSceneMode.Single);
        }
        public void OnQuit() => Application.Quit();
    }
}