using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

	GameObject highScoreCanvas;
	GameObject mainMenuCanvas;

	public Text highScoreText1;
	public Text highScoreText2;
	public Text highScoreText3;
	public Text highScoreText4;
	public Text highScoreText5;
	public Text highScoreText6;
	public Text highScoreText7;
	public Text highScoreText8;
	public Text highScoreText9;
	public Text highScoreText10;

	public Text dateText1;
	public Text dateText2;
	public Text dateText3;
	public Text dateText4;
	public Text dateText5;
	public Text dateText6;
	public Text dateText7;
	public Text dateText8;
	public Text dateText9;
	public Text dateText10;

    public Text threeText1, fourText1, fiveText1, HVText1, DText1, totalText1;
    public Text threeText2, fourText2, fiveText2, HVText2, DText2, totalText2;
    public Text threeText3, fourText3, fiveText3, HVText3, DText3, totalText3;
    public Text threeText4, fourText4, fiveText4, HVText4, DText4, totalText4;
    public Text threeText5, fourText5, fiveText5, HVText5, DText5, totalText5;
    public Text threeText6, fourText6, fiveText6, HVText6, DText6, totalText6;
    public Text threeText7, fourText7, fiveText7, HVText7, DText7, totalText7;
    public Text threeText8, fourText8, fiveText8, HVText8, DText8, totalText8;
    public Text threeText9, fourText9, fiveText9, HVText9, DText9, totalText9;
    public Text threeText10, fourText10, fiveText10, HVText10, DText10, totalText10;

    Animator highScoreAnimator;
	Animator mainMenuAnimator;

	bool viewingHighScores = false;

	void Awake () {
        
		mainMenuCanvas = this.gameObject;
		highScoreCanvas = GameObject.FindGameObjectWithTag ("HSCanvas");

		mainMenuAnimator = mainMenuCanvas.GetComponent<Animator> ();
		highScoreAnimator = highScoreCanvas.GetComponent<Animator> ();

		highScoreText1.text = PlayerPrefs.GetInt ("HighScore1").ToString ();
		highScoreText2.text = PlayerPrefs.GetInt ("HighScore2").ToString ();
		highScoreText3.text = PlayerPrefs.GetInt ("HighScore3").ToString ();
		highScoreText4.text = PlayerPrefs.GetInt ("HighScore4").ToString ();
		highScoreText5.text = PlayerPrefs.GetInt ("HighScore5").ToString ();
		highScoreText6.text = PlayerPrefs.GetInt ("HighScore6").ToString ();
		highScoreText7.text = PlayerPrefs.GetInt ("HighScore7").ToString ();
		highScoreText8.text = PlayerPrefs.GetInt ("HighScore8").ToString ();
		highScoreText9.text = PlayerPrefs.GetInt ("HighScore9").ToString ();
		highScoreText10.text = PlayerPrefs.GetInt ("HighScore10").ToString ();

		dateText1.text = PlayerPrefs.GetString ("Date1");
		dateText2.text = PlayerPrefs.GetString ("Date2");
		dateText3.text = PlayerPrefs.GetString ("Date3");
		dateText4.text = PlayerPrefs.GetString ("Date4");
		dateText5.text = PlayerPrefs.GetString ("Date5");
		dateText6.text = PlayerPrefs.GetString ("Date6");
		dateText7.text = PlayerPrefs.GetString ("Date7");
		dateText8.text = PlayerPrefs.GetString ("Date8");
		dateText9.text = PlayerPrefs.GetString ("Date9");
		dateText10.text = PlayerPrefs.GetString ("Date10");

        threeText1.text = PlayerPrefs.GetInt("Three1").ToString();
        threeText2.text = PlayerPrefs.GetInt("Three2").ToString();
        threeText3.text = PlayerPrefs.GetInt("Three3").ToString();
        threeText4.text = PlayerPrefs.GetInt("Three4").ToString();
        threeText5.text = PlayerPrefs.GetInt("Three5").ToString();
        threeText6.text = PlayerPrefs.GetInt("Three6").ToString();
        threeText7.text = PlayerPrefs.GetInt("Three7").ToString();
        threeText8.text = PlayerPrefs.GetInt("Three8").ToString();
        threeText9.text = PlayerPrefs.GetInt("Three9").ToString();
        threeText10.text = PlayerPrefs.GetInt("Three10").ToString();

        fourText1.text = PlayerPrefs.GetInt("Four1").ToString();
        fourText2.text = PlayerPrefs.GetInt("Four2").ToString();
        fourText3.text = PlayerPrefs.GetInt("Four3").ToString();
        fourText4.text = PlayerPrefs.GetInt("Four4").ToString();
        fourText5.text = PlayerPrefs.GetInt("Four5").ToString();
        fourText6.text = PlayerPrefs.GetInt("Four6").ToString();
        fourText7.text = PlayerPrefs.GetInt("Four7").ToString();
        fourText8.text = PlayerPrefs.GetInt("Four8").ToString();
        fourText9.text = PlayerPrefs.GetInt("Four9").ToString();
        fourText10.text = PlayerPrefs.GetInt("Four10").ToString();

        fiveText1.text = PlayerPrefs.GetInt("Five1").ToString();
        fiveText2.text = PlayerPrefs.GetInt("Five2").ToString();
        fiveText3.text = PlayerPrefs.GetInt("Five3").ToString();
        fiveText4.text = PlayerPrefs.GetInt("Five4").ToString();
        fiveText5.text = PlayerPrefs.GetInt("Five5").ToString();
        fiveText6.text = PlayerPrefs.GetInt("Five6").ToString();
        fiveText7.text = PlayerPrefs.GetInt("Five7").ToString();
        fiveText8.text = PlayerPrefs.GetInt("Five8").ToString();
        fiveText9.text = PlayerPrefs.GetInt("Five9").ToString();
        fiveText10.text = PlayerPrefs.GetInt("Five10").ToString();

        HVText1.text = PlayerPrefs.GetInt("HV1").ToString();
        HVText2.text = PlayerPrefs.GetInt("HV2").ToString();
        HVText3.text = PlayerPrefs.GetInt("HV3").ToString();
        HVText4.text = PlayerPrefs.GetInt("HV4").ToString();
        HVText5.text = PlayerPrefs.GetInt("HV5").ToString();
        HVText6.text = PlayerPrefs.GetInt("HV6").ToString();
        HVText7.text = PlayerPrefs.GetInt("HV7").ToString();
        HVText8.text = PlayerPrefs.GetInt("HV8").ToString();
        HVText9.text = PlayerPrefs.GetInt("HV9").ToString();
        HVText10.text = PlayerPrefs.GetInt("HV10").ToString();

        DText1.text = PlayerPrefs.GetInt("D1").ToString();
        DText2.text = PlayerPrefs.GetInt("D2").ToString();
        DText3.text = PlayerPrefs.GetInt("D3").ToString();
        DText4.text = PlayerPrefs.GetInt("D4").ToString();
        DText5.text = PlayerPrefs.GetInt("D5").ToString();
        DText6.text = PlayerPrefs.GetInt("D6").ToString();
        DText7.text = PlayerPrefs.GetInt("D7").ToString();
        DText8.text = PlayerPrefs.GetInt("D8").ToString();
        DText9.text = PlayerPrefs.GetInt("D9").ToString();
        DText10.text = PlayerPrefs.GetInt("D10").ToString();

        totalText1.text = PlayerPrefs.GetInt("Total1").ToString();
        totalText2.text = PlayerPrefs.GetInt("Total2").ToString();
        totalText3.text = PlayerPrefs.GetInt("Total3").ToString();
        totalText4.text = PlayerPrefs.GetInt("Total4").ToString();
        totalText5.text = PlayerPrefs.GetInt("Total5").ToString();
        totalText6.text = PlayerPrefs.GetInt("Total6").ToString();
        totalText7.text = PlayerPrefs.GetInt("Total7").ToString();
        totalText8.text = PlayerPrefs.GetInt("Total8").ToString();
        totalText9.text = PlayerPrefs.GetInt("Total9").ToString();
        totalText10.text = PlayerPrefs.GetInt("Total10").ToString();
    }

	void Update () {
	
		if (viewingHighScores == true && Input.anyKeyDown) {
		
			highScoreAnimator.SetTrigger ("HighScoreFadeOut");
			mainMenuAnimator.SetTrigger ("MainMenuFadeIn");
			viewingHighScores = false;
		}
	}
		
	public void Continue () {

		mainMenuAnimator.SetTrigger ("MainMenuFadeOut");
		PlayerPrefs.SetInt ("Continue", 1);
		SceneManager.LoadScene ("GameScene");
	}

	public void NewGame () {
	
		mainMenuAnimator.SetTrigger ("MainMenuFadeOut");
		PlayerPrefs.SetInt ("Continue", 0);
		SceneManager.LoadScene ("GameScene");
	}

	public void HighScores () {

		mainMenuAnimator.SetTrigger ("MainMenuFadeOut");
		highScoreAnimator.SetTrigger ("HighScoreFadeIn");
		viewingHighScores = true;
	}

    public void QuitGame () {
        Application.Quit();
    }

    public void NewGameCheck () {
	
		mainMenuAnimator.SetTrigger ("NewCheckFadeIn");
	}

	public void CancelNewGameCheck () {

		mainMenuAnimator.SetTrigger ("NewCheckFadeOut");
	}

}
