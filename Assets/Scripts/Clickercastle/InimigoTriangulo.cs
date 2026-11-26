using UnityEngine;

public class InimigoTriangulo : MonoBehaviour
{
    public enum Estado { Patrulhando, Mirando }
    public Estado estadoAtual = Estado.Patrulhando;

    [Header("Referências")]
    public Transform jogador;
    public Transform pontoDeTiro;
    public GameObject projetilPrefab;
    public LayerMask camadaObstaculos;

    [Header("Configurações")]
    public float alcanceVisao = 8f;
    [Range(0, 360)] public float anguloVisao = 90f;
    public float velocidadeGiroPatrulha = 60f;
    public float velocidadeMira = 8f;
    public float cadenciaTiro = 1.2f;

    private float cronometroTiro;

    void Update()
    {
        if (jogador == null) return;

        estadoAtual = PodeVerJogador() ? Estado.Mirando : Estado.Patrulhando;

        if (estadoAtual == Estado.Mirando)
            Mirar();
        else
            Patrulhar();

        // Atira independente de estar vendo o jogador ou não
        cronometroTiro += Time.deltaTime;
        if (cronometroTiro >= cadenciaTiro)
        {
            Atirar();
            cronometroTiro = 0f;
        }
    }

    void Patrulhar()
    {
        transform.Rotate(0f, 0f, velocidadeGiroPatrulha * Time.deltaTime);
    }

    void Mirar()
    {
        Vector2 direcao = jogador.position - transform.position;
        float angulo = Mathf.Atan2(direcao.y, direcao.x) * Mathf.Rad2Deg;
        Quaternion alvo = Quaternion.Euler(0f, 0f, angulo);
        
        transform.rotation = Quaternion.Lerp(transform.rotation, alvo, velocidadeMira * Time.deltaTime);
    }

    void Atirar()
    {
        if (projetilPrefab && pontoDeTiro)
        {
            Instantiate(projetilPrefab, pontoDeTiro.position, pontoDeTiro.rotation);
        }
    }

    bool PodeVerJogador()
    {
        Vector2 dir = jogador.position - transform.position;
        float dist = dir.magnitude;

        if (dist > alcanceVisao) return false;
        if (Vector2.Angle(transform.right, dir) > anguloVisao * 0.5f) return false;

        // Verifica se tem parede bloqueando a visão
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir.normalized, dist, camadaObstaculos);
        return hit.collider == null;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, alcanceVisao);

        Vector3 esq = Quaternion.Euler(0, 0, anguloVisao * 0.5f) * transform.right;
        Vector3 dir = Quaternion.Euler(0, 0, -anguloVisao * 0.5f) * transform.right;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + esq * alcanceVisao);
        Gizmos.DrawLine(transform.position, transform.position + dir * alcanceVisao);
    }
}