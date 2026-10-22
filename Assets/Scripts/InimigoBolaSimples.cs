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

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (agent != null)
        {
            agent.updateRotation = false;
            agent.updateUpAxis = false;
        }
    }

    void Update()
    {
        if (agent == null)
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
        // bem... ele coloco o jogador como alvo ai segue ele... é bem auto explicatico!
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
}
