using UnityEngine;
using System.Collections;

public class RandomRotationOverTime : MonoBehaviour {
    
    private float speed = 10f;
    private int direction = 1;

    public float min = 10f;
    public float max = 100f;

    void changeSpeedDir()
    {
        speed = Random.Range(min, max);

        // if need to change dir
        float value = Random.Range(-1f, 1f);
        if (value > 0)
            direction = 1;
        else direction = -1;
    }

    void Start () {
        InvokeRepeating("changeSpeedDir", 1f, 1f);
    }
	
    
    // Update is called once per frame
    void Update () {
        transform.Rotate(direction * Vector3.forward, speed * Time.deltaTime);
    }
}
