using UnityEngine;
using Vuforia;
using System.Collections;
using System.Collections.Generic;

public class BeerPong_gameManager : MonoBehaviour, ICloudRecoEventHandler
{
	private Rect displayRect;
	private float timer = 30.0f;
	private string timerString;
	private bool gameOver = false;
	private CloudRecoBehaviour mCloudRecoBehaviour;
	public ImageTargetBehaviour ImageTargetTemplate;
	private ImageTargetBehaviour imageTargetBehaviour;
	private PSApi.Communication _api;

	public GameObject Ball;
	public GameObject camera;

	private int speed = 100000;
	
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
			imageTargetBehaviour = (ImageTargetBehaviour)tracker.TargetFinder.EnableTracking(targetSearchResult, ImageTargetTemplate.gameObject);
		}
	}

	void Start ()
	{
		Vector3 newGravity = Physics.gravity;
		newGravity.y = -10.00f;
		Physics.gravity = newGravity;
		mCloudRecoBehaviour = GetComponent<CloudRecoBehaviour>();
		if (mCloudRecoBehaviour)
		{
			mCloudRecoBehaviour.RegisterEventHandler(this);
		}
	}

	void Update ()
	{
		// Instantiate a new ball and put it near the camera
		GameObject clone = GameObject.Find("nextBall");
		if (clone == null)
		{
			clone = Instantiate(Ball, camera.transform.position, camera.transform.rotation) as GameObject;
			clone.SetActive(true);
			clone.GetComponent<MeshRenderer>().enabled = false;
			clone.name = "nextBall";
			clone.transform.parent = Ball.transform.parent;
			clone.transform.localScale = Ball.transform.localScale;
		}
		else
		{
			clone.GetComponent<Rigidbody>().transform.position = camera.transform.position;
			clone.GetComponent<Rigidbody>().transform.rotation = camera.transform.rotation;
			clone.GetComponent<Rigidbody>().transform.Translate(0.0f, 10.0f, 30.0f);
		}
		// Shoot the ball
		if (Input.GetButtonDown("Fire1"))
		{
			clone.GetComponent<MeshRenderer>().enabled = true;
			Destroy(clone, 30f);
			clone.SetActive(true);
			clone.name = "Ball";
			clone.GetComponent<Rigidbody>().AddForce(transform.forward * speed);
		}
	}
}
