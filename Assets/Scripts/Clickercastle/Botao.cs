using UnityEngine;

public class Botao : MonoBehaviour
{
    public GameObject porta; 
    public GameObject SR;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica se quem tocou no botão foi o jogador
        if (collision.CompareTag("Player") || collision.CompareTag("Cursor"))
        {
            // Faz a porta desaparecer
            porta.SetActive(false); 
            SR.SetActive(false);
        }
    }
}