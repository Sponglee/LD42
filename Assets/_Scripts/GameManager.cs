
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager> {

    //current size of a disk
    [SerializeField]
    private float systemSpace;
    public float SystemSpace
    {
        get
        {
            return systemSpace;
        }

        set
        {
            systemSpace = value;
        }
    }

    private float flashSpace;
    public float FlashSpace
    {
        get
        {
            return flashSpace;
        }

        set
        {
            flashSpace = value;
        }
    }


   

    //Ui for disks
    public Slider systemSlider;
    public Slider flashSlider;


    //Total size of a disk
    public float systemSize;
    public float flashSize;

    //Downloads holders etc
    public Stack<GameObject> downloads;
    public GameObject[] downloadPrefs;
    public Transform downloadHolder;

    //Game Over check
    public bool BannerCheck = false;
    public bool DeathScreenCheck = false;


    //game over handler with property
    public GameObject deathScreenPref;
    public GameObject bannerPref;

    private bool gameOver = false;
    public bool GameOver
    {
        get
        {
            return gameOver;
        }

        set
        {
           gameOver = value;
            if(gameOver == true)
            {
                GameObject contextGameOver = null;
                if (BannerCheck)
                {
                    contextGameOver = bannerPref;
                }
                else if (DeathScreenCheck)
                {
                    contextGameOver = deathScreenPref;
                }

                Instantiate(contextGameOver,gameObject.transform);
            }
        }
    }
    public void Start()
    {
        systemSlider.value = SystemSpace / systemSize;
        flashSlider.value = FlashSpace / flashSize;
        
        downloads = new Stack<GameObject>();

        StartCoroutine(SpawnDownloads());
        
    }

    public void Update()
    {
        
    }
    public IEnumerator SpawnDownloads()
    {
        while(true)
        {
            GameObject tmp = null;
            if (downloadHolder.childCount<=8)
            {
                int virusChance = Random.Range(0, 100);

                if(virusChance<=30)
                {
                    tmp = downloadPrefs[0];   
                }
                else
                {
                    tmp = downloadPrefs[1];
                }

                Instantiate(tmp, downloadHolder);
            }
           
            yield return new WaitForSeconds(3);
        }
       
    }

}
