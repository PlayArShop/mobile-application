using UnityEngine;
using System.Collections;

public class RowRandom {

	/* The 3 CardBoards */
	public GameObject[] row;

	/* Speed of Shuffling*/
	private float speed;

	/* direction of Translation */
	static public Vector3 T_Direction = new Vector3 (0, 0, 1);

	/* Initial Position */
	private Vector3 INITIAL_FirstPosition;
	private Vector3 INITIAL_MiddlePosition;
	private Vector3 INITIAL_LastPosition;

	/* Cursor on Last (Graphically) card */
	private int CURSOR_Last;

	/* different states */
	private bool BackToPosOverTrue = false;

	public RowRandom(GameObject first, GameObject second , GameObject third)
	{
		row = new GameObject[3];  
		row [0] = first;
		row [1] = second;
		row [2] = third;
		 
		/* Set Speed to a Random Value between 150&3000f */
		speed = Random.Range (1f, 5f);

		/* BackUp Old Position */
		INITIAL_FirstPosition = first.transform.position;
		INITIAL_MiddlePosition = second.transform.position;
		INITIAL_LastPosition = third.transform.position;
	
		/* Initially the last card is the second one in the array */
		CURSOR_Last = 2;
	}

	public GameObject getMiddle()
	{
		int CURSOR_Middle = (CURSOR_Last - 1) < 0 ? 2 : CURSOR_Last - 1 ;
		
		GameObject WinningCard = row [CURSOR_Middle];
		return WinningCard;
	}

	/* Update the card set
	 * (Should be Called at each Frame)
	 */
	public bool UpdateSet()
	{
		if (BackToPosOverTrue) {
			return ShowTheCard();
		}
		if (speed == 0) {
			BackToInitialPosition ();
			return true;
		}
		TransformCorrection ();
		MoveCards ();
		SpeedDown ();
		return true;
	}
	 
	/* Show Up The Card */
	public bool ShowTheCard() 
	{
		/* Determining the MiddleCursor && the FirstCursor */
		int CURSOR_Middle = (CURSOR_Last - 1) < 0 ? 2 : CURSOR_Last - 1 ;

		GameObject WinningCard = row [CURSOR_Middle];
		  
		/* SmoothTime & Velocity */
		float smoothTime = 0.05F;  
		Vector3 velocity = Vector3.zero; 
		         
		Debug.Log ("je bouge la carte");      

		WinningCard.transform.position = Vector3.SmoothDamp (WinningCard.transform.position, new Vector3( INITIAL_MiddlePosition.x , INITIAL_MiddlePosition.y + 0.01f, INITIAL_MiddlePosition.z), ref velocity, smoothTime);
		WinningCard.transform.localScale = Vector3.Lerp (WinningCard.transform.localScale, new Vector3 (0.006f, 0.05f, -0.0059f), Time.deltaTime);

		if ((int)WinningCard.transform.position.magnitude == (int)new Vector3 (INITIAL_MiddlePosition.x, INITIAL_MiddlePosition.y + 0.01f, INITIAL_MiddlePosition.z).magnitude &&
						(int)WinningCard.transform.localScale.magnitude == (int)new Vector3 (0.006f, 0.05f, -0.0059f).magnitude)
						return false; return true;
	} 

	/* Replace CardBoards in Position even in High Speed */
	private void TransformCorrection()
	{
		/* Determining the MiddleCursor && the FirstCursor*/
		int CURSOR_Middle = (CURSOR_Last - 1) < 0 ? 2 : CURSOR_Last - 1 ;
		int CURSOR_First = (CURSOR_Last + 1) > 2 ? 0 : CURSOR_Last + 1;

		/* Does the last card should be moved up ? */
		if (row [CURSOR_Middle].transform.position.y < INITIAL_LastPosition.y && row [CURSOR_Middle].transform.position.z < INITIAL_LastPosition.z) return;

		/* Correcting the position of all Cards depending on their placement */
		row [CURSOR_First].transform.position = INITIAL_MiddlePosition;
		row [CURSOR_Middle].transform.position = INITIAL_LastPosition;
		row [CURSOR_Last].transform.position = INITIAL_FirstPosition;

		/* updating the LastCursor */
		CURSOR_Last = (CURSOR_Last - 1) < 0 ? 2 : CURSOR_Last - 1;
	}

	/* MoveCards graphically */
	private void MoveCards()
	{
		foreach (GameObject card in row) {
			card.transform.Translate( T_Direction  * Time.deltaTime * speed);
		}
	}

	/* Reduce the speed till 0 */
	private void SpeedDown() { speed = (speed - 0.01F) > 0 ? speed - 0.01F : 0; }

	/**
	 * Due to the random of speed, the cards may be not aligned perfectly with the screen, when they stop
	 * they should get smoothly back to a visually comfortable position
	*/
	private void BackToInitialPosition()
	{
		/* SmoothTime & Velocity */
		float smoothTime = 0.05F;  
		Vector3 velocity = Vector3.zero;

		/* Determining the MiddleCursor && the FirstCursor*/
		int CURSOR_Middle = (CURSOR_Last - 1) < 0 ? 2 : CURSOR_Last - 1;
		int CURSOR_First = (CURSOR_Last + 1) > 2 ? 0 : CURSOR_Last + 1;


		if ((int)row [CURSOR_First].transform.position.magnitude == (int)INITIAL_FirstPosition.magnitude && 
						(int)row [CURSOR_Middle].transform.position.magnitude == (int)INITIAL_MiddlePosition.magnitude &&
						(int)row [CURSOR_Last].transform.position.magnitude == (int)INITIAL_LastPosition.magnitude) {
			BackToPosOverTrue = true;
			return; 
		}
		 

		row [CURSOR_First].transform.position = Vector3.SmoothDamp (row [CURSOR_First].transform.position, INITIAL_FirstPosition, ref velocity, smoothTime);
		row [CURSOR_Middle].transform.position = Vector3.SmoothDamp (row [CURSOR_Middle].transform.position, INITIAL_MiddlePosition, ref velocity, smoothTime);
		row [CURSOR_Last].transform.position = Vector3.SmoothDamp (row [CURSOR_Last].transform.position, INITIAL_LastPosition, ref velocity, smoothTime);
	}

}
