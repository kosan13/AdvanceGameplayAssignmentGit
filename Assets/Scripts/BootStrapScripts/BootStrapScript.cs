using UnityEngine;
using UnityEngine.SceneManagement;

namespace BootStrapScripts
{
    public class BootStrapScript : MonoBehaviour
    {
        [SerializeField] private BootStrapMonoBehaviour[] bootStrapMonoBehaviours;
        private void Awake()
        {
            foreach (BootStrapMonoBehaviour bootStrapMonoBehaviour in bootStrapMonoBehaviours) bootStrapMonoBehaviour.Init(); 
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }
    }
}