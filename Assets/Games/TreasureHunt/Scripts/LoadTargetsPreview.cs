using UnityEngine;
using System.Collections;

public class LoadTargetsPreview : MonoBehaviour {

	public GameObject [] panels;
	public Texture2D [] textures;
	public bool [] state;
	// Use this for initialization
	void Start () {
	
	}

    IEnumerator DownloadImageFromUrl(string url, uint n)
    {
        while (true)
        {
            // Start a download of the given url
		    var www = new WWW(url);
			
            // wait until the download is done
            yield return www;

			if (www.isDone){
			Debug.Log("apply image" + n);
			//panels[n].GetComponent<DefaultAndRainbowMode>().rainbowMode = false;
			
			textures[n] = www.texture;
			Debug.Log(textures[n].width);
			state[n] = true;
			yield break;}
        }
    }

	public void LoadUrls(string [] urls) {
		uint n = 0;
		textures = new Texture2D[urls.Length];
		state = new bool[urls.Length];
		for (int x = 0; x < urls.Length; x++) {
			state[x] = false;
		}
		foreach (GameObject panel in panels) {
			if (n < urls.Length) {
				StartCoroutine(DownloadImageFromUrl(urls[n], n));
			}
			n++;
		}		
	}
}
