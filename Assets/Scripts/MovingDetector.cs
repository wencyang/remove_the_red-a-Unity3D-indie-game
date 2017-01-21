using UnityEngine;
using System.Collections;

public class MovingDetector : MonoBehaviour {
	private Transform myTransform;
	private Vector3 curPos,lastPos;
	private Rigidbody rb;
	public int Moving=0;
	void FixedUpdate () 
	{   rb=GetComponent<Rigidbody> ();
		myTransform=rb.GetComponent<Transform> ();
		curPos = myTransform.position;
		if (curPos == lastPos) {
			Moving=0;
		} else {
			if(Mathf.Abs((curPos-lastPos).magnitude)>0.0025f)
				Moving=1;
		}
		lastPos = curPos;
	}
}
//after stable,start to judge
//sticks not selected, move more than critical distance
