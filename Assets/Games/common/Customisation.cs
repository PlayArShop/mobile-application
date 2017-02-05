using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Customisation : MonoBehaviour {
    public GameObject[] Logos;
    public GameObject[] Color1;
    public GameObject[] Color2;
    public GameObject[] CText;
    
    private PSApi.Communication _api;

    IEnumerator DownloadImage(string url)
    {
        while (true)
        {
            // Start a download of the given URL
            var www = new WWW(url);

            // wait until the download is done
            yield return www;

            // assign the downloaded image to the main texture of the object
            foreach (GameObject o in Logos)
            {
                //www.LoadImageIntoTexture((Texture2D)o.GetComponent<Renderer>().material.mainTexture);
                o.GetComponent<Renderer>().material.mainTexture = www.texture;
            }
        }
    }

    void Awake()
    {
        _api = GameObject.Find("@ComAPI").GetComponent<PSApi.Communication>();
    }

    void Start()
    {        
        customLogos();
        customColors();
        customText();
    }

    void customText()
    {
        foreach (GameObject o in CText)
        {
            o.GetComponent<TextMesh>().text = _api.Target.Customization.data.custom;
        }
    }

    void customColors()
    {
        Color color1;
        Color color2;

        ColorUtility.TryParseHtmlString(_api.Target.Customization.data.color1, out color1);
        ColorUtility.TryParseHtmlString(_api.Target.Customization.data.color2, out color2);

        foreach (GameObject o in Color1)
        {
            o.GetComponent<Renderer>().sharedMaterial.color = color1;
        }
        foreach (GameObject o in Color2)
        {
            o.GetComponent<Renderer>().sharedMaterial.color = color2;
        }
    }

    void customLogos()
    {
        StartCoroutine(DownloadImage(_api.Target.Customization.data.logo));
    }
}
