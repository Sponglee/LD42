using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlotManager : Singleton<PlotManager> {

    public int plotStep = 0;

    public UnityEvent plotTrigger;

    public string[] plotMessages;
    public string[] plotMessagesIdle;
    public string[] plotMessagesQuestions;



    public Image irqIcon;
    public Image virusImage;
    public Image folderImage;
    public Image jetGetImage;


    public Color[] blinkColors;

    //plot MEssages
    public string plotRaw;
    //---------IDLE MESSAGES
    public string plotRawIdle;
    //For randomizer
    public int plotIndexIdle = 0;
    //For timeout questions
    public string plotRawQuestions;
    //For randomizer
    public int plotIndexQuestions=0;


    // Use this for initialization
    private void Awake()
    {
        plotRaw = "Hey," +
               "You there?," +
               "Have you thought about what we discussed yesterday?," +
               "If you really want to be able to pay for your university you gotta accept my offer," +
               "Plus i've got the app already. It' here on my pc and it's gorgeous," +
               "I can send it to you just say the word," +
               "What you gonna say?"+
                "One moment.,"+
               "<color=blue>hhtp://trustworthyh@ckers.org/jetget.zip</color> Click it.," +            //STEP 8!!!//
               "Did you click the link?,"+
               "I'll be paying for each flash drive u send me," +
               "Remember. this thing downloads all fresh releases straight to your flash drive, got it?" +
               "Be aware of viruses though.Right click to dismiss them. Your antivirus should handle the rest," +
               "Eject flash drive with right mouse button once it's full," +
               "Oh and don't let the hard drive fill with junk. Clear temp folder on your system drive by right clicking on it," +
               "GET ME THOSE FLASH DRIVES!";
        plotRawIdle = "Come on," +
                "Stop fooling around," +
                "I've got no time for this," +
                "How's it going?," +
                "Get to work," +
                "Stop messing around with the chat," +
                "Do you need money or not?," +
                "Not interested," +
                "* Away*," +
                "Sup?," +
                "These thing is golden! Don't you think?," +
                "I hope you like the app," +
                "It's all good," +
                "Gotta go," +
                "Brb," +
                "Don't disturb me," +
                "Do your thing or u'll get no money," +
                "No time to chat";

        plotRawQuestions = "Hello?," +
                "Stop fooling around," +
                "I've got no time for this, you there?," +
                "How's it going?," +
                "So?," +
                "Sup?," +
                "Do you need money or not?," +
                "So you're in?," +
                "*Poke*," +
                "Hey?," +
                "How do you like it?," +
                "Say something," +
                "So what?," +
                "Are you gonna be silent? time is money!," +
                "Are you there?," +
                "Uhm.....," +
                "......................," +
                "I'll wait sure....";

     
    }


    
    void Start ()                                                                                                                       //START//
    {
        StartCoroutine(StopBlink(virusImage));
       
        plotMessages= plotRaw.Split(',');
        plotMessagesIdle = plotRawIdle.Split(',');
        plotMessagesQuestions = plotRawQuestions.Split(',');
        plotStep = PlayerPrefs.GetInt("PlotStep", 0);
        if (plotStep >= 8)
        {
            PlayerPrefs.SetInt("PlotStep",9);
            plotStep = 9;
            FunctionHandler.Instance.DownloadJetGet();
        }

    }
	
	public void PlotTriggered()
    {
        if (PlayerPrefs.GetInt("PlotStep", 0) > 9)
        {
            foreach(GameObject obj in FunctionHandler.Instance.jetGetObjs)
            {
                obj.SetActive(true);
            }
        }
        StartCoroutine(StopSend());
       
    }


    private IEnumerator StopSend()
    {
        yield return new WaitForSeconds(Random.Range(1, 3));
        StartCoroutine(StopBlink(irqIcon));
        plotIndexIdle = PlotRandomiser(plotIndexIdle, plotMessagesIdle.Length);
        if (PlayerPrefs.GetInt("PlotStep", 0) >= plotMessages.Length)
            GameManager.Instance.SendMessageToChat("  <color=blue><b>[" + 
                System.DateTime.Now.ToString("hh:mm") + "] <RlsMaster>:</b></color> " + 
                   plotMessagesIdle[plotIndexIdle]);
        else
            GameManager.Instance.SendMessageToChat("  <color=blue><b>[" + 
                System.DateTime.Now.ToString("hh:mm") + "] <RlsMaster>:</b></color> " + 
                    plotMessages[plotStep]);
        plotStep++;
        PlayerPrefs.SetInt("PlotStep", plotStep);
        StartCoroutine(ChatTimeOut());
    }

    public IEnumerator StopBlink(Image contextImage)
    {

        for (int i = 0; i < 4; i++)
        {
            contextImage.color = blinkColors[1];
            yield return new WaitForSeconds(0.5f);
            contextImage.color = blinkColors[0];
            yield return new WaitForSeconds(0.2f);
        }
    }

    public IEnumerator ChatTimeOut()
    {
        int plotCurrentStep = plotStep;
        if(plotStep>9)
            yield return new WaitForSeconds(Random.Range(5, 10));
        else
            yield return new WaitForSeconds(Random.Range(5 ,7));

    
        plotIndexQuestions = PlotRandomiser(plotIndexQuestions, plotMessagesQuestions.Length);
        if (PlayerPrefs.GetInt("PlotStep", 0) == plotCurrentStep && PlayerPrefs.GetInt("PlotStep", 0) < 9)
        {
            GameManager.Instance.SendMessageToChat("  <color=blue><b>[" + 
                System.DateTime.Now.ToString("hh:mm") + "] <RlsMaster>:</b></color> " + 
                    plotMessagesQuestions[plotIndexQuestions]);
            StartCoroutine(StopBlink(irqIcon));
            StartCoroutine(ChatTimeOut());
        }
         else if (PlayerPrefs.GetInt("PlotStep", 0) == plotCurrentStep && PlayerPrefs.GetInt("PlotStep", 0) >= 9
            && PlayerPrefs.GetInt("PlotStep", 0) < 16)
        {
            plotTrigger.Invoke();
        }
       
    }


    public int PlotRandomiser(int lastValue, int length)
    {
        int tmp;
        do
        {
            tmp = Random.Range(0, length);
        }
        while (tmp == lastValue);
        return tmp;
    }
}
