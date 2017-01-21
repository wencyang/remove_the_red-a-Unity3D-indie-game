using UnityEngine;
using System.Collections;

public class PinchZoom : MonoBehaviour {

	public float perspectiveZoomSpeed = 0.3f;        // The rate of change of the field of view in perspective mode.
	public float orthoZoomSpeed = 0.3f;        // The rate of change of the orthographic size in orthographic mode.
	public Camera camera;
	public float speedMod ;//a speed modifier
	public bool isRotating;
	private Vector3 point;//the coord to the point where the camera looks at
	private bool Slected;

	void Start () 
	{
		point = new Vector3 (0.0f, -2.0f, 0.0f);//get target's coords
		camera.transform.LookAt(point);
	}

	void Update()
	{
		if (Input.touchCount == 2)
		{
			// Store both touches.
			Touch touchZero = Input.GetTouch(0);
			Touch touchOne = Input.GetTouch(1);
			
			// Find the position in the previous frame of each touch.
			Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
			Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;
			
			// Find the magnitude of the vector (the distance) between the touches in each frame.
			float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
			float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;
			
			// Find the difference in the distances between each frame.
			float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;
			
			// If the camera is orthographic...
			if (camera.orthographic)
			{
				// ... change the orthographic size based on the change in distance between the touches.
				camera.orthographicSize += deltaMagnitudeDiff * orthoZoomSpeed;
				
				// Make sure the orthographic size never drops below zero.
				camera.orthographicSize = Mathf.Clamp(camera.orthographicSize, 45f,70.0f);
			}
			else
			{
				// Otherwise change the field of view based on the change in distance between the touches.
				camera.fieldOfView += deltaMagnitudeDiff * perspectiveZoomSpeed;
				
				// Clamp the field of view to make sure it's between 0 and 180.
				camera.fieldOfView = Mathf.Clamp(camera.fieldOfView, 45f, 70.0f);
			}
		}

		//rotate the camera with one finger when it's on the ground
		bool canRotate = !GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ().selected;
		if (Input.touchCount == 1 && canRotate) {
			Ray ray = Camera.main.ScreenPointToRay (Input.GetTouch (0).position);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit, Mathf.Infinity)) {
				if (hit.transform.tag == "Ground") {
					Touch touchTwo = Input.GetTouch (0);
					Vector2 touchTwoPrevPos = touchTwo.position - touchTwo.deltaPosition;
					float deltaMagnitudeX = touchTwo.position.x - touchTwoPrevPos.x;
					if(deltaMagnitudeX!=0){
						isRotating=true;
					}
					camera.transform.RotateAround (point, Vector3.up, 20 * Time.deltaTime * speedMod *deltaMagnitudeX);
				}
			}

		} else {
			isRotating=false;
		}
     }
}