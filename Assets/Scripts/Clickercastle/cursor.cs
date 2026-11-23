using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; // Necessário para detectar troca de cena

[RequireComponent(typeof(Rigidbody2D))]
public class cursor : MonoBehaviour
{
    private Collider2D colisor2d;
    private Rigidbody2D corpo2d;
    private Camera cameraPrincipal;
    private bool estaTangivel = false;
    private Coroutine rotinaTangibilidade;

    void Awake()
    {
        corpo2d = GetComponent<Rigidbody2D>();
        colisor2d = GetComponent<Collider2D>();
    }

    void OnEnable()
    {
        // Escuta o evento de carregamento de cena
        SceneManager.sceneLoaded += AoCarregarCena;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= AoCarregarCena;
    }

    void Start()
    {
        ResetarEstado();
    }

    // Roda automaticamente toda vez que uma nova cena abre
    void AoCarregarCena(Scene cena, LoadSceneMode modo)
    {
        ResetarEstado();
    }

    void ResetarEstado()
    {
        // Garante que o jogo não esteja pausado na nova cena
        Time.timeScale = 1f;

        // Atualiza a referência da câmera da nova cena
        cameraPrincipal = Camera.main;

        if (rotinaTangibilidade != null)
        {
            StopCoroutine(rotinaTangibilidade);
            rotinaTangibilidade = null;
        }

        estaTangivel = false;

        if (colisor2d != null)
        {
            colisor2d.enabled = false;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !estaTangivel)
        {
            rotinaTangibilidade = StartCoroutine(AtivarTangibilidade());
        }
    }

    void FixedUpdate()
    {
        // Se perdeu a câmera, tenta encontrar novamente
        if (cameraPrincipal == null)
        {
            cameraPrincipal = Camera.main;
            if (cameraPrincipal == null) return; // Evita erro se não houver MainCamera na cena
        }

        Vector3 posicaoMouseMundo = cameraPrincipal.ScreenToWorldPoint(Input.mousePosition);
        posicaoMouseMundo.z = transform.position.z;

        corpo2d.MovePosition(new Vector2(posicaoMouseMundo.x, posicaoMouseMundo.y));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.GetContact(0).normal.y < -0.5f)
            {
                collision.transform.SetParent(this.transform);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.gameObject.activeInHierarchy)
        {
            collision.transform.SetParent(null);
        }
    }

    IEnumerator AtivarTangibilidade()
    {
        estaTangivel = true;
        colisor2d.enabled = true;

        yield return new WaitForSeconds(0.5f);

        colisor2d.enabled = false;
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