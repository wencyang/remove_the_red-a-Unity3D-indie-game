using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Dragable : MonoBehaviour
{
	
	public int normalCollisionCount = 1;
	public float moveLimit = .5f;
	public float collisionMoveFactor = .01f;
	public bool freezeRotationOnDrag = true;
	public Camera cam  ;
	private Rigidbody myRigidbody ;
	private Transform myTransform  ;
	private bool canMove = false;
	private bool gravitySetting ;
	private bool freezeRotationSetting ;
	private float sqrMoveLimit ;
	private int collisionCount = 0;
	private Vector3 screenPoint;
	private Vector3 offset;
	void Start () 
	{
		myRigidbody = GetComponent<Rigidbody>();
		myTransform = transform;
		if (!cam) 
		{
			cam = Camera.main;
		}
		if (!cam) 
		{
			Debug.LogError("Can't find camera tagged MainCamera");
			return;
		}
		sqrMoveLimit = moveLimit * moveLimit;   // Since we're using sqrMagnitude, which is faster than magnitude
	}
	
	void OnMouseDown () 
	{
		canMove = true;
		screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
		offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
		gravitySetting = myRigidbody.useGravity;
		freezeRotationSetting = myRigidbody.freezeRotation;
		myRigidbody.useGravity = false;
		myRigidbody.freezeRotation = freezeRotationOnDrag;
	}
	void OnMouseUp () 
	{
		canMove = false;
		myRigidbody.useGravity = gravitySetting;
		myRigidbody.freezeRotation = freezeRotationSetting;
		if (!myRigidbody.useGravity) 
		{
			Vector3 pos = myTransform.position;
			myTransform.position = pos;
		}
	}
	void OnCollisionEnter (Collision collision) 
	{
		collisionCount++;
	}
	
	void OnCollisionExit () 
	{
		collisionCount--;
	}
	
	void FixedUpdate () 
	{
		if (!canMove)
		{
			return;
		}
		
		myRigidbody.velocity = Vector3.zero;
		myRigidbody.angularVelocity = Vector3.zero;
		
		Vector3 cursorPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
		Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorPoint) + offset;
		Vector3 move=cursorPosition-transform.position;

		if (collisionCount > normalCollisionCount)		
		{
			move = move.normalized*collisionMoveFactor;
		}
		else if (move.sqrMagnitude > sqrMoveLimit) 
		{
			move = move.normalized*moveLimit;
		}
		myRigidbody.MovePosition(myRigidbody.position + move);
	}
}