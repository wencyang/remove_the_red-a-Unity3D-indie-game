using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SocialPlatforms;

public class GameController : MonoBehaviour
{
	public bool isAuthenticated = false;
	public Text timeText;
	public Text usedTimeText;
	public Text resultText;
	[HideInInspector]
	public bool win;
	[HideInInspector]
	public bool lose;
	[HideInInspector]
	public int stickCount;
	[HideInInspector]
	public bool selected;
	private int score;
	private int selectedCount=0;
	private GameObject spObject;
	private GameObject obj;
	private GameObject temp;
	private bool firstTouch;
	private bool wasStable;
	private float offset;
	private float resetTime;
	private float remainingTime;
	private int sum,lastSum;
	private int level;
	private MovingDetector movingDetector;
	private GameSetUp gameSetUp; 
	private MenuScript menuScript;
	private SpecialStick spScript;
	private string leaderboardID="07202015";

	void Start ()
	{ 
		obj=GameObject.FindGameObjectWithTag("Menu");
		menuScript = obj.GetComponent<MenuScript> ();
		level = menuScript.level;

		stickCount = 10 + level * 5;
		win = false;
		lose = false;
		firstTouch = false;
		wasStable = false;
		resetTime = 0.0f;
		gameSetUp= GetComponent<GameSetUp> ();
		Screen.orientation = ScreenOrientation.LandscapeLeft;
		gameSetUp.SetUp (stickCount);
		Social.localUser.Authenticate ( success => { if (success) { Debug.Log("==iOS GC authenticate ok"); } else { Debug.Log("==iOS GC authenticate Failed"); } } );
	}
	

	void DisplayTime()
	{
		if ((Input.touchCount == 0||!wasStable) && !firstTouch) 
		{
			offset = Time.time;
		} 
		else if(!firstTouch)
		{
			firstTouch=true;
		}
		if (!win) 
		{
			remainingTime = Mathf.Max(0.0f,60.0f + offset - Time.time);
		}

		timeText.text = "TIME:" + remainingTime.ToString ()+"S";
	}

	void Judgement()
	{
		spObject = GameObject.FindGameObjectWithTag("SpecialStick");
		spScript = spObject.GetComponent <SpecialStick>();
		win = spScript.outside;

		foreach(GameObject fooObj in GameObject.FindGameObjectsWithTag("Stick"))
		{
			movingDetector = fooObj.GetComponent <MovingDetector>();
			sum=sum+movingDetector.Moving;
		}
		if (lastSum == 0 && sum == 0) 
		{
			wasStable = true;
		} 
		else if (wasStable && sum > 1) 
		{
			lose=true;
		}
		lastSum=sum;
		sum = 0;
	}

	void GameManager()
	{
		if(Input.touchCount!=2)
		{
			Judgement ();
		}

		if (remainingTime ==0.0f&&!lose&&!win) 
		{
			lose=true;
			resultText.text="FAILURE!";
			usedTimeText.text="RUN OUT OF TIME";
		}
		if (lose && remainingTime > 0 && !win) {
			resultText.text = "FAILURE!";
			usedTimeText.text = "YOU MOVED OTHER STICKS";
		} else if (win && !lose && remainingTime != 60f) {
			resultText.text = "SUCCESS!";
			float usedTime = 60f - remainingTime;
			usedTimeText.text = "THE TIME YOU USED:" + usedTime.ToString () + "s";
			score = stickCount;
			//report the score to leaderboard
			bool isGCAuthenticated = Social.localUser.authenticated;
			if (isGCAuthenticated) {
				Social.ReportScore (score, leaderboardID, success => {
					if (success) {
						Debug.Log ("==iOS GC report score ok: " + score + "\n");
					} else {
						Debug.Log ("==iOS GC report score Failed: " + leaderboardID + "\n");
					} });
				
			} else {
				Debug.Log ("==iOS GC can't report score, not authenticated\n");
				
			}
		} else if(win && !lose && remainingTime== 60f)
		{
			lose=true;
			resultText.text="FAILURE!";
			usedTimeText.text="YOU MOVED THE RED BEFORE TIMING";
		}

	}

    public void Selected()
	{
		bool isRotating = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<PinchZoom> ().isRotating;
		if (Input.touchCount == 1 &&!isRotating) {
			Ray ray = Camera.main.ScreenPointToRay (Input.GetTouch (0).position);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit, Mathf.Infinity)&&selectedCount==0) {
				if (hit.collider.transform.gameObject.CompareTag ("Stick")) {
					temp = hit.collider.transform.gameObject;
					temp.GetComponent<Renderer> ().material.color = Color.gray;
					selected = true;
					selectedCount++;
				}
			}
		} else if (temp != null) {
			temp.GetComponent<Renderer> ().material.color = Color.white;
			selected = false;
			selectedCount--;
		} else 
		{
			selected=false;
			selectedCount--;
		}
		if(Input.touchCount==0)
		{		
			selected=false;
			selectedCount=0;
			foreach(GameObject fooObj in GameObject.FindGameObjectsWithTag("Stick"))
			{
				fooObj.GetComponent<Renderer>().material.color=Color.white;
			}
		}
	}

	 public void restart()
	{
		resetTime=Time.time;
		win = false;
		lose = false;
		firstTouch = false;
		wasStable = false;
		foreach(GameObject fooObj in GameObject.FindGameObjectsWithTag("Stick"))
		{
			Destroy(fooObj);
		}
		Destroy(GameObject.FindGameObjectWithTag("SpecialStick"));
		stickCount=10+menuScript.level*5;
		gameSetUp.SetUp (stickCount);
	}

	void FixedUpdate()
	{
		Selected ();
		DisplayTime();
		if(Time.time-resetTime>stickCount/10.0f+2.0f)
		{
			GameManager();
		}
	}
}