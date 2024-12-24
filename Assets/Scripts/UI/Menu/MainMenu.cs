using UnityEngine;
using UnityEngine.SceneManagement;

namespace Digital_Subcurrent
{
    public class MainMenu : MonoBehaviour
    {
        private SceneManagerCUS sceneManagerCUS;

        void Start()
        {
            sceneManagerCUS = SceneManagerCUS.Instance;
        }

        public void PlayGame()
        {
            sceneManagerCUS.ChangeScene("IF");
        }

        public void QuitGame()
        {
            Debug.Log("Quit");
            Application.Quit();
        }
    }
}
