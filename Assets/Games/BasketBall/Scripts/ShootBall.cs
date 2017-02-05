using UnityEngine;
using System.Collections;

public class ShootBall : MonoBehaviour {

    public GameObject ballSource;
    public GameObject camera;

    private int speed = 2000;

    // Use this for initialization
	void Start () {
	
	}
    
   
	// Update is called once per frame
	void Update () {
        // When the user click with the left button of his mouse 
        // #TODO change to onTouch for mobile game

        GameObject clone = GameObject.Find("nextBall");
        if (clone == null)
        {
            clone = Instantiate(ballSource, camera.transform.position, camera.transform.rotation) as GameObject;
            clone.SetActive(true);
            clone.GetComponent<MeshRenderer>().enabled = false;
            clone.name = "nextBall";
            clone.transform.parent = ballSource.transform.parent;
            clone.transform.localScale = ballSource.transform.localScale;
            
        }
        else
        {
            clone.GetComponent<Rigidbody>().transform.position = camera.transform.position;
            clone.GetComponent<Rigidbody>().transform.rotation = camera.transform.rotation;
            clone.GetComponent<Rigidbody>().transform.Translate(0.0f, -2.0f, 7.0f);
        }

        if (Input.GetButtonDown("Fire1"))
        {
            clone.GetComponent<MeshRenderer>().enabled = true;
            // Instantiate the projectile at the position and rotation of this transform
             // Give the cloned object an initial velocity along the current
             // object's Z axis
             Destroy(clone, 10.0f);
             clone.SetActive(true);
             // change parent
             clone.name = "Ball";
             // throw the ball
             clone.GetComponent<Rigidbody>().AddRelativeForce(-transform.forward * speed);
        }
	}
}
