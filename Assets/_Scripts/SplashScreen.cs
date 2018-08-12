using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour {

    private CanvasGroup fadeGroup;
    private float loadTime;
    [SerializeField]
    private float minimumLogoTime = 3.0f; //Minimum logo time;
    public GameObject solidHinken;
    public string nextScene;
    public int speed = 1;

    void Start()
    {
        
        fadeGroup = FindObjectOfType<CanvasGroup>();

        fadeGroup.alpha = 0.99f;


        //Get a timestep of a completion time
        if (Time.timeSinceLevelLoad < minimumLogoTime)
            loadTime = minimumLogoTime;
        else
            loadTime = Time.timeSinceLevelLoad;




    }


    private void Update()
    {
     

        //FadeIn
        if (Time.timeSinceLevelLoad < minimumLogoTime)
             fadeGroup.alpha = 1 - speed*Time.timeSinceLevelLoad;
      
        //FadeOut
        if (Time.timeSinceLevelLoad > minimumLogoTime && loadTime != 0)
        {
            //solidHinken.SetActive(false);
            //if (nextScene != "MAIN")
            //{
                fadeGroup.alpha = speed*Time.timeSinceLevelLoad - minimumLogoTime;
               
            //}
            //else
            //{
            //    fadeGroup.alpha = 1;
            //}
            
            if (fadeGroup.alpha>=1)
            {
                SceneManager.LoadScene(nextScene);
            }
        }
            


    }
}
