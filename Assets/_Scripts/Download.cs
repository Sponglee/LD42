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

    // Use this for initialization
    void Start () {

        dwnldble.name = "Cat.gif";
        dwnldble.size = Random.Range(1f, 5000f);
        if (gameObject.name != "Virus")
            dwnldble.isVirus = false;  
        
        dwnldble.downloadSpeed = Random.Range(1f, 10f);
        StartCoroutine(StartDownload());

	}

    private IEnumerator StartDownload()
    {
        while(dwnldble.progress<=dwnldble.size)
        {
            dwnldble.progress += dwnldble.downloadSpeed;
            slider.value = dwnldble.progress / dwnldble.size;
            yield return new WaitForSeconds(1);
        }

        if(gameObject.CompareTag("Virus"))
        {
            GameManager.Instance.GameOver = true;
            GameManager.Instance.BannerCheck = true;
        }

        Destroy(gameObject);
    }
	
}
