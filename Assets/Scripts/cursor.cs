using UnityEngine;

public class cursor : MonoBehaviour
{
    void Update()
    {

        Camera mainCamera = Camera.main;

        // Cria um objeto "imaginario"
        Plane objectPlane = new Plane(Vector3.forward, transform.position);

        // Converte a posição do mouse em um raio
        Ray mouseRay = mainCamera.ScreenPointToRay(Input.mousePosition);

        // verifica raio caso dar, vira posição
        if (objectPlane.Raycast(mouseRay, out float distance))
        {
            // vira a posilção do mouse 
            transform.position = mouseRay.GetPoint(distance);
        }
    }
}
