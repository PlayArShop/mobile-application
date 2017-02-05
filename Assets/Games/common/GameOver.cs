using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Vuforia;

public class GameOver : MonoBehaviour {

    private PSApi.Communication _api;
    private float timeLeft = 0.33f;
    private float goalTimer = 38.0f;
    private bool sent = false;

    public static float score = 0.0f;
    public Text text;
    public Transform firework;
    public Transform explosion;
    public Transform spark;
    public GameObject camera;
    public GameObject toDestroy;

    void Awake()
    {
        _api = GameObject.Find("@ComAPI").GetComponent<PSApi.Communication>();
    }

	void Start ()
    {
        DontDestroyOnLoad(_api);
	}
	
	void Update ()
    {
        goalTimer -= Time.deltaTime;
        if (goalTimer <= 8.0f) 
        {
            if (sent == false) {
                _api.postScore("Gagne !", score.ToString(), handleResponsePostScore);
                sent = true;
            }

            // Destroy a given GameObject
            Destroy(toDestroy);

            // Increase the size of the text
            if (text.transform.position.z >= 70.0f)
                text.transform.Translate(0.0f, 0.0f, -5.0f);

            // Generates the fireworks
            timeLeft -= Time.deltaTime;
            if (timeLeft <= 0.0f)
            {
                Vector3 p1 = new Vector3(Random.Range(-25.0f, 25.0f), Random.Range(-25.0f, 25.0f), Random.Range(15.0f, 35.0f));
                Vector3 p2 = new Vector3(Random.Range(-25.0f, 25.0f), Random.Range(-25.0f, 25.0f), Random.Range(0.0f, 30.0f));
                Vector3 p3 = new Vector3(Random.Range(-25.0f, 25.0f), Random.Range(-25.0f, 25.0f), Random.Range(0.0f, 30.0f));

                Transform iFirework = GameObject.Instantiate(firework, p1, Quaternion.identity) as Transform;
                Transform iExplosion = GameObject.Instantiate(explosion, p2, Quaternion.identity) as Transform;
                Transform iSpark = GameObject.Instantiate(spark, p3, Quaternion.identity) as Transform;

                iFirework.transform.parent = camera.transform;
                iExplosion.transform.parent = camera.transform;
                iSpark.transform.parent = camera.transform;

                timeLeft = 0.5f;
            }

            // Come back to the main menu at the end of the countdown
            if (goalTimer <= 0.0f)
            {
                score = 0.0f;
                Application.LoadLevel("@main");
            }
        }
	}

    private int handleResponsePostScore(int r, string err)
    {   
            // Text displayed when the game is over
        // TODO need to handle the result, and show win or loose depending to response
        text.text = _api.Score.result;
        return 1;
    }
}
