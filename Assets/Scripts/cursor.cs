using System.Collections;
using UnityEngine;

public class cursor : MonoBehaviour
{
    private Collider2D colisor2d;
    private bool estaTangivel = false;

    void Start()
    {
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
        // converte posição do mouse para o mundo do jogo
        Vector3 posicaoMouseMundo = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // mantem o objeto na mesma profundidade
        posicaoMouseMundo.z = transform.position.z;

        // Move o objeto para o mouse
        transform.position = posicaoMouseMundo;

        if (Input.GetMouseButtonDown(0) && !estaTangivel)
        {
            // Inicia o temporizador
            StartCoroutine(AtivarTangibilidade());
        }
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
    }
}
