using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunctionHandler : Singleton<FunctionHandler>
{

    public GameObject menuWindow;
    public GameObject driveMenu;
    public GameObject folderMenu;


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

        CloseAllContexts();

        if (context != null)
        {
            context.SetActive(!context.gameObject.activeSelf);
            context.transform.position = Input.mousePosition;
        }
       
    }

    //Close all menus that are open
    public void CloseAllContexts()
    {
        driveMenu.SetActive(false);
        folderMenu.SetActive(false);
    }


    //Handling clicks on any clickable object
    //Click index 1- rightclick 0-leftclick
    public void ClickHandler(string name, int clickIndex)
    {
       

        switch (name)
        {
            case "FlashDrive":
                if(clickIndex == 1)
                    OpenContext("FlashDrive");
                break;
            case "Folder":
                if (clickIndex == 1)
                    OpenContext("Folder");
                break;
           
            default:
                CloseAllContexts();
                break;
        }
    }
}
