using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Story : MonoBehaviour
{
    public Transform head;
    public float height = 10f, speed = 1f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(sceneName: "Game");
        }

        float newY = Mathf.Sin(Time.time * speed) * height;
        head.eulerAngles = new Vector3(15, newY + 162, 0);
    }
}
