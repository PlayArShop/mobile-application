using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TriggerBasketGoal : MonoBehaviour
{
	void OnCollisionEnter (Collision col)
	{
		if(col.gameObject.name == "Cracker" || col.gameObject.name == "nextCracker")
		{
			//Destroy(col.gameObject);
			GameOver.score++;
		}
	}

	void Update ()
	{

	}
}