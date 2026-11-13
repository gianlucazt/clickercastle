using UnityEngine;
using UnityEngine.SceneManagement;

public class FimDaFase : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            int proximaCena = SceneManager.GetActiveScene().buildIndex + 1;
            if (proximaCena < SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadScene(proximaCena);
            }
            else
            {
                Debug.LogWarning("Não existe uma próxima cena habilitada no Build Settings.");
            }
        }
    }
}
