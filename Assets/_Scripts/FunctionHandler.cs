﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunctionHandler : Singleton<FunctionHandler>
{

    public GameObject driveMenu;
    public GameObject folderMenu;
    public GameObject binMenu;

    public GameObject menuWindow;
    public GameObject folderWindow;
    public GameObject compWindow;

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


    //Handling clicks on any clickable object
    //Click index 1- rightclick 0-leftclick
    public void ClickHandler(string name, int clickIndex)
    {
       

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
            default:
                CloseAllContexts();
                break;
        }
    }
}
