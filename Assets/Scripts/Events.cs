using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Events : MonoBehaviour
{

    public void Menu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
    public void Level()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

}
