using UnityEngine;
using UnityEngine.UI;

using System.Collections;

public class DefaultAndRainbowMode : MonoBehaviour {

    public Color mainColor = new Color(42, 115, 90);
    public bool rainbowMode = false;

	// Use this for initialization
	void Start () {
        if (gameObject.GetComponent<Image>())
            gameObject.GetComponent<Image>().color = mainColor;
	    else if (gameObject.GetComponent<RawImage>())
            gameObject.GetComponent<RawImage>().color = mainColor;
	    else if (gameObject.GetComponent<Text>())
            gameObject.GetComponent<Text>().color = mainColor;            
}
	
	// Update is called once per frame
	void Update () {
        if (!rainbowMode)
        {
          if (gameObject.GetComponent<Image>()) {gameObject.GetComponent<Image>().color = mainColor;}
	      else if (gameObject.GetComponent<RawImage>()) {gameObject.GetComponent<RawImage>().color = mainColor;}            
          return;
        }
        if (gameObject.GetComponent<RawImage>())
           gameObject.GetComponent<RawImage>().color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
        else if (gameObject.GetComponent<Image>())
          gameObject.GetComponent<Image>().color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
        else if (gameObject.GetComponent<Text>())
          gameObject.GetComponent<Text>().color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
    }
}
