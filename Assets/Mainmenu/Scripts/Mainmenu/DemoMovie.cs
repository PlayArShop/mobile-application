using UnityEngine;
using System.Collections;

public class DemoMovie : MonoBehaviour
{
	public void StartMovie ()
	{
		#if !UNITY_STANDALONE
		Handheld.PlayFullScreenMovie ("PlayARShop_demo.mp4", Color.black, FullScreenMovieControlMode.CancelOnInput);
		#endif
	}
}
