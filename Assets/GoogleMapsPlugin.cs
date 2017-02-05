using UnityEngine;
using System;
using System.Collections;

public class GoogleMapsPlugin : MonoBehaviour
{
    public void LaunchGoogleMaps()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            using (var javaUnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (var currentActivity = javaUnityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    using (var androidPlugin = new AndroidJavaObject("com.RSG.AndroidPlugin.AndroidPlugin", currentActivity))
                    {
                        androidPlugin.Call<float>("AndroidLaunchGoogleMaps");
                    }
                }
            }
        }
    }
}
