
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


//Chat class
[System.Serializable]
public class Message
{
    public string text;
    public Text textObject;
    public string timeStamp;
}

public class GameManager : Singleton<GameManager> {

    //current size of a disk
    public Text systemText;
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
            //Update systemText
            systemText.text = string.Format("{0}GB free of {1}GB", systemSpace.ToString(), systemSize.ToString());
            systemSlider.value = SystemSpace / systemSize;
            //Set colors
            if (systemSlider.value > 0.75f)
            {
                systemColor.color = sliderColors[1];
            }
            else
            {
                systemColor.color = sliderColors[0];
            }
        }
    }
    //Flash drive space
    public Text flashText;
    [SerializeField]
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
            //Update systemText
            flashText.text = string.Format("{0}GB free of {1}GB", flashSpace.ToString(), flashSize.ToString());

            //reset Error Toggle
            if (flashSpace<flashSize)
                GameManager.Instance.errorShown = false;
            flashSlider.value = FlashSpace / flashSize;
            //Switch color
            if (flashSlider.value>0.75f)
            {
                flashColor.color = sliderColors[1];
            }
            else
            {
                flashColor.color = sliderColors[0];
            }
        }
    }

    //Progress handler for space
    private float progressChecker = 0;
    public float ProgressChecker
    {
        get
        {
            return progressChecker;
        }

        set
        {
            progressChecker = value;

            //Add to flash disk if there's space
            if (flashSpace <= flashSize)
            {
              
                FlashSpace += value;
               
                if (SystemSpace < systemSize)
                {
                    SystemSpace += value/3;
                    
                }
                //GameOver check
                else if (!GameOver)
                {
                    DeathScreenCheck = true;
                    GameOver = true;
                }
            }
            else
            {
                StartCoroutine(PlotManager.Instance.StopBlink(PlotManager.Instance.folderImage));
           
                errorShown = true;
                //Add to system disk if flash is full
                if (SystemSpace < systemSize)
                {
                    SystemSpace += value;
                    
                }
                //GameOver check
                else if(!GameOver)
                {
                    DeathScreenCheck = true;
                    GameOver = true;
                }
            }

            //Reset progress checker for next write
            progressChecker = 0;
        }
    }

    //Temp Clear in progress
    public bool clearTempProgress = false;

    //Reference for window and error pop ups
    public Transform errorHolder;

    //Time in tray
    public Text timeTray;

    //errorTracking bool 
    public bool errorShown = false;

    //Trash bin variables
    public float trashSize = 0;
    private bool trashBinFull = false;
    public Sprite[] trashBinIcons;
    public Image trashBinIcon;

    public bool TrashBinFull
    {
        get
        {
            return trashBinFull;
        }

        set
        {
            trashBinFull = value;
            if(trashBinFull)
            {
                //change icon on the desktop
                trashBinIcon.sprite = trashBinIcons[1];
            }
            else
            {
                trashBinIcon.sprite = trashBinIcons[0];
            }
        }
    }



    //game over handler with property
    public GameObject deathScreenPref;
    public GameObject bannerPref;
    [SerializeField]
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
            if (gameOver == true)
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

                Instantiate(contextGameOver, errorHolder.position, Quaternion.identity, errorHolder);
               
                
            }
        }
    }

    //Goal/money
    public float goal = 10000;
    public float flashPrice = 100;
    public Text moneyText;
    private float money = 13.77f;
    public float Money
    {
        get
        {
            return money;
        }

        set
        {

            money = float.Parse(System.Math.Round(value, 2).ToString());
            moneyText.text = string.Format("{0}/{1}$", money.ToString(), goal.ToString());
        }
    }


    //Ui for disks
    public Slider systemSlider;
    public Slider flashSlider;
    public Color[] sliderColors;
    public Image flashColor;
    public Image systemColor;

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


    //Chat handler
    public string chatPref;

    public GameObject chatPanel;
    public GameObject textObject;
    public InputField chatBox;

    //JEtGet check bool
    public bool jetStarted;
    //Irq window open bool
    public bool irqOpen = false;
    [SerializeField]
    List<Message> messageList = new List<Message>();



    //Chat function
    public void SendMessageToChat(string text)
    {
        Message newMessage = new Message();
        newMessage.text = text;

        GameObject newText = Instantiate(textObject, chatPanel.transform);

        newMessage.textObject = newText.GetComponent<Text>();
        newMessage.textObject.text = newMessage.text;

        messageList.Add(newMessage);
    }

   

    public void Start()                                                //START//
    {
        //Update systemText
        systemText.text = string.Format("{0}GB free of {1}GB", systemSpace.ToString(),systemSize.ToString());
        //Update systemText
        flashText.text = string.Format("{0}GB free of {1}GB", flashSpace.ToString(), flashSize.ToString());

        systemSlider.value = SystemSpace / systemSize;
        flashSlider.value = FlashSpace / flashSize;
        
        downloads = new Stack<GameObject>();
      
        //Activate plot manager sequence
        StartCoroutine(StartPlot());

        //Start storyline if below idle

        //StartCoroutine(SpawnDownloads());


    }

    public void Update()
    {
        //Grab time
        timeTray.text = System.DateTime.Now.ToString("hh:mm");

        //Death screen reset anykey
        if (GameOver)
        {
            StopAllCoroutines();
            if(Input.anyKeyDown)
            {
                if (Input.GetMouseButtonDown(0)
                 || Input.GetMouseButtonDown(1)
                 || Input.GetMouseButtonDown(2))
                    return; //Do Nothing
                SceneManager.LoadScene("Boot");
            }
        }

        //Chat window
        if (irqOpen)
        {
            if (chatBox.text != "")
            {
                //Activate input on any buttonclick
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    SendMessageToChat("  <color=red><b>[" + System.DateTime.Now.ToString("hh:mm") +"] mEgAPiRate777:</b></color> " + chatBox.text);
                    PlotManager.Instance.plotTrigger.Invoke();
                    chatBox.text = "";
                }
            }
            else
            {
                if (!chatBox.isFocused && Input.anyKeyDown)
                {
                    if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
                        return; //Do Nothing
                    chatBox.ActivateInputField();
                }
            }
        }
    }

    //JetStop handler
    public void JetStop()
    {
        StopCoroutine(SpawnDownloads());
        jetStarted = false;
    }


    public IEnumerator SpawnDownloads()
    {
        yield return new WaitForSeconds(3f);





        int virusChance = 0;
        while(jetStarted)
        {
            GameObject tmp = null;
            if (downloadHolder.childCount<=8)
            {

                virusChance += Random.Range(0, 10);
                //Debug.Log(virusChance + " :  :  : " + virusChance % 30);
                if(virusChance%10==0)
                {
                    tmp = downloadPrefs[0];   
                }
                else
                {
                    tmp = downloadPrefs[1];
                }

                Instantiate(tmp, downloadHolder);
                tmp.transform.SetAsFirstSibling();
            }
           
            yield return new WaitForSeconds(Random.Range(1f,3f));
        }
       
    }

    public IEnumerator StartPlot()
    {
        yield return new WaitForSeconds(1f);
        if (PlayerPrefs.GetInt("PlotStep", 0) < PlotManager.Instance.plotMessages.Length)
        {
            for (int i = 0; i < 6; i++)
            {
                PlotManager.Instance.plotTrigger.Invoke();
                yield return new WaitForSeconds(Random.Range(1f,3f));
            }
        }
        
    }



}
