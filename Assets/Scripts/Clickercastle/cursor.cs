using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class cursor : MonoBehaviour
{
    private Collider2D colisor2d;
    private Rigidbody2D corpo2d;
    private bool estaTangivel = false;
    private Coroutine rotinaTangibilidade;

    void Start()
    {
        corpo2d = GetComponent<Rigidbody2D>();

        // pega o collider2d do objeto
        colisor2d = GetComponent<Collider2D>();

        // faça em que o objeto comece intangível
        if (colisor2d != null)
        {
            colisor2d.enabled = false;
        }
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !estaTangivel)
        {
            // Inicia o temporizador
            rotinaTangibilidade = StartCoroutine(AtivarTangibilidade());
        }
    }

   private void OnCollisionEnter2D(Collision2D collision)
{
    // Verifica se quem colidiu é o jogador e se ele está acima da plataforma
    if (collision.gameObject.CompareTag("Player"))
    {
        // Garante que o jogador está pisando em cima (normal apontando para baixo)
        if (collision.GetContact(0).normal.y < -0.5f)
        {
            collision.transform.SetParent(this.transform);
        }
    }
}

private void OnCollisionExit2D(Collision2D collision)
{
    if (collision.gameObject.CompareTag("Player"))
    {
        collision.transform.SetParent(null);
    }
}
    void FixedUpdate()
    {
        // converte posição do mouse para o mundo do jogo
        Vector3 posicaoMouseMundo = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // mantem o objeto na mesma profundidade
        posicaoMouseMundo.z = transform.position.z;

        // Move o objeto para o mouse
        corpo2d.MovePosition(new Vector2(posicaoMouseMundo.x, posicaoMouseMundo.y));
    }

    IEnumerator AtivarTangibilidade()
    {
        // marca que o objeto esta tangivel
        estaTangivel = true;
        
        // Liga a colisão (fica tangível)
        colisor2d.enabled = true;

        // espera 0.5 segundos
        yield return new WaitForSeconds(0.5f);

        // Desliga a colisão 
        colisor2d.enabled = false;
        
        // Libera para o jogador poder clicar de novo
        estaTangivel = false;
        rotinaTangibilidade = null;
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player") &&
            collision.collider.IsTouchingLayers(LayerMask.GetMask("ground")))
        {
            colisor2d.enabled = false;
            estaTangivel = false;

            if (rotinaTangibilidade != null)
            {
                StopCoroutine(rotinaTangibilidade);
                rotinaTangibilidade = null;
            }
        }
    }

}
