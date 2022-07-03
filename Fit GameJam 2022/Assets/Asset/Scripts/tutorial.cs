using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class tutorial : MonoBehaviour
{
    public float TutorialTimes = 3f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Tutorial());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    IEnumerator Tutorial()
    {
        yield return new WaitForSecondsRealtime(TutorialTimes);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
