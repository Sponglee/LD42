using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FunctionHandler : Singleton<FunctionHandler>
{

    public GameObject driveMenu;
    public GameObject folderMenu;
    public GameObject binMenu;

    public GameObject menuWindow;
    public GameObject folderWindow;
    public GameObject compWindow;

    public GameObject errorWindowPref;


    public GameObject flashDrive;

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
            errorWindowPref.transform.GetChild(0).GetComponentInChildren<Text>().text = errorText;
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
        StartCoroutine(StopEject());
    }

    public IEnumerator StopEject()
    {
        flashDrive.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        GameManager.Instance.FlashSpace = 0;
        flashDrive.SetActive(true);
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
                    //Get window from a button click and reset Error Toggle
                    GameManager.Instance.errorShown = false;
                    Destroy(clickObj.transform.parent.parent.gameObject);
                }
                break;
            default:
                CloseAllContexts();
                break;
        }
    }
}
