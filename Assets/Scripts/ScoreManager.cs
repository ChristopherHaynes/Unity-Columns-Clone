using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

	Spawner spawner;
	GameObject canvas;
	Animator canvasAnimator;

	public Text score;
	public Text gameOverScore;
	public Text highScoreText1;
    public Text highScoreText2;
    public Text highScoreText3;
    public Text match3s;
    public Text match4s;
    public Text match5s;
    public Text matchHVs;
    public Text matchDs;
    public string scoreAsText;

	public int upDown3Matches = 0;
	public int upDown4Matches = 0;
	public int upDown5Matches = 0;

	public int diagonal3Matches = 0;
	public int diagonal4Matches = 0;
	public int diagonal5Matches = 0;

	public int numberOfMatches = 0;

	public int totalScore = 0;
	int newScore = 0;
	int dividedScore = 0;
	float floatScore = 0f;

	public int highScore1 = 0;
    public int highScore2 = 0;
    public int highScore3 = 0;
    bool highScoreReached = false;

	bool blocksInPlay7 = false;
	bool blocksInPlay8 = false;
	bool blocksInPlay9 = false;

    // Per game count of total number of matches
    public int tot3Match = 0;
    public int tot4Match = 0;
    public int tot5Match = 0;
    public int totHVMatch = 0;
    public int totDMatch = 0;

    void Awake (){

		canvas = GameObject.FindGameObjectWithTag ("Canvas");
		canvasAnimator = canvas.GetComponent<Animator>();
		spawner = this.GetComponent<Spawner> ();
        
        // Set highscore values and text objects
		highScoreText1.text = PlayerPrefs.GetInt ("HighScore1").ToString();
		highScore1 = PlayerPrefs.GetInt ("HighScore1");

        highScoreText2.text = PlayerPrefs.GetInt("HighScore2").ToString();
        highScore2 = PlayerPrefs.GetInt("HighScore2");

        highScoreText3.text = PlayerPrefs.GetInt("HighScore3").ToString();
        highScore3 = PlayerPrefs.GetInt("HighScore3");

        // Set match counter text objects
        match3s.text = tot3Match.ToString();
        match4s.text = tot4Match.ToString();
        match5s.text = tot5Match.ToString();
        matchHVs.text = totHVMatch.ToString();
        matchDs.text = totDMatch.ToString();

        // Update the on screen highscore values
        StartCoroutine(UpdateScore());

        // If no highscores exist, load the historic highscores
        if (PlayerPrefs.GetInt("HighScore1") == 0)
        {
            setHistoricHighscores();
        }
    }

	void Update (){
	
		if (totalScore >= 10000 && blocksInPlay7 == false) {
		
			blocksInPlay7 = true;
			canvasAnimator.SetTrigger ("NewColour");
			spawner.totalBlocksInPlay++;
		}

		if (totalScore >= 30000 && blocksInPlay8 == false) {

			blocksInPlay8 = true;
			canvasAnimator.SetTrigger ("NewColour");
			spawner.totalBlocksInPlay++;
		}

		if (totalScore >= 50000 && blocksInPlay9 == false) {

			blocksInPlay9 = true;
			canvasAnimator.SetTrigger ("NewColour");
			spawner.totalBlocksInPlay++;
		}

        // Update the match counter text objects
        match3s.text = tot3Match.ToString();
        match4s.text = tot4Match.ToString();
        match5s.text = tot5Match.ToString();
        matchHVs.text = totHVMatch.ToString();
        matchDs.text = totDMatch.ToString();
    }

	public void CalculateScore () {

        // Work out the total score for the new matches
		floatScore = ((upDown3Matches * 50f) + (upDown4Matches * 100f) + (upDown5Matches * 200f) +
					(diagonal3Matches * 100f) + (diagonal4Matches * 200f) + (diagonal5Matches * 400f)) *
					(0.8f + (0.2f * numberOfMatches));
		newScore = (int)floatScore;

        // Update the totals for the current number of matches
        tot3Match += (upDown3Matches + diagonal3Matches);
        tot4Match += (upDown4Matches + diagonal4Matches);
        tot5Match += (upDown5Matches + diagonal5Matches);
        totHVMatch += (upDown3Matches + upDown4Matches + upDown5Matches);
        totDMatch += (diagonal3Matches + diagonal4Matches + diagonal5Matches);

        // Reset the match values to zero and update the on screen score
        upDown3Matches = 0; upDown4Matches = 0; upDown5Matches = 0;
		diagonal3Matches = 0; diagonal4Matches = 0; diagonal5Matches = 0;
		numberOfMatches = 0;
		StartCoroutine (UpdateScore ());
	}

	public void SetScoreOnLoad () {

		scoreAsText = totalScore.ToString ();
		score.text = scoreAsText;
	}

	IEnumerator UpdateScore () {
	
		dividedScore = newScore / 10;
		newScore = 0;

		for (int i = 0; i < 10; i++) {
		
			totalScore += dividedScore;

			yield return new WaitForSeconds (0.05f);
			scoreAsText = totalScore.ToString ();
			score.text = scoreAsText;
			gameOverScore.text = scoreAsText;
			if (highScoreReached == true) { highScoreText1.text = scoreAsText; }
		}

        // Update the highscores to include the current game score
		if (totalScore >= highScore3 && totalScore < highScore2 && highScoreReached == false)
        {
			highScore3 = totalScore;
			highScoreText3.text = scoreAsText;
            highScoreText3.color = new Color(0.074f, 0.929f, 0.964f);
        }
        else if (totalScore >= highScore2 && totalScore < highScore1 && highScoreReached == false)
        {
            highScore2 = totalScore;
            highScoreText2.text = scoreAsText;
            highScoreText2.color = new Color(0.074f, 0.929f, 0.964f);

            highScoreText3.text = PlayerPrefs.GetInt("HighScore2").ToString();
            highScore3 = PlayerPrefs.GetInt("HighScore2");
            highScoreText3.color = new Color(0.753f, 0.733f, 0.039f);                   
        }
        else if (totalScore >= highScore1 && highScoreReached == false)
        {
            highScore1 = totalScore;
            highScoreText1.text = scoreAsText;           
            highScoreText1.color = new Color(0.074f, 0.929f, 0.964f);

            highScoreText2.text = PlayerPrefs.GetInt("HighScore1").ToString();
            highScore2 = PlayerPrefs.GetInt("HighScore1");
            highScoreText2.color = new Color(0.753f, 0.733f, 0.039f);

            highScoreText3.text = PlayerPrefs.GetInt("HighScore2").ToString();
            highScore3 = PlayerPrefs.GetInt("HighScore2");
            highScoreText3.color = new Color(0.753f, 0.733f, 0.039f);

            highScoreReached = true;
        }
	}

	public void UpdateHighScores () {
	
		int highScore1 = PlayerPrefs.GetInt ("HighScore1");
		int highScore2 = PlayerPrefs.GetInt ("HighScore2");
		int highScore3 = PlayerPrefs.GetInt ("HighScore3");
		int highScore4 = PlayerPrefs.GetInt ("HighScore4");
		int highScore5 = PlayerPrefs.GetInt ("HighScore5");
		int highScore6 = PlayerPrefs.GetInt ("HighScore6");
		int highScore7 = PlayerPrefs.GetInt ("HighScore7");
		int highScore8 = PlayerPrefs.GetInt ("HighScore8");
		int highScore9 = PlayerPrefs.GetInt ("HighScore9");
		int highScore10 = PlayerPrefs.GetInt ("HighScore10");

		string date1 = PlayerPrefs.GetString ("Date1");
		string date2 = PlayerPrefs.GetString ("Date2");
		string date3 = PlayerPrefs.GetString ("Date3");
		string date4 = PlayerPrefs.GetString ("Date4");
		string date5 = PlayerPrefs.GetString ("Date5");
		string date6 = PlayerPrefs.GetString ("Date6");
		string date7 = PlayerPrefs.GetString ("Date7");
		string date8 = PlayerPrefs.GetString ("Date8");
		string date9 = PlayerPrefs.GetString ("Date9");

        int threes1 = PlayerPrefs.GetInt("Three1");
        int threes2 = PlayerPrefs.GetInt("Three2");
        int threes3 = PlayerPrefs.GetInt("Three3");
        int threes4 = PlayerPrefs.GetInt("Three4");
        int threes5 = PlayerPrefs.GetInt("Three5");
        int threes6 = PlayerPrefs.GetInt("Three6");
        int threes7 = PlayerPrefs.GetInt("Three7");
        int threes8 = PlayerPrefs.GetInt("Three8");
        int threes9 = PlayerPrefs.GetInt("Three9");
        int threes10 = PlayerPrefs.GetInt("Three10");

        int fours1 = PlayerPrefs.GetInt("Four1");
        int fours2 = PlayerPrefs.GetInt("Four2");
        int fours3 = PlayerPrefs.GetInt("Four3");
        int fours4 = PlayerPrefs.GetInt("Four4");
        int fours5 = PlayerPrefs.GetInt("Four5");
        int fours6 = PlayerPrefs.GetInt("Four6");
        int fours7 = PlayerPrefs.GetInt("Four7");
        int fours8 = PlayerPrefs.GetInt("Four8");
        int fours9 = PlayerPrefs.GetInt("Four9");
        int fours10 = PlayerPrefs.GetInt("Four10");

        int fives1 = PlayerPrefs.GetInt("Five1");
        int fives2 = PlayerPrefs.GetInt("Five2");
        int fives3 = PlayerPrefs.GetInt("Five3");
        int fives4 = PlayerPrefs.GetInt("Five4");
        int fives5 = PlayerPrefs.GetInt("Five5");
        int fives6 = PlayerPrefs.GetInt("Five6");
        int fives7 = PlayerPrefs.GetInt("Five7");
        int fives8 = PlayerPrefs.GetInt("Five8");
        int fives9 = PlayerPrefs.GetInt("Five9");
        int fives10 = PlayerPrefs.GetInt("Five10");

        int HV1 = PlayerPrefs.GetInt("HV1");
        int HV2 = PlayerPrefs.GetInt("HV2");
        int HV3 = PlayerPrefs.GetInt("HV3");
        int HV4 = PlayerPrefs.GetInt("HV4");
        int HV5 = PlayerPrefs.GetInt("HV5");
        int HV6 = PlayerPrefs.GetInt("HV6");
        int HV7 = PlayerPrefs.GetInt("HV7");
        int HV8 = PlayerPrefs.GetInt("HV8");
        int HV9 = PlayerPrefs.GetInt("HV9");
        int HV10 = PlayerPrefs.GetInt("HV10");

        int D1 = PlayerPrefs.GetInt("D1");
        int D2 = PlayerPrefs.GetInt("D2");
        int D3 = PlayerPrefs.GetInt("D3");
        int D4 = PlayerPrefs.GetInt("D4");
        int D5 = PlayerPrefs.GetInt("D5");
        int D6 = PlayerPrefs.GetInt("D6");
        int D7 = PlayerPrefs.GetInt("D7");
        int D8 = PlayerPrefs.GetInt("D8");
        int D9 = PlayerPrefs.GetInt("D9");
        int D10 = PlayerPrefs.GetInt("D10");

        int total1 = PlayerPrefs.GetInt("Total1");
        int total2 = PlayerPrefs.GetInt("Total2");
        int total3 = PlayerPrefs.GetInt("Total3");
        int total4 = PlayerPrefs.GetInt("Total4");
        int total5 = PlayerPrefs.GetInt("Total5");
        int total6 = PlayerPrefs.GetInt("Total6");
        int total7 = PlayerPrefs.GetInt("Total7");
        int total8 = PlayerPrefs.GetInt("Total8");
        int total9 = PlayerPrefs.GetInt("Total9");
        int total10 = PlayerPrefs.GetInt("Total10");

        //10th Place
        if (totalScore > highScore10 && totalScore < highScore9) {

			PlayerPrefs.SetInt ("HighScore10", totalScore);
			PlayerPrefs.SetString ("Date10", System.DateTime.Now.ToString("dd-MM-yyyy"));
            PlayerPrefs.SetInt("Three10", tot3Match);
            PlayerPrefs.SetInt("Four10", tot4Match);
            PlayerPrefs.SetInt("Five10", tot5Match);
            PlayerPrefs.SetInt("HV10", totHVMatch);
            PlayerPrefs.SetInt("D10", totDMatch);
            PlayerPrefs.SetInt("Total10", totHVMatch + totDMatch);
        }

		//9th Place
		if (totalScore >= highScore9 && totalScore < highScore8) {

			PlayerPrefs.SetInt ("HighScore10", highScore9);
			PlayerPrefs.SetString ("Date10", date9);
            PlayerPrefs.SetInt("Three10", threes9);
            PlayerPrefs.SetInt("Four10", fours9);
            PlayerPrefs.SetInt("Five10", fives9);
            PlayerPrefs.SetInt("HV10", HV9);
            PlayerPrefs.SetInt("D10", D9);
            PlayerPrefs.SetInt("Total10", total9);

            PlayerPrefs.SetInt ("HighScore9", totalScore);
			PlayerPrefs.SetString ("Date9", System.DateTime.Now.ToString("dd-MM-yyyy"));
            PlayerPrefs.SetInt("Three9", tot3Match);
            PlayerPrefs.SetInt("Four9", tot4Match);
            PlayerPrefs.SetInt("Five9", tot5Match);
            PlayerPrefs.SetInt("HV9", totHVMatch);
            PlayerPrefs.SetInt("D9", totDMatch);
            PlayerPrefs.SetInt("Total9", totHVMatch + totDMatch);
        }

		//8th Place
		if (totalScore >= highScore8 && totalScore < highScore7) {

			PlayerPrefs.SetInt ("HighScore10", highScore9);
			PlayerPrefs.SetString ("Date10", date9);
            PlayerPrefs.SetInt("Three10", threes9);
            PlayerPrefs.SetInt("Four10", fours9);
            PlayerPrefs.SetInt("Five10", fives9);
            PlayerPrefs.SetInt("HV10", HV9);
            PlayerPrefs.SetInt("D10", D9);
            PlayerPrefs.SetInt("Total10", total9);

            PlayerPrefs.SetInt ("HighScore9", highScore8);
			PlayerPrefs.SetString ("Date9", date8);
            PlayerPrefs.SetInt("Three9", threes8);
            PlayerPrefs.SetInt("Four9", fours8);
            PlayerPrefs.SetInt("Five9", fives8);
            PlayerPrefs.SetInt("HV9", HV8);
            PlayerPrefs.SetInt("D9", D8);
            PlayerPrefs.SetInt("Total9", total8);

            PlayerPrefs.SetInt ("HighScore8", totalScore);
			PlayerPrefs.SetString ("Date8", System.DateTime.Now.ToString("dd-MM-yyyy"));
            PlayerPrefs.SetInt("Three8", tot3Match);
            PlayerPrefs.SetInt("Four8", tot4Match);
            PlayerPrefs.SetInt("Five8", tot5Match);
            PlayerPrefs.SetInt("HV8", totHVMatch);
            PlayerPrefs.SetInt("D8", totDMatch);
            PlayerPrefs.SetInt("Total8", totHVMatch + totDMatch);
        }

		//7th Place
		if (totalScore >= highScore7 && totalScore < highScore6) {

            PlayerPrefs.SetInt("HighScore10", highScore9);
            PlayerPrefs.SetString("Date10", date9);
            PlayerPrefs.SetInt("Three10", threes9);
            PlayerPrefs.SetInt("Four10", fours9);
            PlayerPrefs.SetInt("Five10", fives9);
            PlayerPrefs.SetInt("HV10", HV9);
            PlayerPrefs.SetInt("D10", D9);
            PlayerPrefs.SetInt("Total10", total9);

            PlayerPrefs.SetInt("HighScore9", highScore8);
            PlayerPrefs.SetString("Date9", date8);
            PlayerPrefs.SetInt("Three9", threes8);
            PlayerPrefs.SetInt("Four9", fours8);
            PlayerPrefs.SetInt("Five9", fives8);
            PlayerPrefs.SetInt("HV9", HV8);
            PlayerPrefs.SetInt("D9", D8);
            PlayerPrefs.SetInt("Total9", total8);

            PlayerPrefs.SetInt ("HighScore8", highScore7);
			PlayerPrefs.SetString ("Date8", date7);
            PlayerPrefs.SetInt("Three8", threes7);
            PlayerPrefs.SetInt("Four8", fours7);
            PlayerPrefs.SetInt("Five8", fives7);
            PlayerPrefs.SetInt("HV8", HV7);
            PlayerPrefs.SetInt("D8", D7);
            PlayerPrefs.SetInt("Total8", total7);

            PlayerPrefs.SetInt ("HighScore7", totalScore);
			PlayerPrefs.SetString ("Date7", System.DateTime.Now.ToString("dd-MM-yyyy"));
            PlayerPrefs.SetInt("Three7", tot3Match);
            PlayerPrefs.SetInt("Four7", tot4Match);
            PlayerPrefs.SetInt("Five7", tot5Match);
            PlayerPrefs.SetInt("HV7", totHVMatch);
            PlayerPrefs.SetInt("D7", totDMatch);
            PlayerPrefs.SetInt("Total7", totHVMatch + totDMatch);
        }

		//6th Place
		if (totalScore >= highScore6 && totalScore < highScore5) {

            PlayerPrefs.SetInt("HighScore10", highScore9);
            PlayerPrefs.SetString("Date10", date9);
            PlayerPrefs.SetInt("Three10", threes9);
            PlayerPrefs.SetInt("Four10", fours9);
            PlayerPrefs.SetInt("Five10", fives9);
            PlayerPrefs.SetInt("HV10", HV9);
            PlayerPrefs.SetInt("D10", D9);
            PlayerPrefs.SetInt("Total10", total9);

            PlayerPrefs.SetInt("HighScore9", highScore8);
            PlayerPrefs.SetString("Date9", date8);
            PlayerPrefs.SetInt("Three9", threes8);
            PlayerPrefs.SetInt("Four9", fours8);
            PlayerPrefs.SetInt("Five9", fives8);
            PlayerPrefs.SetInt("HV9", HV8);
            PlayerPrefs.SetInt("D9", D8);
            PlayerPrefs.SetInt("Total9", total8);

            PlayerPrefs.SetInt("HighScore8", highScore7);
            PlayerPrefs.SetString("Date8", date7);
            PlayerPrefs.SetInt("Three8", threes7);
            PlayerPrefs.SetInt("Four8", fours7);
            PlayerPrefs.SetInt("Five8", fives7);
            PlayerPrefs.SetInt("HV8", HV7);
            PlayerPrefs.SetInt("D8", D7);
            PlayerPrefs.SetInt("Total8", total7);

            PlayerPrefs.SetInt ("HighScore7", highScore6);
			PlayerPrefs.SetString ("Date7", date6);
            PlayerPrefs.SetInt("Three7", threes6);
            PlayerPrefs.SetInt("Four7", fours6);
            PlayerPrefs.SetInt("Five7", fives6);
            PlayerPrefs.SetInt("HV7", HV6);
            PlayerPrefs.SetInt("D7", D6);
            PlayerPrefs.SetInt("Total7", total6);

            PlayerPrefs.SetInt ("HighScore6", totalScore);
			PlayerPrefs.SetString ("Date6", System.DateTime.Now.ToString("dd-MM-yyyy"));
            PlayerPrefs.SetInt("Three6", tot3Match);
            PlayerPrefs.SetInt("Four6", tot4Match);
            PlayerPrefs.SetInt("Five6", tot5Match);
            PlayerPrefs.SetInt("HV6", totHVMatch);
            PlayerPrefs.SetInt("D6", totDMatch);
            PlayerPrefs.SetInt("Total6", totHVMatch + totDMatch);
        }

		//5th Place
		if (totalScore >= highScore5 && totalScore < highScore4) {

            PlayerPrefs.SetInt("HighScore10", highScore9);
            PlayerPrefs.SetString("Date10", date9);
            PlayerPrefs.SetInt("Three10", threes9);
            PlayerPrefs.SetInt("Four10", fours9);
            PlayerPrefs.SetInt("Five10", fives9);
            PlayerPrefs.SetInt("HV10", HV9);
            PlayerPrefs.SetInt("D10", D9);
            PlayerPrefs.SetInt("Total10", total9);

            PlayerPrefs.SetInt("HighScore9", highScore8);
            PlayerPrefs.SetString("Date9", date8);
            PlayerPrefs.SetInt("Three9", threes8);
            PlayerPrefs.SetInt("Four9", fours8);
            PlayerPrefs.SetInt("Five9", fives8);
            PlayerPrefs.SetInt("HV9", HV8);
            PlayerPrefs.SetInt("D9", D8);
            PlayerPrefs.SetInt("Total9", total8);

            PlayerPrefs.SetInt("HighScore8", highScore7);
            PlayerPrefs.SetString("Date8", date7);
            PlayerPrefs.SetInt("Three8", threes7);
            PlayerPrefs.SetInt("Four8", fours7);
            PlayerPrefs.SetInt("Five8", fives7);
            PlayerPrefs.SetInt("HV8", HV7);
            PlayerPrefs.SetInt("D8", D7);
            PlayerPrefs.SetInt("Total8", total7);

            PlayerPrefs.SetInt("HighScore7", highScore6);
            PlayerPrefs.SetString("Date7", date6);
            PlayerPrefs.SetInt("Three7", threes6);
            PlayerPrefs.SetInt("Four7", fours6);
            PlayerPrefs.SetInt("Five7", fives6);
            PlayerPrefs.SetInt("HV7", HV6);
            PlayerPrefs.SetInt("D7", D6);
            PlayerPrefs.SetInt("Total7", total6);

            PlayerPrefs.SetInt ("HighScore6", highScore5);
			PlayerPrefs.SetString ("Date6", date5);
            PlayerPrefs.SetInt("Three6", threes5);
            PlayerPrefs.SetInt("Four6", fours5);
            PlayerPrefs.SetInt("Five6", fives5);
            PlayerPrefs.SetInt("HV6", HV5);
            PlayerPrefs.SetInt("D6", D5);
            PlayerPrefs.SetInt("Total6", total5);

            PlayerPrefs.SetInt ("HighScore5", totalScore);
			PlayerPrefs.SetString ("Date5", System.DateTime.Now.ToString("dd-MM-yyyy"));
            PlayerPrefs.SetInt("Three5", tot3Match);
            PlayerPrefs.SetInt("Four5", tot4Match);
            PlayerPrefs.SetInt("Five5", tot5Match);
            PlayerPrefs.SetInt("HV5", totHVMatch);
            PlayerPrefs.SetInt("D5", totDMatch);
            PlayerPrefs.SetInt("Total5", totHVMatch + totDMatch);
        }

		//4th Place
		if (totalScore >= highScore4 && totalScore < highScore3) {

            PlayerPrefs.SetInt("HighScore10", highScore9);
            PlayerPrefs.SetString("Date10", date9);
            PlayerPrefs.SetInt("Three10", threes9);
            PlayerPrefs.SetInt("Four10", fours9);
            PlayerPrefs.SetInt("Five10", fives9);
            PlayerPrefs.SetInt("HV10", HV9);
            PlayerPrefs.SetInt("D10", D9);
            PlayerPrefs.SetInt("Total10", total9);

            PlayerPrefs.SetInt("HighScore9", highScore8);
            PlayerPrefs.SetString("Date9", date8);
            PlayerPrefs.SetInt("Three9", threes8);
            PlayerPrefs.SetInt("Four9", fours8);
            PlayerPrefs.SetInt("Five9", fives8);
            PlayerPrefs.SetInt("HV9", HV8);
            PlayerPrefs.SetInt("D9", D8);
            PlayerPrefs.SetInt("Total9", total8);

            PlayerPrefs.SetInt("HighScore8", highScore7);
            PlayerPrefs.SetString("Date8", date7);
            PlayerPrefs.SetInt("Three8", threes7);
            PlayerPrefs.SetInt("Four8", fours7);
            PlayerPrefs.SetInt("Five8", fives7);
            PlayerPrefs.SetInt("HV8", HV7);
            PlayerPrefs.SetInt("D8", D7);
            PlayerPrefs.SetInt("Total8", total7);

            PlayerPrefs.SetInt("HighScore7", highScore6);
            PlayerPrefs.SetString("Date7", date6);
            PlayerPrefs.SetInt("Three7", threes6);
            PlayerPrefs.SetInt("Four7", fours6);
            PlayerPrefs.SetInt("Five7", fives6);
            PlayerPrefs.SetInt("HV7", HV6);
            PlayerPrefs.SetInt("D7", D6);
            PlayerPrefs.SetInt("Total7", total6);

            PlayerPrefs.SetInt("HighScore6", highScore5);
            PlayerPrefs.SetString("Date6", date5);
            PlayerPrefs.SetInt("Three6", threes5);
            PlayerPrefs.SetInt("Four6", fours5);
            PlayerPrefs.SetInt("Five6", fives5);
            PlayerPrefs.SetInt("HV6", HV5);
            PlayerPrefs.SetInt("D6", D5);
            PlayerPrefs.SetInt("Total6", total5);

            PlayerPrefs.SetInt ("HighScore5", highScore4);
			PlayerPrefs.SetString ("Date5", date4);
            PlayerPrefs.SetInt("Three5", threes4);
            PlayerPrefs.SetInt("Four5", fours4);
            PlayerPrefs.SetInt("Five5", fives4);
            PlayerPrefs.SetInt("HV5", HV4);
            PlayerPrefs.SetInt("D5", D4);
            PlayerPrefs.SetInt("Total5", total4);

            PlayerPrefs.SetInt ("HighScore4", totalScore);
			PlayerPrefs.SetString ("Date4", System.DateTime.Now.ToString("dd-MM-yyyy"));
            PlayerPrefs.SetInt("Three4", tot3Match);
            PlayerPrefs.SetInt("Four4", tot4Match);
            PlayerPrefs.SetInt("Five4", tot5Match);
            PlayerPrefs.SetInt("HV4", totHVMatch);
            PlayerPrefs.SetInt("D4", totDMatch);
            PlayerPrefs.SetInt("Total4", totHVMatch + totDMatch);
        }

		//3rd Place
		if (totalScore >= highScore3 && totalScore < highScore2) {

            PlayerPrefs.SetInt("HighScore10", highScore9);
            PlayerPrefs.SetString("Date10", date9);
            PlayerPrefs.SetInt("Three10", threes9);
            PlayerPrefs.SetInt("Four10", fours9);
            PlayerPrefs.SetInt("Five10", fives9);
            PlayerPrefs.SetInt("HV10", HV9);
            PlayerPrefs.SetInt("D10", D9);
            PlayerPrefs.SetInt("Total10", total9);

            PlayerPrefs.SetInt("HighScore9", highScore8);
            PlayerPrefs.SetString("Date9", date8);
            PlayerPrefs.SetInt("Three9", threes8);
            PlayerPrefs.SetInt("Four9", fours8);
            PlayerPrefs.SetInt("Five9", fives8);
            PlayerPrefs.SetInt("HV9", HV8);
            PlayerPrefs.SetInt("D9", D8);
            PlayerPrefs.SetInt("Total9", total8);

            PlayerPrefs.SetInt("HighScore8", highScore7);
            PlayerPrefs.SetString("Date8", date7);
            PlayerPrefs.SetInt("Three8", threes7);
            PlayerPrefs.SetInt("Four8", fours7);
            PlayerPrefs.SetInt("Five8", fives7);
            PlayerPrefs.SetInt("HV8", HV7);
            PlayerPrefs.SetInt("D8", D7);
            PlayerPrefs.SetInt("Total8", total7);

            PlayerPrefs.SetInt("HighScore7", highScore6);
            PlayerPrefs.SetString("Date7", date6);
            PlayerPrefs.SetInt("Three7", threes6);
            PlayerPrefs.SetInt("Four7", fours6);
            PlayerPrefs.SetInt("Five7", fives6);
            PlayerPrefs.SetInt("HV7", HV6);
            PlayerPrefs.SetInt("D7", D6);
            PlayerPrefs.SetInt("Total7", total6);

            PlayerPrefs.SetInt("HighScore6", highScore5);
            PlayerPrefs.SetString("Date6", date5);
            PlayerPrefs.SetInt("Three6", threes5);
            PlayerPrefs.SetInt("Four6", fours5);
            PlayerPrefs.SetInt("Five6", fives5);
            PlayerPrefs.SetInt("HV6", HV5);
            PlayerPrefs.SetInt("D6", D5);
            PlayerPrefs.SetInt("Total6", total5);

            PlayerPrefs.SetInt("HighScore5", highScore4);
            PlayerPrefs.SetString("Date5", date4);
            PlayerPrefs.SetInt("Three5", threes4);
            PlayerPrefs.SetInt("Four5", fours4);
            PlayerPrefs.SetInt("Five5", fives4);
            PlayerPrefs.SetInt("HV5", HV4);
            PlayerPrefs.SetInt("D5", D4);
            PlayerPrefs.SetInt("Total5", total4);

            PlayerPrefs.SetInt ("HighScore4", highScore3);
			PlayerPrefs.SetString ("Date4", date3);
            PlayerPrefs.SetInt("Three4", threes3);
            PlayerPrefs.SetInt("Four4", fours3);
            PlayerPrefs.SetInt("Five4", fives3);
            PlayerPrefs.SetInt("HV4", HV3);
            PlayerPrefs.SetInt("D4", D3);
            PlayerPrefs.SetInt("Total4", total3);

            PlayerPrefs.SetInt ("HighScore3", totalScore);
			PlayerPrefs.SetString ("Date3", System.DateTime.Now.ToString("dd-MM-yyyy"));
            PlayerPrefs.SetInt("Three3", tot3Match);
            PlayerPrefs.SetInt("Four3", tot4Match);
            PlayerPrefs.SetInt("Five3", tot5Match);
            PlayerPrefs.SetInt("HV3", totHVMatch);
            PlayerPrefs.SetInt("D3", totDMatch);
            PlayerPrefs.SetInt("Total3", totHVMatch + totDMatch);
        }

		//2nd Place
		if (totalScore >= highScore2 && totalScore < highScore1) {

            PlayerPrefs.SetInt("HighScore10", highScore9);
            PlayerPrefs.SetString("Date10", date9);
            PlayerPrefs.SetInt("Three10", threes9);
            PlayerPrefs.SetInt("Four10", fours9);
            PlayerPrefs.SetInt("Five10", fives9);
            PlayerPrefs.SetInt("HV10", HV9);
            PlayerPrefs.SetInt("D10", D9);
            PlayerPrefs.SetInt("Total10", total9);

            PlayerPrefs.SetInt("HighScore9", highScore8);
            PlayerPrefs.SetString("Date9", date8);
            PlayerPrefs.SetInt("Three9", threes8);
            PlayerPrefs.SetInt("Four9", fours8);
            PlayerPrefs.SetInt("Five9", fives8);
            PlayerPrefs.SetInt("HV9", HV8);
            PlayerPrefs.SetInt("D9", D8);
            PlayerPrefs.SetInt("Total9", total8);

            PlayerPrefs.SetInt("HighScore8", highScore7);
            PlayerPrefs.SetString("Date8", date7);
            PlayerPrefs.SetInt("Three8", threes7);
            PlayerPrefs.SetInt("Four8", fours7);
            PlayerPrefs.SetInt("Five8", fives7);
            PlayerPrefs.SetInt("HV8", HV7);
            PlayerPrefs.SetInt("D8", D7);
            PlayerPrefs.SetInt("Total8", total7);

            PlayerPrefs.SetInt("HighScore7", highScore6);
            PlayerPrefs.SetString("Date7", date6);
            PlayerPrefs.SetInt("Three7", threes6);
            PlayerPrefs.SetInt("Four7", fours6);
            PlayerPrefs.SetInt("Five7", fives6);
            PlayerPrefs.SetInt("HV7", HV6);
            PlayerPrefs.SetInt("D7", D6);
            PlayerPrefs.SetInt("Total7", total6);

            PlayerPrefs.SetInt("HighScore6", highScore5);
            PlayerPrefs.SetString("Date6", date5);
            PlayerPrefs.SetInt("Three6", threes5);
            PlayerPrefs.SetInt("Four6", fours5);
            PlayerPrefs.SetInt("Five6", fives5);
            PlayerPrefs.SetInt("HV6", HV5);
            PlayerPrefs.SetInt("D6", D5);
            PlayerPrefs.SetInt("Total6", total5);

            PlayerPrefs.SetInt("HighScore5", highScore4);
            PlayerPrefs.SetString("Date5", date4);
            PlayerPrefs.SetInt("Three5", threes4);
            PlayerPrefs.SetInt("Four5", fours4);
            PlayerPrefs.SetInt("Five5", fives4);
            PlayerPrefs.SetInt("HV5", HV4);
            PlayerPrefs.SetInt("D5", D4);
            PlayerPrefs.SetInt("Total5", total4);

            PlayerPrefs.SetInt("HighScore4", highScore3);
            PlayerPrefs.SetString("Date4", date3);
            PlayerPrefs.SetInt("Three4", threes3);
            PlayerPrefs.SetInt("Four4", fours3);
            PlayerPrefs.SetInt("Five4", fives3);
            PlayerPrefs.SetInt("HV4", HV3);
            PlayerPrefs.SetInt("D4", D3);
            PlayerPrefs.SetInt("Total4", total3);

            PlayerPrefs.SetInt ("HighScore3", highScore2);
			PlayerPrefs.SetString ("Date3", date2);
            PlayerPrefs.SetInt("Three3", threes2);
            PlayerPrefs.SetInt("Four3", fours2);
            PlayerPrefs.SetInt("Five3", fives2);
            PlayerPrefs.SetInt("HV3", HV2);
            PlayerPrefs.SetInt("D3", D2);
            PlayerPrefs.SetInt("Total3", total2);

            PlayerPrefs.SetInt ("HighScore2", totalScore);
			PlayerPrefs.SetString ("Date2", System.DateTime.Now.ToString("dd-MM-yyyy"));
            PlayerPrefs.SetInt("Three2", tot3Match);
            PlayerPrefs.SetInt("Four2", tot4Match);
            PlayerPrefs.SetInt("Five2", tot5Match);
            PlayerPrefs.SetInt("HV2", totHVMatch);
            PlayerPrefs.SetInt("D2", totDMatch);
            PlayerPrefs.SetInt("Total2", totHVMatch + totDMatch);
        }

		//1st Place
		if (totalScore >= highScore1) {

            PlayerPrefs.SetInt("HighScore10", highScore9);
            PlayerPrefs.SetString("Date10", date9);
            PlayerPrefs.SetInt("Three10", threes9);
            PlayerPrefs.SetInt("Four10", fours9);
            PlayerPrefs.SetInt("Five10", fives9);
            PlayerPrefs.SetInt("HV10", HV9);
            PlayerPrefs.SetInt("D10", D9);
            PlayerPrefs.SetInt("Total10", total9);

            PlayerPrefs.SetInt("HighScore9", highScore8);
            PlayerPrefs.SetString("Date9", date8);
            PlayerPrefs.SetInt("Three9", threes8);
            PlayerPrefs.SetInt("Four9", fours8);
            PlayerPrefs.SetInt("Five9", fives8);
            PlayerPrefs.SetInt("HV9", HV8);
            PlayerPrefs.SetInt("D9", D8);
            PlayerPrefs.SetInt("Total9", total8);

            PlayerPrefs.SetInt("HighScore8", highScore7);
            PlayerPrefs.SetString("Date8", date7);
            PlayerPrefs.SetInt("Three8", threes7);
            PlayerPrefs.SetInt("Four8", fours7);
            PlayerPrefs.SetInt("Five8", fives7);
            PlayerPrefs.SetInt("HV8", HV7);
            PlayerPrefs.SetInt("D8", D7);
            PlayerPrefs.SetInt("Total8", total7);

            PlayerPrefs.SetInt("HighScore7", highScore6);
            PlayerPrefs.SetString("Date7", date6);
            PlayerPrefs.SetInt("Three7", threes6);
            PlayerPrefs.SetInt("Four7", fours6);
            PlayerPrefs.SetInt("Five7", fives6);
            PlayerPrefs.SetInt("HV7", HV6);
            PlayerPrefs.SetInt("D7", D6);
            PlayerPrefs.SetInt("Total7", total6);

            PlayerPrefs.SetInt("HighScore6", highScore5);
            PlayerPrefs.SetString("Date6", date5);
            PlayerPrefs.SetInt("Three6", threes5);
            PlayerPrefs.SetInt("Four6", fours5);
            PlayerPrefs.SetInt("Five6", fives5);
            PlayerPrefs.SetInt("HV6", HV5);
            PlayerPrefs.SetInt("D6", D5);
            PlayerPrefs.SetInt("Total6", total5);

            PlayerPrefs.SetInt("HighScore5", highScore4);
            PlayerPrefs.SetString("Date5", date4);
            PlayerPrefs.SetInt("Three5", threes4);
            PlayerPrefs.SetInt("Four5", fours4);
            PlayerPrefs.SetInt("Five5", fives4);
            PlayerPrefs.SetInt("HV5", HV4);
            PlayerPrefs.SetInt("D5", D4);
            PlayerPrefs.SetInt("Total5", total4);

            PlayerPrefs.SetInt("HighScore4", highScore3);
            PlayerPrefs.SetString("Date4", date3);
            PlayerPrefs.SetInt("Three4", threes3);
            PlayerPrefs.SetInt("Four4", fours3);
            PlayerPrefs.SetInt("Five4", fives3);
            PlayerPrefs.SetInt("HV4", HV3);
            PlayerPrefs.SetInt("D4", D3);
            PlayerPrefs.SetInt("Total4", total3);

            PlayerPrefs.SetInt("HighScore3", highScore2);
            PlayerPrefs.SetString("Date3", date2);
            PlayerPrefs.SetInt("Three3", threes2);
            PlayerPrefs.SetInt("Four3", fours2);
            PlayerPrefs.SetInt("Five3", fives2);
            PlayerPrefs.SetInt("HV3", HV2);
            PlayerPrefs.SetInt("D3", D2);
            PlayerPrefs.SetInt("Total3", total2);

            PlayerPrefs.SetInt ("HighScore2", highScore1);
			PlayerPrefs.SetString ("Date2", date1);
            PlayerPrefs.SetInt("Three2", threes1);
            PlayerPrefs.SetInt("Four2", fours1);
            PlayerPrefs.SetInt("Five2", fives1);
            PlayerPrefs.SetInt("HV2", HV1);
            PlayerPrefs.SetInt("D2", D1);
            PlayerPrefs.SetInt("Total2", total1);

            PlayerPrefs.SetInt ("HighScore1", totalScore);
			PlayerPrefs.SetString ("Date1", System.DateTime.Now.ToString("dd-MM-yyyy"));
            PlayerPrefs.SetInt("Three1", tot3Match);
            PlayerPrefs.SetInt("Four1", tot4Match);
            PlayerPrefs.SetInt("Five1", tot5Match);
            PlayerPrefs.SetInt("HV1", totHVMatch);
            PlayerPrefs.SetInt("D1", totDMatch);
            PlayerPrefs.SetInt("Total1", totHVMatch + totDMatch);
        }
	}

    public void setHistoricHighscores() {
        
        PlayerPrefs.SetInt("HighScore1", 835690);
        PlayerPrefs.SetString("Date1", "23-01-2019");
        PlayerPrefs.SetInt("Three1", 8985);
        PlayerPrefs.SetInt("Four1", 557);
        PlayerPrefs.SetInt("Five1", 21);
        PlayerPrefs.SetInt("HV1", 5615);
        PlayerPrefs.SetInt("D1", 3948);
        PlayerPrefs.SetInt("Total1", 9563);

        PlayerPrefs.SetInt("HighScore2", 700190);
        PlayerPrefs.SetString("Date2", "27-11-2018");
        PlayerPrefs.SetInt("Three2", 7474);
        PlayerPrefs.SetInt("Four2", 469);
        PlayerPrefs.SetInt("Five2", 36);
        PlayerPrefs.SetInt("HV2", 4702);
        PlayerPrefs.SetInt("D2", 3277);
        PlayerPrefs.SetInt("Total2", 7979);

        PlayerPrefs.SetInt("HighScore3", 565210);
        PlayerPrefs.SetString("Date3", "19-08-2018");

        PlayerPrefs.SetInt("HighScore4", 553730);
        PlayerPrefs.SetString("Date4", "22-05-2019");
        PlayerPrefs.SetInt("Three4", 5885);
        PlayerPrefs.SetInt("Four4", 376);
        PlayerPrefs.SetInt("Five4", 17);
        PlayerPrefs.SetInt("HV4", 3678);
        PlayerPrefs.SetInt("D4", 2600);
        PlayerPrefs.SetInt("Total4", 6278);

        PlayerPrefs.SetInt("HighScore5", 516620);
        PlayerPrefs.SetString("Date5", "01-04-2019");
        PlayerPrefs.SetInt("Three5", 5567);
        PlayerPrefs.SetInt("Four5", 315);
        PlayerPrefs.SetInt("Five5", 17);
        PlayerPrefs.SetInt("HV5", 3435);
        PlayerPrefs.SetInt("D5", 2464);
        PlayerPrefs.SetInt("Total5", 5899);

        PlayerPrefs.SetInt("HighScore6", 354460);
        PlayerPrefs.SetString("Date6", "24-09-2017");

        PlayerPrefs.SetInt("HighScore7", 300070);
        PlayerPrefs.SetString("Date7", "02-03-2018");

        PlayerPrefs.SetInt("HighScore8", 284980);
        PlayerPrefs.SetString("Date8", "17-05-2018");

        PlayerPrefs.SetInt("HighScore9", 215950);
        PlayerPrefs.SetString("Date9", "07-07-2018");

        PlayerPrefs.SetInt("HighScore10", 196650);
        PlayerPrefs.SetString("Date10", "06-02-2019");
        PlayerPrefs.SetInt("Three10", 2070);
        PlayerPrefs.SetInt("Four10", 130);
        PlayerPrefs.SetInt("Five10", 10);
        PlayerPrefs.SetInt("HV10", 1284);
        PlayerPrefs.SetInt("D10", 926);
        PlayerPrefs.SetInt("Total10", 2210);
    }
}
