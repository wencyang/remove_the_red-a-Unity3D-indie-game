using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class Stick : MonoBehaviour {

	public GameObject explosion;
	public Renderer rend;
	public float tumble;
	private Rigidbody rb;
	private Transform stickTransform;
	private float x,y,z,d;
	private GameObject temp;
	private bool hitGround;
	void Start ()
	{
		rend = GetComponent<Renderer>();
		rend.material.color=Color.white;
		rb = GetComponent<Rigidbody> ();
		rb.angularVelocity = Random.insideUnitSphere * tumble; 
	}
	void Update () 
	{
		stickTransform = GetComponent<Rigidbody>().transform;
		x = stickTransform.position.x;
		z = stickTransform.position.z;
		d=Mathf.Sqrt (x * x + z * z);
		y = stickTransform.position.y;

		if (d > 1.43f && (y<-1.7f||hitGround)) 
		{
			Instantiate(explosion, transform.position, transform.rotation);
			Destroy(gameObject);
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