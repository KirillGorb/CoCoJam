using UnityEngine.SceneManagement;

namespace CodeScripts.Scene
{
    public class SceneController
    {
        public  int ThisIdScene => SceneManager.GetActiveScene().buildIndex;
        public  void SetScene(int idScene) => SceneManager.LoadScene(idScene);
        public  void SetScene(string nameScene) => SceneManager.LoadScene(nameScene);
        public  void ResetScene() => SetScene(ThisIdScene);
    }
}