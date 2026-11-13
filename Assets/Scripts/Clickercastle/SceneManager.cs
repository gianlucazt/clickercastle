using UnityEngine;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

namespace ClickerCastle
{
    public class SceneManager : MonoBehaviour
    {

        public void proximaScene()
        {
            UnitySceneManager.LoadSceneAsync(UnitySceneManager.GetActiveScene().buildIndex + 1);
        }

        public void carregarscene(string sceneName)
        {
            UnitySceneManager.LoadSceneAsync(sceneName);
        }
    }
}
