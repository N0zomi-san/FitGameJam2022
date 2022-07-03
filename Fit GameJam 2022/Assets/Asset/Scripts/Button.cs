using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{
    public GameObject Canvas;
    public GameObject Tutorial;
    public float TutorialTimes = 3f;
    // Start is called before the first frame update
    void Start()
    {
        Tutorial.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextScene()
    {
        StartCoroutine(StartTutorial());
        Canvas.SetActive(false);
        Tutorial.SetActive(true);
    }

    public void doExitGame() {
        Application.Quit();
    }

    IEnumerator StartTutorial()
    {
        yield return new WaitForSecondsRealtime(TutorialTimes);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
