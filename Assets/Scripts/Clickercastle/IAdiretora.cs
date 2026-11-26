using System.Reflection;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class IAdiretora : MonoBehaviour
{
    [SerializeField, Range(0.1f, 1f)]
    private float reducaoPorReinicio = 0.85f;

    [SerializeField, Range(0.1f, 1f)]
    private float velocidadeMinima = 0.35f;

    private static IAdiretora instancia;
    private static float multiplicadorVelocidade = 1f;
    private int ultimaCenaCarregada = -1;

    private static readonly FieldInfo velocidadeLancamentoBola =
        typeof(InimigoBolaSimples).GetField(
            "velocidadeLancamento",
            BindingFlags.Instance | BindingFlags.NonPublic
        );

    private void Awake()
    {
        if (instancia != null && instancia != this)
        {
            Destroy(gameObject);
            return;
        }

        instancia = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        if (instancia == this)
            SceneManager.sceneLoaded += AoCarregarCena;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= AoCarregarCena;
    }

    private void OnDestroy()
    {
        if (instancia == this)
            instancia = null;
    }

    private void AoCarregarCena(Scene cena, LoadSceneMode modo)
    {
        if (cena.buildIndex == 0)
        {
            multiplicadorVelocidade = 1f;
        }
        else if (cena.buildIndex == ultimaCenaCarregada)
        {
            multiplicadorVelocidade = Mathf.Max(
                velocidadeMinima,
                multiplicadorVelocidade * reducaoPorReinicio
            );
        }

        ultimaCenaCarregada = cena.buildIndex;
        AplicarVelocidadeAosInimigos();
    }

    private void AplicarVelocidadeAosInimigos()
    {
        foreach (InimigoX inimigo in FindObjectsByType<InimigoX>(FindObjectsInactive.Include))
        {
            inimigo.velocidadeQueda *= multiplicadorVelocidade;
            inimigo.velocidadeSubida *= multiplicadorVelocidade;
        }

        foreach (InimigoTriangulo inimigo in FindObjectsByType<InimigoTriangulo>(FindObjectsInactive.Include))
        {
            inimigo.velocidadeGiroPatrulha *= multiplicadorVelocidade;
            inimigo.velocidadeMira *= multiplicadorVelocidade;
        }

        foreach (InimigoBolaSimples inimigo in FindObjectsByType<InimigoBolaSimples>(FindObjectsInactive.Include))
        {
            NavMeshAgent agente = inimigo.GetComponent<NavMeshAgent>();
            if (agente != null)
                agente.speed *= multiplicadorVelocidade;

            if (velocidadeLancamentoBola != null)
            {
                float velocidade = (float)velocidadeLancamentoBola.GetValue(inimigo);
                velocidadeLancamentoBola.SetValue(inimigo, velocidade * multiplicadorVelocidade);
            }
            else
            {
                Debug.LogError("Não foi possível encontrar o atributo de lançamento do InimigoBolaSimples.");
            }
        }

        foreach (InimigoBOSS inimigo in FindObjectsByType<InimigoBOSS>(FindObjectsInactive.Include))
        {
            inimigo.velocidadeInvestida *= multiplicadorVelocidade;
            inimigo.velocidadeLancamento *= multiplicadorVelocidade;
        }
    }
}
