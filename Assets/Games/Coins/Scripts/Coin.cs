using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour
{
	private GameObject sphere;
	private RaycastHit hit;
	private Ray ray;
	
	void Start ()
	{

	}

	public void Create()
	{
		SphereCollider sphereCollider;

		sphere = Instantiate(Resources.Load("Coin")) as GameObject;
		sphere.AddComponent<Rigidbody>();
		sphere.transform.Rotate(180f, 0, 0);
		sphere.transform.position = new Vector3((float)Random.Range(-30, 30), -80, (float)Random.Range(30, 30));
		sphereCollider = sphere.AddComponent<SphereCollider>();
		sphereCollider.radius = 0.60f;
		sphereCollider = sphere.AddComponent<SphereCollider>();
	}

	void Update()
	{
		if (sphere && (sphere.transform.position.y > 100))
			Destroy(sphere);
	}

	public void SphereDestructor ()
	{
		if((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Stationary) || (Input.GetMouseButtonDown(0)))
		{
			if (Application.platform == RuntimePlatform.Android)
				ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
			else
				ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if(Physics.Raycast(ray,out hit))
			{
				GameOver.score++;
				Destroy(hit.transform.gameObject);
			}
		}
	}
}