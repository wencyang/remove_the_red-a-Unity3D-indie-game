using UnityEngine;
using System.Collections;

public class SpecialStick : MonoBehaviour {

	public Renderer rend;
	public float tumble;
	public bool outside;
	private Rigidbody rb;
	private Transform stickTransform;
	private float x,y,z,d;
	private bool hitGround;

	void Start ()
	{
		rend = GetComponent<Renderer>();
		rend.material.color=Color.red;
		rb = GetComponent<Rigidbody> ();
		rb.angularVelocity = Random.insideUnitSphere * tumble; 
		stickTransform = GetComponent<Rigidbody>().transform;
		outside=false;

	}

	void FixedUpdate()
	{
		x=stickTransform.position.x;
		z=stickTransform.position.z;
		y=stickTransform.position.y;

		d=Mathf.Sqrt (x * x + z * z);
		if (d > 1.6f && (y<-1.7f||hitGround)) 
		{
			outside=true;
		}
		hitGround = false;
	}
	void OnCollisionEnter(Collision collision)
	{
		if (collision.collider.CompareTag ("Ground")) 
		{
			hitGround=true;
		}
	}
}
