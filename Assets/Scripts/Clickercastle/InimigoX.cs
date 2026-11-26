using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class InimigoX : MonoBehaviour
{
    public enum Estado { Esperando, Caindo, NoChao, Subindo }
    public Estado estadoAtual = Estado.Esperando;

    [Header("Detecção")]
    public Vector2 areaDeteccao = new Vector2(2f, 8f);
    public float margemChao = 0.1f;
    public LayerMask camadaJogador;
    public LayerMask camadaChao;

    [Header("Movimento")]
    public float velocidadeQueda = 20f;
    public float velocidadeSubida = 3f;
    public float tempoNoChao = 1f;

    private Rigidbody2D rb;
    private Collider2D col;
    private Vector2 posicaoInicial;
    private float cronometro;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        posicaoInicial = rb.position;
    }

    void FixedUpdate()
    {
        switch (estadoAtual)
        {
            case Estado.Esperando:
                if (JogadorAbaixo())
                    estadoAtual = Estado.Caindo;
                break;

            case Estado.Caindo:
                rb.MovePosition(rb.position + Vector2.down * (velocidadeQueda * Time.fixedDeltaTime));
                if (TocouNoChao())
                {
                    cronometro = 0f;
                    estadoAtual = Estado.NoChao;
                }
                break;

            case Estado.NoChao:
                cronometro += Time.fixedDeltaTime;
                if (cronometro >= tempoNoChao)
                    estadoAtual = Estado.Subindo;
                break;

            case Estado.Subindo:
                rb.MovePosition(Vector2.MoveTowards(rb.position, posicaoInicial, velocidadeSubida * Time.fixedDeltaTime));
                if (Vector2.Distance(rb.position, posicaoInicial) < 0.01f)
                {
                    rb.MovePosition(posicaoInicial);
                    estadoAtual = Estado.Esperando;
                }
                break;
        }
    }

    bool JogadorAbaixo()
    {
        Vector2 centro = (Vector2)transform.position + Vector2.down * (areaDeteccao.y * 0.5f);
        return Physics2D.OverlapBox(centro, areaDeteccao, 0f, camadaJogador) != null;
    }

    bool TocouNoChao()
    {
        // Reduz levemente a largura da caixa para não prender em paredes laterais
        RaycastHit2D hit = Physics2D.BoxCast(col.bounds.center, col.bounds.size * 0.95f, 0f, Vector2.down, margemChao, camadaChao);
        return hit.collider != null;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0f, 0f, 0.35f);
        Vector2 centro = (Vector2)transform.position + Vector2.down * (areaDeteccao.y * 0.5f);
        Gizmos.DrawCube(centro, areaDeteccao);
    }
}