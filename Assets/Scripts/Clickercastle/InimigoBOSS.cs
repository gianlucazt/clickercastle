using System.Collections;
using UnityEngine;

public class InimigoBOSS : MonoBehaviour
{
    public enum BossState { Mirar, Investida, Atordoado, Lancado }
    public BossState currentState = BossState.Mirar;

    public Transform player;

    [Space]
    public float tempoMirando = 1.5f;
    public float velocidadeInvestida = 15f;
    public float duracaoInvestida = 2.5f;
    public float tempoAtordoado = 2.5f;

    [Space]
    public float velocidadeLancamento = 12f;
    public float duracaoLancamento = 0.4f;

    private Rigidbody2D rb;
    private Coroutine cicloAtual;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cicloAtual = StartCoroutine(CicloDeAtaque());
    }

    IEnumerator CicloDeAtaque()
    {
        while (true)
        {
            // fica parado e mira no jogador
            currentState = BossState.Mirar;
            rb.linearVelocity = Vector2.zero;

            float cronometro = 0f;
            Vector2 direcaoMira = Vector2.down;

            while (cronometro < tempoMirando)
            {
                if (player != null)
                    direcaoMira = ((Vector2)(player.position - transform.position)).normalized;

                cronometro += Time.deltaTime;
                yield return null;
            }

            currentState = BossState.Investida;
            rb.linearVelocity = direcaoMira * velocidadeInvestida;
            yield return new WaitForSeconds(duracaoInvestida);

            // stuna o boss após a investida
            currentState = BossState.Atordoado;
            rb.linearVelocity = Vector2.zero;
            yield return new WaitForSeconds(tempoAtordoado);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Cursor") && currentState == BossState.Atordoado)
        {
            Vector2 direcao = (transform.position - collision.transform.position).normalized;
            if (direcao == Vector2.zero)
                direcao = Vector2.up;

            if (cicloAtual != null)
                StopCoroutine(cicloAtual);

            StartCoroutine(LancarBoss(direcao));
        }
    }

    IEnumerator LancarBoss(Vector2 direcao)
    {
        currentState = BossState.Lancado;
        rb.linearVelocity = direcao * velocidadeLancamento;

        yield return new WaitForSeconds(duracaoLancamento);

        rb.linearVelocity = Vector2.zero;
        cicloAtual = StartCoroutine(CicloDeAtaque());
    }
}