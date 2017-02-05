using UnityEngine;
using System.Collections;

public class CustomImage : MonoBehaviour {

	// Use this for initialization
    // Continuously get the latest webcam shot from outside "Friday's" in Times Square
    // and DXT compress them at runtime
    public string url = "http://www.abondance.com/Bin/g-4-couleurs-google.png";

    IEnumerator DownloadImage(string url)
    {
        while (true)
        {
            // Start a download of the given URL
            var www = new WWW(url);

            // wait until the download is done
            yield return www;

            // assign the downloaded image to the main texture of the object
            www.LoadImageIntoTexture((Texture2D)GetComponent<Renderer>().material.mainTexture);
        }
    }

    void Start()
    {
        // Create a texture in DXT1 format
        GetComponent<Renderer>().material.mainTexture = new Texture2D(4, 4, TextureFormat.DXT1, false);
        // Start Routine
        StartCoroutine(DownloadImage(url));
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
