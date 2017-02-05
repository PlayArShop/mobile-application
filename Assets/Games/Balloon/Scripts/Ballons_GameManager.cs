using UnityEngine;
using Vuforia;
using System.Collections;
using System.Collections.Generic;

public class Ballons_GameManager : MonoBehaviour, ICloudRecoEventHandler
{
	private CloudRecoBehaviour mCloudRecoBehaviour;
	public ImageTargetBehaviour ImageTargetTemplate;
    private PSApi.Communication _api;
	
	public void OnInitialized() 
	{
		Debug.Log ("Cloud Reco initialized");

	}
	
	public void OnInitError(TargetFinder.InitState initError) 
	{
		Debug.Log ("Cloud Reco init error " + initError.ToString());
	}
	
	public void OnUpdateError(TargetFinder.UpdateState updateError) 
	{
		Debug.Log ("Cloud Reco update error " + updateError.ToString());
	}
	
	public void OnStateChanged(bool scanning)
	{
		if (scanning)
		{
			ObjectTracker tracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
			tracker.TargetFinder.ClearTrackables(false);
		}
	}

	public void OnNewSearchResult(TargetFinder.TargetSearchResult targetSearchResult)
	{
		mCloudRecoBehaviour.CloudRecoEnabled = false;
		if (ImageTargetTemplate)
		{
			ObjectTracker tracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
			ImageTargetBehaviour imageTargetBehaviour = (ImageTargetBehaviour)tracker.TargetFinder.EnableTracking(targetSearchResult, ImageTargetTemplate.gameObject);
		}
	}

	void Start ()
	{
        _api = PSApi.Communication.Instance;
        DontDestroyOnLoad(_api);
		mCloudRecoBehaviour = GetComponent<CloudRecoBehaviour>();
		if (mCloudRecoBehaviour)
		{
			mCloudRecoBehaviour.RegisterEventHandler(this);
		}
	}

	void Update ()
	{
		string name = "";
		Ballon myBallon = gameObject.AddComponent<Ballon>() as Ballon;
		StateManager sm = TrackerManager.Instance.GetStateManager();
		IEnumerable<TrackableBehaviour> tbs = sm.GetActiveTrackableBehaviours();
		Screen.orientation = ScreenOrientation.AutoRotation;

		foreach(TrackableBehaviour tb in tbs)
			name = tb.TrackableName;
		if (name != "" && (Random.Range(0, 100) < 3)) {
			myBallon.Create();
		}
		myBallon.SphereDestructor();
	}
}
