using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class GameOver : MonoBehaviour {

	ScoreManager scoreManager;
	Grid grid;

	public bool isGameOver = false;

	void Start () {

		scoreManager = this.GetComponent<ScoreManager> ();
		grid = this.GetComponent<Grid> ();
	}

	public void StartGameOver () {

		isGameOver = true;
		GameObject canvas = GameObject.FindGameObjectWithTag("Canvas");
		Animator anim = canvas.GetComponent<Animator> ();

		anim.SetTrigger ("GameOver");
		scoreManager.UpdateHighScores ();
		grid.SaveClear ();
	}

	public void RestartButton () {
	
		PlayerPrefs.SetInt ("Continue", 0);
		SceneManager.LoadScene ("GameScene");
	}

	public void Quit () {

		SceneManager.LoadScene ("MainMenu");
	}
}
