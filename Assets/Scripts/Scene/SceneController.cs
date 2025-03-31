using UnityEngine.SceneManagement;

namespace CodeScripts.Scene
{
    public static class SceneController
    {
        private static int ThisIdScene => SceneManager.GetActiveScene().buildIndex;
        public static void SetScene(int idScene = 0) => SceneManager.LoadScene(idScene);
        public static void ResetScene() => SetScene(ThisIdScene);
    }
}