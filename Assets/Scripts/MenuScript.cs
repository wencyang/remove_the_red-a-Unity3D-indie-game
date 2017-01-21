using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SocialPlatforms;

public class MenuScript : MonoBehaviour {

	static string RATE="rate";
	static int mRate;
	public bool isAuthenticated = false;
	public Text resultText;
	public Text usedTimeText;
	public Text timeText;
	public Text levelText,sticksText;
	public Text playText,shareText,rateText,helpText;
	public Button playButton;
	public Button helpButton;
	public Button levelDown;
	public Button levelUp;
	public Button shareButton;
	public Button rateButton;
	public Image menuBackground;
	public bool replay;
	private GameObject obj;
	private GameController gController;
	[HideInInspector]
	public int level;

	void Start () 
	{
		if (PlayerPrefs.HasKey (RATE)) {
			mRate = PlayerPrefs.GetInt (RATE);
		} 
		else {
			PlayerPrefs.SetInt(RATE,mRate);
		}

		resultText.enabled = false;
		usedTimeText.enabled = false;
		timeText.enabled = true;
		levelText.enabled = false;
		sticksText.enabled = false;
		playButton.enabled = false;
		playText.enabled = false;
		shareText.enabled = false;
		rateText.enabled=false;
		helpButton.enabled = true;
		shareButton.enabled = false;
		rateButton.enabled = false;
		levelDown.enabled = false;
		levelDown.image.enabled = false;
		levelUp.enabled = false;
		levelUp.image.enabled = false;
		menuBackground.enabled = false;

		level = 1;
		obj=GameObject.FindGameObjectWithTag("GameController");
		Social.localUser.Authenticate ( success => { if (success) { Debug.Log("==iOS GC authenticate ok"); } else { Debug.Log("==iOS GC authenticate Failed"); } } );
		
	}

	void FixedUpdate ()
	{
		if (Time.time == 5.0) {
			helpText.enabled = false;
		} 
		gController = obj.GetComponent<GameController> ();
		if (gController.lose || gController.win)
		{
			resultText.enabled = true;
			usedTimeText.enabled = true;
			timeText.enabled = false;
			levelText.enabled = true;
			sticksText.enabled = true;
			playButton.enabled = true;
			playText.enabled=true;
			levelDown.enabled = true;
			levelDown.image.enabled = true;
			levelUp.enabled = true;
			levelUp.image.enabled = true;
			menuBackground.enabled = true;
		}
		if (level < 18) {
			levelText.text = "0" + (10 + level * 5).ToString ();
		} else {
			levelText.text=(10+level*5).ToString();
		}
		replay = false;

	}
	public void helpButtonPressed()
	{
		if (!rateText.enabled) {
			helpText.enabled = true;
			rateButton.enabled = true;
			rateText.enabled = true;
			shareButton.enabled = true;
			shareText.enabled = true;
		} else if (rateText.enabled) 
		{
			helpText.enabled = false;
			rateButton.enabled = false;
			rateText.enabled = false;
			shareButton.enabled = false;
			shareText.enabled = false;
		}
	}
	public void shareButtonPressed()
	{
		#if UNITY_IOS
		Social.ShowLeaderboardUI ();
		#endif

	}
	public void rateButtonPressed()
	{
//		if (mRate == -1) 
//		{
//			return;
//		}
        #if UNITY_IOS
		IOSBridge.AddNotification("Rate Me","Enjoy My game?","Later","Yes!","No!");
        #endif
	}
	public void playButtonPressed()
	{
        replay=true;
		resultText.enabled = false;
		usedTimeText.enabled = false;
		timeText.enabled = true;
		levelText.enabled = false;
		sticksText.enabled = false;
		playButton.enabled = false;
		playText.enabled = false;
		levelDown.enabled = false;
		levelDown.image.enabled = false;
		levelUp.enabled = false;
		levelUp.image.enabled = false;
		menuBackground.enabled = false;
	}
	public void levelDownPressed()
	{
		level = Mathf.Max (0,level - 1);
	}
	public void levelUpPressed()
	{
		level = level + 1;
	}
	//get feedback from iOS
	void UserFeedBack(string id)
	{
		int selectedID;
		if(int.TryParse(id,out selectedID))
		{
			switch(selectedID)
			{
			case 1:
				Application.OpenURL("itms-apps://itunes.apple.com/app/id1017432877");
				break;
			case 2:
				mRate=-1;
				PlayerPrefs.SetInt(RATE,mRate);
				break;
			}
		}
	}
}

