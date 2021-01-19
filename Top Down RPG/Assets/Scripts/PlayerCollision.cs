using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCollision : MonoBehaviour
{
    public SceneLoader sceneLoader;

    void Start()
    {
        sceneLoader = GetComponent<SceneLoader>();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        var obj = collision.gameObject.tag;

        if (obj == "Item")
            Destroy(collision.gameObject);
        if (obj == "BadObject")
            SceneManager.LoadSceneAsync(2);
        if (obj == "Exit")
        {
            StaticClass.PreviousScene = StaticClass.CurrentScene;
            sceneLoader.NavigateFrom(collision.gameObject.GetComponent<Exit>().exitNumber);
        }
    }
}
