using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TriggerGlassGoal : MonoBehaviour
{
	void OnCollisionEnter (Collision col)
	{
		if(col.gameObject.name == "Ball" || col.gameObject.name == "nextBall")
		{
			Destroy(col.gameObject);
			GameOver.score++;
		}
	}

	void Update ()
	{

	}
}