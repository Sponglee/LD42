
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
            systemSlider.value = SystemSpace / systemSize;
        }
    }
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
          
           //reset Error Toggle
            if (flashSpace<flashSize)
                GameManager.Instance.errorShown = false;
            flashSlider.value = FlashSpace / flashSize;
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
                if(!errorShown)
                    FunctionHandler.Instance.ShowError("Not enough space on usb Flash Drive! Please remove");
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

   
    //Reference for window and error pop ups
    public Transform errorHolder;

    //Time in tray
    public Text timeTray;

    //errorTracking bool 
    public bool errorShown = false;

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


    //Chat handler
    public string chatPref;

    public GameObject chatPanel;
    public GameObject textObject;
    public InputField chatBox;

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



    public void Start()
    {
        
        systemSlider.value = SystemSpace / systemSize;
        flashSlider.value = FlashSpace / flashSize;
        
        downloads = new Stack<GameObject>();

        StartCoroutine(SpawnDownloads());
        
    }

    public void Update()
    {
        //Grab time
        timeTray.text = System.DateTime.Now.ToString("hh:mm");

        //Death screen reset anykey
        if (GameOver && DeathScreenCheck)
        {
            if(Input.anyKeyDown)
            {
                if (Input.GetMouseButtonDown(0)
                 || Input.GetMouseButtonDown(1)
                 || Input.GetMouseButtonDown(2))
                    return; //Do Nothing
                SceneManager.LoadScene("Main");
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
    public IEnumerator SpawnDownloads()
    {
        int virusChance = 0;
        while(true)
        {
            GameObject tmp = null;
            if (downloadHolder.childCount<=8)
            {

                virusChance += Random.Range(0, 10);
                Debug.Log(virusChance + " :  :  : " + virusChance % 30);
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




}
