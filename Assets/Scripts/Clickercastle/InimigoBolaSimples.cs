using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class InimigoBolaSimples : MonoBehaviour
{
    public enum EnemyState { Patrulhar, Perseguir, Atacar }

    [SerializeField] private Transform Player;
    public Transform[] waypoints;

    public EnemyState currentState = EnemyState.Patrulhar;

    private int currentWaypointIndex = 0;
    private NavMeshAgent agent;
    private Rigidbody2D rb;
    [SerializeField] private float velocidadeLancamento = 8f;
    [SerializeField] private float duracaoLancamento = 0.25f;
    private bool estaSendoLancado;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody2D>();

        if (agent != null)
        {
            agent.updateRotation = false;
            agent.updateUpAxis = false;
        }
    }

    void Update()
    {
        if (agent == null || estaSendoLancado)
            return;

        if (Player == null)
        {
            currentState = EnemyState.Patrulhar;
            Patrulhar();
            return;
        }

        // aqui ele olha a distacia do inimigo para o jogador, daqui dependendo da distancia, ele muda de estados!
        float DistanciaParaJogador = Vector3.Distance(transform.position, Player.position);

        // dependendo da distancia muda de es
        if (DistanciaParaJogador <= 2f)
        {
            currentState = EnemyState.Atacar;
        }
        else if (DistanciaParaJogador <= 8f)
        {
            currentState = EnemyState.Perseguir;
        }
        else
        {
            currentState = EnemyState.Patrulhar;
        }

        // dependendo do estado atual é chamado a função
        switch (currentState)
        {
            case EnemyState.Patrulhar:
                Patrulhar();
                break;
            case EnemyState.Perseguir:
                Perseguir();
                break;
            case EnemyState.Atacar:
                Atacar();
                break;
        }
    }

    void Patrulhar()
    {
        if (waypoints == null || waypoints.Length == 0)
            return;

            // seleciona o waypoint atual que no caso é o 0 declarado no inicio
        Transform Target = waypoints[currentWaypointIndex];
        moveTowards(Target.position);

        // verifica se chegou no waypoint selecionado
        if (Vector3.Distance(transform.position, Target.position) < 0.5f)
        {
            // seleciona o proximo waypoint de modo crescente
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }

    void Perseguir()
    {
        // bem... ele coloca o jogador como target
        moveTowards(Player.position);
    }

    void Atacar()
    {
        // da um "dash" em direção ao jogador
        Vector3 direction = (Player.position - transform.position).normalized;
        transform.position += direction * 3f * Time.deltaTime;
    }

    // essa função é quem faz o inimigo se mover até o target
    void moveTowards(Vector3 targetPosition)
    {
        agent.SetDestination(targetPosition);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Cursor") && !estaSendoLancado)
        {
            // calcula a direção do lançamento com base na posição do cursor e do inimigo
            Vector2 direcao = (Vector2)(transform.position - collision.transform.position);
            if (direcao.sqrMagnitude == 0f)
                direcao = Vector2.up;
            else
                direcao.Normalize();

            StartCoroutine(Lancar(direcao));
        }
    }

    private IEnumerator Lancar(Vector2 direcao)
    {
        //desliga o NavMeshAgent e ativa a física para o lançamento
        estaSendoLancado = true;
        agent.isStopped = true;
        agent.updatePosition = false;
        agent.ResetPath();
        rb.linearVelocity = direcao * velocidadeLancamento;

        // espera a duração do lançamento
        yield return new WaitForSeconds(duracaoLancamento);

        // reativa o NavMeshAgent e desativa a física
        rb.linearVelocity = Vector2.zero;
        agent.Warp(rb.position);
        agent.updatePosition = true;
        agent.isStopped = false;
        estaSendoLancado = false;
    }
}
