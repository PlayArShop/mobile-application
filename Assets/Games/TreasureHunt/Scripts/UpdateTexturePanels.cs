using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UpdateTexturePanels : MonoBehaviour {

	public int textureNumber;
	private bool  once;
	private LoadTargetsPreview scr;

	// Use this for initialization
	void Start () {
		once = false;
		scr = GameObject.Find("GameLogic").GetComponent<LoadTargetsPreview>();
	}
	
	// Update is called once per frame
	void Update () {
		if (!once) {
			if (textureNumber < scr.textures.Length && scr.state[textureNumber] == true && scr.textures[textureNumber])
			{
				Debug.Log(gameObject.name + "new textures" + textureNumber);

				gameObject.GetComponent<RawImage>().texture = scr.textures[textureNumber];
				gameObject.GetComponent<RawImage>().material.mainTexture = scr.textures[textureNumber];				
				once = true;
			}
		}
	}
}
