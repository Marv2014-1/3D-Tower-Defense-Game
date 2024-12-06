using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Story : MonoBehaviour
{
    public TMP_Text storyText;
    private static int step;

    // Start is called before the first frame update
    void Start()
    {
        step = 1;
        storyText.text = "In the faraway kingdom of Oratorio lived the humble wizard Giocoso who used his musical magic to aid those in need and spread love and song to all he would come across.";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            Progress();
        }
    }

    void Progress()
    {
        switch (step)
        {
            case 1:
                storyText.text = "However one day in their travels his apprentice Coda betrayed Giocoso, stealing the bulk of his magic for his own ill intentions.";
                break;
            case 2:
                storyText.text = "With the power of music under Coda’s command, he warped the lands and its inhabitants into his dark image.";
                break;
            case 3:
                storyText.text = "Now Giocoso must travel his newly twisted home and defeat Coda’s minions in order to regain his lost power and restore Oratorio to its former glory.";
                break;
            case 4:
                SceneManager.LoadScene(sceneName: "Brandon-Test");
                break;
            default:
                break;
        }
        step++;
    }
}
