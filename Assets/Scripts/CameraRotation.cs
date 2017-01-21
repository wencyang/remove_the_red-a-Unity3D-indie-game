using UnityEngine;
using System.Collections;

public class CameraRotation : MonoBehaviour 
{
	public Camera ca;
	public float speedMod ;//a speed modifier
	private Vector3 point;
	private bool firstTouch=false;
	private float duration;
	private GameController gameController;

	void Start () 
	{
		point = new Vector3 (0.0f, -2.0f, 0.0f);//get target's coords
		ca.transform.LookAt(point);//makes the camera look to it

		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");

		if (gameControllerObject != null)
		{
			gameController = gameControllerObject.GetComponent <GameController>();
		}
		if (gameController == null)
		{
			Debug.Log ("Cannot find 'GameController' script");
		}
		duration=gameController.stickCount/10.0f+2.0f;
	}
		
	void Update () 
	{//makes the camera rotate around "point" coords, rotating around its Y axis, 20 degrees per second times the speed modifier


		if (Input.touchCount > 0 && !firstTouch) 
		{
			firstTouch = true;
		} 
		if(Time.realtimeSinceStartup<duration&&!firstTouch)
		{
	    	ca.transform.RotateAround (point,Vector3.up,20 * Time.deltaTime * speedMod);
		}
	}
}
