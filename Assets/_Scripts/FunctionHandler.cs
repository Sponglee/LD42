using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FunctionHandler : Singleton<FunctionHandler>
{
   

    public GameObject driveMenu;
    public GameObject folderMenu;
    public GameObject binMenu;

    public GameObject menuWindow;
    public GameObject folderWindow;
    public GameObject compWindow;
    public GameObject jetWindow;
    public GameObject irqWindow;
    public GameObject virusHelp;
    public GameObject errorWindowPref;


    public GameObject flashDrive;


    //For Download JetGet:
    public GameObject[] jetGetObjs;
    
    //Offset for stacking errorMessages
    private int windowOffset = 0;


    //Show Error Message
    public void ShowError(string errorText, int times=1)
    {
        
        for (int i = 0; i < times; i++)
        {
            if (windowOffset >= 250)
                windowOffset = 0;

            Vector3 errorSpawnPos = new Vector3(GameManager.Instance.errorHolder.position.x + windowOffset, GameManager.Instance.errorHolder.position.y + windowOffset);
            Instantiate(errorWindowPref, errorSpawnPos, Quaternion.identity, GameManager.Instance.errorHolder);
            errorWindowPref.transform.GetChild(0).GetChild(3).GetComponent<Text>().text = errorText;
            //Move next window a bit
            windowOffset += 50;

        }
       
        GameManager.Instance.errorShown = false;
    }
    //Open MainMenu
    public void OpenMenu()
    {
        CloseAllContexts();
        menuWindow.SetActive(!menuWindow.gameObject.activeSelf);
        if (Time.timeScale == 1)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }

    //Open window
    public void OpenWindow(string name)
    {
        GameObject context = null;

        if (name == "FolderWindow")
        {
            context = folderWindow;
        }
        else if (name == "MyComputerWindow")
        {
            context = compWindow;
        }
        if (name == "IrqHandler")
        {
            context = irqWindow;
            GameManager.Instance.irqOpen = true;
        }
        if (name == "JetGet")
        {
            context = jetWindow;
          
        }
        if (name == "VirusHelp")
        {
            context = virusHelp;

        }
        if (context != null)
        {
            context.SetActive(true);
        }
    }
    //Open contextMenu
    public void OpenContext(string name)
    {
        GameObject context = null;

        if(name == "FlashDrive")
        {
            context = driveMenu;    
        }
        else if (name == "Folder")
        {
            context = folderMenu;
        }
        else if (name == "Bin")
        {
            context = binMenu;
        }
       

        CloseAllContexts();

        if (context != null)
        {
            context.SetActive(!context.gameObject.activeSelf);
            context.transform.position = Input.mousePosition;
        }
       
    }

    //Close clicked winow
    public void CloseWindow(GameObject window)
    {
        CloseAllContexts();
        //If dealing with IrQ window - toggle gameManager bool
        if(window.name == "IrqHandler")
        {
            GameManager.Instance.irqOpen = false;
        }
        window.SetActive(false);
    }

    //Close all menus that are open
    public void CloseAllContexts()
    {
        driveMenu.SetActive(false);
        folderMenu.SetActive(false);
        binMenu.SetActive(false);
    }

    //Close Virus window
    public void CloseVirus(GameObject clickObj)
    {
        Destroy(clickObj);
    }



   
    //Handling Flash Eject Button
    public void EjectFlash()
    {
        CloseAllContexts();
        StartCoroutine(StopEject());
    }
    //Context Menu eject flash drive
    public IEnumerator StopEject()
    {

        flashDrive.SetActive(false);
        //Get Payment for how full is flash drive
        GameManager.Instance.Money += GameManager.Instance.flashPrice*GameManager.Instance.FlashSpace/GameManager.Instance.flashSize;
        yield return new WaitForSeconds(0.5f);
        GameManager.Instance.FlashSpace = 0;
        flashDrive.SetActive(true);
    }


    //Handling TempClear 
    public void ClearTemp()
    {
        CloseAllContexts();
        if(GameManager.Instance.SystemSpace>64)
            StartCoroutine(StopTemp());
    }
    //Context Menu clear Temp
    public IEnumerator StopTemp()
    {
        GameManager.Instance.TrashBinFull = true;
        for (int i = 0; i < 5; i++)
        {
            GameManager.Instance.SystemSpace -= 1;
            GameManager.Instance.trashSize += 1;
            yield return new WaitForSeconds(0.5f);
        }
    }


    //Handling Flash Eject Button
    public void ClearTrash()
    {
        CloseAllContexts();
        StartCoroutine(StopTrash());
    }
    //Context Menu clear Temp
    public IEnumerator StopTrash()
    { 
        
        yield return new WaitForSeconds(0.5f);
        if(GameManager.Instance.trashSize > 0)
        {
            GameManager.Instance.SystemSpace -= GameManager.Instance.trashSize;
            //PLAYSOUND TRASH
            GameManager.Instance.TrashBinFull = false;
            GameManager.Instance.trashSize = 0;
        }

    }

    //Get JetGet running
    public void DownloadJetGet()
    {
        StartCoroutine(StopDownloadJet());
    }
    public IEnumerator StopDownloadJet()
    {
        yield return new WaitForSeconds(0.5f);
        foreach (GameObject jetget in jetGetObjs)
        {
            jetget.SetActive(true);
        }
        ShowError("JetGet is successfully installed!");
    }
        

    //JetGetStart
    public void JetStart()
    {
        if(!GameManager.Instance.jetStarted)
        {
            StartCoroutine(GameManager.Instance.SpawnDownloads());
            GameManager.Instance.jetStarted = true;
        }

    }

    //JetGet Stop
    public void JetStop()
    {
        GameManager.Instance.JetStop();
    }

    //Handling clicks on any clickable object
    //Click index 1- rightclick 0-leftclick
    public void ClickHandler(GameObject clickObj, int clickIndex)
    {
        name = clickObj.name;

        //Chose how to interact on a click
        switch (name)
        {
            case "FlashDrive":
                if(clickIndex == 1)
                {
                    CloseAllContexts();
                    OpenContext("FlashDrive");
                }
                break;
            case "LocalDrive":
                if (clickIndex == 0)
                {
                    CloseAllContexts();
                    OpenWindow("FolderWindow");
                }
                break;
            case "Folder":
                if (clickIndex == 1)
                {
                    CloseAllContexts();
                    OpenContext("Folder");
                }
                break;
            case "Bin":
                if (clickIndex == 1)
                    OpenContext("Bin");
                break;
            case "Computer":
                if (clickIndex == 0)
                {
                    CloseAllContexts();
                    OpenWindow("MyComputerWindow");
                }
                break;
            case "JetGet":
                if (clickIndex == 0)
                {
                    CloseAllContexts();
                    OpenWindow("JetGet");
                }
                break;
            case "Virus(Clone)":
                if (clickIndex == 1)
                {
                    Debug.Log(clickObj.name);
                    CloseAllContexts();
                    CloseVirus(clickObj);
                }
                break;
            case "CloseWindow":
                if (clickIndex == 0)
                {
                    CloseAllContexts();
                    Destroy(clickObj.transform.parent.parent.gameObject);
                }
                break;
            case "Reset":
                if (clickIndex == 0)
                {
                    SceneManager.LoadScene("Boot");
                }
                break;
            case "Irq":
                if (clickIndex == 0)
                {
                    CloseAllContexts();
                    OpenWindow("IrqHandler");
                }
                break;
            case "FolderIcon":
                if (clickIndex == 0)
                {
                    CloseAllContexts();
                    OpenWindow("FolderWIndow");
                }
                break;
            case "AntiVirus":
                if (clickIndex == 0)
                {
                    CloseAllContexts();
                    OpenWindow("VirusHelp");
                }
                break;
            case "IrqHandler":
                if (clickIndex == 0)
                {
                    if(PlayerPrefs.GetInt("PlotStep",0)>=8 && PlayerPrefs.GetInt("LinkClicked",0)!=1)
                    {
                        CloseAllContexts();
                        DownloadJetGet();
                        PlotManager.Instance.plotTrigger.Invoke();
                        PlayerPrefs.SetInt("LinkClicked",1);
                    }
                    
                }
                break;
            default:
                CloseAllContexts();
                break;
        }
    }




    //MENU ITEMS



    public void NewGame()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("Intro");
    }

    public void Credits()
    {

    }

    public void Exit()
    {
        Application.Quit();
    }
}
