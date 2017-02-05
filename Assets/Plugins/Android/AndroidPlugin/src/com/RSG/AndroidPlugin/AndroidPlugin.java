package com.RSG.AndroidPlugin;

import android.content.Intent;
import android.content.Context;
import android.content.ComponentName;
import android.net.Uri;

public class AndroidPlugin
{
	private Context context;

	public AndroidPlugin(Context context)
	{
		this.context = context;
	}

    public float AndroidLaunchGoogleMaps()
    {
        Uri gmmIntentUri = Uri.parse("https://www.google.com/maps/@46.7379015,3.2917911,6.5z/data=!4m2!6m1!1s1jDrs80mOYjt18XztzYhnSWhA3Fw");
        Intent mapIntent = new Intent(Intent.ACTION_VIEW, gmmIntentUri);
        mapIntent.setPackage("com.google.android.apps.maps");
        if (mapIntent.resolveActivity(context.getPackageManager()) != null) {
            context.startActivity(mapIntent);
        }
        return 0f;
    }
}