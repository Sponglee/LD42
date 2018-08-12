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
               "One moment. Oh and don't let the hard drive fill with junk. Clear temp folder on your system drive by right clicking on it," +
               "<color=blue>hhtp://trustworthyh@ckers.org/jetget.zip</color>," +            //STEP 8!!!//
               "Did you click the link?,"+
               "I'll be paying for each flash drive u send me, okay?," +
               "Remember. this thing downloads all fresh releases straight to your flash drive, got it?" +
               "Be aware of viruses though.Right click to dismiss them. Your antivirus should handle the rest," +
               "Eject flash drive with right mouse button once it's full is that clear?," +
               "This thing is no joke, don't tell anybody about it!," +
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
        if (PlayerPrefs.GetInt("PlotStep", 0) > 8)
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
        StartCoroutine(StopBlink());
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

    public IEnumerator StopBlink()
    {

        for (int i = 0; i < 4; i++)
        {
            irqIcon.color = blinkColors[1];
            yield return new WaitForSeconds(0.5f);
            irqIcon.color = blinkColors[0];
            yield return new WaitForSeconds(0.2f);
        }
    }

    public IEnumerator ChatTimeOut()
    {
        int plotCurrentStep = plotStep;
        yield return new WaitForSeconds(Random.Range(10,15));
        Debug.Log(">>>>>" + PlayerPrefs.GetInt("PlotStep", 0) + " " + plotCurrentStep);
        plotIndexQuestions = PlotRandomiser(plotIndexQuestions, plotMessagesQuestions.Length);
        if (PlayerPrefs.GetInt("PlotStep", 0) == plotCurrentStep && PlayerPrefs.GetInt("PlotStep", 0) < 8)
        {
            GameManager.Instance.SendMessageToChat("  <color=blue><b>[" + 
                System.DateTime.Now.ToString("hh:mm") + "] <RlsMaster>:</b></color> " + 
                    plotMessagesQuestions[plotIndexQuestions]);
            StartCoroutine(StopBlink());
            StartCoroutine(ChatTimeOut());
        }
         else if (PlayerPrefs.GetInt("PlotStep", 0) == plotCurrentStep && PlayerPrefs.GetInt("PlotStep", 0) >= 8)
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
