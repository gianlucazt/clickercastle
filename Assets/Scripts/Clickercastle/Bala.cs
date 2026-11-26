using UnityEngine;

public class Bala : MonoBehaviour
{
    public float velocidade = 10f;
    public float tempoDeVida = 3f;

    void Start()
    {
        Destroy(gameObject, tempoDeVida);
    }

    void Update()
    {
        transform.Translate(Vector2.right * velocidade * Time.deltaTime);
    }

    // Trocado de OnTriggerEnter2D para OnCollisionEnter2D
    void OnCollisionEnter2D(Collision2D colisao)
    {
        // Destrói a bala ao bater no jogador, na parede ou em qualquer colisor sólido
        Destroy(gameObject);
    }
}