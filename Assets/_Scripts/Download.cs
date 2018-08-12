using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct Downloadable
{
    public bool isVirus;
    public float downloadSpeed;
    public string name;
    public float progress;
    public float size;
}




public class Download : MonoBehaviour {

    //properties for this download
    
    public Downloadable dwnldble;

    //ui
    [SerializeField]
    private Text sizeText;
    [SerializeField]
    private Text nameText;
    [SerializeField]
    private Slider slider;



    public string[] downloadNames;
    public string[]  virusNames;

    public string downloadNamesRaw;
    public string virusNamesRaw;
    //For randomizer
    public int downloadNameIndex = 0;
    //For randomizer
    public int virusNameIndex = 0;

    
    // Use this for initialization
    void Start () {
        downloadNamesRaw = "NinjaBurtles.avi," +
               "yYy.avi," +
               "Where's my bike dude?.avi," +
               "Glitcher3.iso," +
               "BlighKnight-Rises.avi," +
               "Cybercrank2077.iso," +
               "Gold_of_War.iso," +
               "KeyneEast_Album.rar," +
               "Mughead.iso," +
               "CatGifsFolder.rar," +
               "SolarWars:Return_of_the_Yedi.mp4," +
               "SolarTrek.mov," +
               "GraveRaider.iso," +
               "JakeBond008.mp4," +
               "LudumDareGamesCollection.zip," +
               "LD42entries.rar," +
               "RedBlueRedemption2.iso," +
               "LD41entries.zip";
        virusNamesRaw = "NotAVirus.exe?," +
                "Trojan777.vab," +
                "FreePrn.7z," +
                "GameOfThornsSeason15.zip," +
                "NotTrojan100percent.exe," +
                "TaskManager.exe," +
                "keygen.exe," +
                "patcher.exe," +
                "WindowsUpdate.msi," +
                "DownloadableGif.zip," +
                "VirusFreeStuff.rar," +
                "FreeCouponVoucher.exe," +
                "ClickMe!.zip," +
                "CheckTHISthing.vab," +
                "NoVirusesHere.zip";
        downloadNames = downloadNamesRaw.Split(',');
        virusNames = virusNamesRaw.Split(',');

      
       
        dwnldble.size = Random.Range(1f, 8f);
        if (gameObject.name != "Virus")
        {
            downloadNameIndex = DownloadRandomizer(downloadNameIndex, downloadNames.Length);
            dwnldble.name = downloadNames[downloadNameIndex];

            dwnldble.isVirus = false;  
        }
        else
        {
            downloadNameIndex = DownloadRandomizer(virusNameIndex, virusNames.Length);
            dwnldble.name = virusNames[virusNameIndex];

        }

        dwnldble.downloadSpeed = Random.Range(dwnldble.size / 10, dwnldble.size/3);

        //Set names and sizes
        gameObject.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = dwnldble.name;
        gameObject.transform.GetChild(1).GetChild(1).GetComponent<Text>().text = dwnldble.size.ToString();

        StartCoroutine(StartDownload());

	}

    private IEnumerator StartDownload()
    {
        while(dwnldble.progress<=dwnldble.size)
        {
            dwnldble.progress += dwnldble.downloadSpeed;
            GameManager.Instance.ProgressChecker += dwnldble.downloadSpeed;
            slider.value = dwnldble.progress / dwnldble.size;

            yield return new WaitForSeconds(1);
        }

        if(gameObject.CompareTag("Virus") && !GameManager.Instance.GameOver)
        {
            StartCoroutine(StopGameOver());
        }

        Destroy(gameObject);
    }
	
    public IEnumerator StopGameOver()
    {
        yield return new WaitForSeconds(0.5f);
        GameManager.Instance.BannerCheck = true;
        GameManager.Instance.GameOver = true;
    }



    public int DownloadRandomizer(int lastValue, int length)
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
