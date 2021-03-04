using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spawner : MonoBehaviour {

	Grid grid;
	SoundManager soundManager;
	GameOver gameOver;

	//Block object is declared
	public GameObject[] blocks;
	public GameObject[,] placedBlocks = new GameObject[10, 15];

	public GameObject blockCentre;
	public GameObject blockLeft;
	public GameObject blockRight;

	public GameObject nextBlockCentre;
	public GameObject nextBlockLeft;
	public GameObject nextBlockRight;

	public int bundlePosition = 0;
	public int newBundlePosition = 0;

	public int totalBlocksInPlay = 6;

	bool leftSettled = false, centreSettled = false, rightSettled = false;
	bool leftMoving = false, centreMoving = false, rightMoving = false;

	int leftX, leftY;
	int centreX, centreY;
	int rightX, rightY;

	public bool moveLeftTouch = false, moveRightTouch = false, dropTouch = false;
	public bool turnLeftTouch = false, turnRightTouch = false;

	bool leftSoundPlayed = false, centreSoundPlayed = false, rightSoundPlayed = false;

	public bool saveAvailable = false;

	//Public function for spawning a random block
	public void SpawnNext ()  {

		bundlePosition = 0;
		newBundlePosition = 0;
		int i = Random.Range(0, totalBlocksInPlay);

		nextBlockCentre = (GameObject)Instantiate(blocks[i], transform.localPosition = new Vector3(12.5f,12,0f), Quaternion.identity);

		i = Random.Range(0, totalBlocksInPlay);

		nextBlockLeft = (GameObject)Instantiate(blocks[i], transform.localPosition = new Vector3(11.5f,12,0f), Quaternion.identity);

		i = Random.Range(0, totalBlocksInPlay);

		nextBlockRight = (GameObject)Instantiate(blocks[i], transform.localPosition = new Vector3(13.5f,12,0f), Quaternion.identity);

		saveAvailable = true;
	}

	public void NextToPlay () {

		nextBlockLeft.transform.localPosition = new Vector3 (4, 14, 0f);
		nextBlockCentre.transform.localPosition = new Vector3 (5, 14, 0f);
		nextBlockRight.transform.localPosition = new Vector3 (6, 14, 0f);

		blockLeft = nextBlockLeft;
		blockCentre = nextBlockCentre;
		blockRight = nextBlockRight;

		SpawnNext ();
	}
		
	void Start () 
	{
		grid = this.GetComponent<Grid> ();
		soundManager = this.GetComponent<SoundManager> ();
		gameOver = this.GetComponent<GameOver> ();

		if (PlayerPrefs.GetInt ("Continue") == 0) {
			SpawnNext ();
			NextToPlay ();
			grid.Save (false);
		}

		if (PlayerPrefs.GetInt ("Continue") == 1) {
			grid.Load ();
		}
	}
		
	void Update () {

		bool leftToSpawn = false, centreToSpawn = false, rightToSpawn = false;
		float dropSpeed = 10f;

		if (leftMoving == false && rightMoving == false && centreMoving == false && grid.startDrop == false && grid.droppedBox == false && gameOver.isGameOver == false) {

			RotateBundle ();
			MoveBundle ();
		}

		if ((Input.GetKeyDown (KeyCode.Space) || dropTouch == true) && leftMoving == false && rightMoving == false && centreMoving == false  && grid.startDrop == false && grid.droppedBox == false && gameOver.isGameOver == false) { 

			dropTouch = false;
			DropBundle (); 
		}

		if (leftSettled == true || rightSettled == true || centreSettled == true) {
			if ((int)blockLeft.transform.localPosition.y >= leftY) {

				blockLeft.transform.localPosition += (new Vector3 (0, -dropSpeed, 0f) * Time.deltaTime); 
			} else {

				if (leftToSpawn == false) {
					blockLeft.transform.position = new Vector3 (leftX, leftY - 0.01f, 0f);
					if (leftSoundPlayed == false) { leftSoundPlayed = true; soundManager.BlockHit (); }
					leftSettled = false; leftToSpawn = true; leftMoving = false;
				}
			}
			if ((int)blockRight.transform.localPosition.y >= rightY) {

				blockRight.transform.localPosition +=(new Vector3 (0, -dropSpeed, 0f) * Time.deltaTime); 
			}else {

				if (rightToSpawn == false) {
					blockRight.transform.position = new Vector3 (rightX, rightY - 0.01f, 0f);
					if (rightSoundPlayed == false) { rightSoundPlayed = true; soundManager.BlockHit (); }  
					rightSettled = false; rightToSpawn = true; rightMoving = false;
				}
			}
			if ((int)blockCentre.transform.localPosition.y >= centreY) {

				blockCentre.transform.localPosition += (new Vector3 (0, -dropSpeed, 0f) * Time.deltaTime); 
			}else {

				if (centreToSpawn == false) {
					blockCentre.transform.position = new Vector3 (centreX, centreY - 0.01f, 0f);
					if (centreSoundPlayed == false) { centreSoundPlayed = true; soundManager.BlockHit (); } 
					centreSettled = false; centreToSpawn = true;	centreMoving = false;
				}
			}
		}

		GameOverCheck ();

		if (leftToSpawn == true && rightToSpawn == true && centreToSpawn == true && gameOver.isGameOver == true) {
		
			gameOver.StartGameOver ();
		}

		if (leftToSpawn == true && rightToSpawn == true && centreToSpawn == true && gameOver.isGameOver == false) {

			grid.MatchCheck ();

			leftSoundPlayed = false; rightSoundPlayed = false; centreSoundPlayed = false;
			leftToSpawn = false; rightToSpawn = false; centreToSpawn = false;
		}
	}

	void DropBundle () {

		saveAvailable = false;
		leftX = (int)blockLeft.transform.localPosition.x; leftY = (int)blockLeft.transform.localPosition.y;
		centreX = (int)blockCentre.transform.localPosition.x; centreY = (int)blockCentre.transform.localPosition.y;
		rightX = (int)blockRight.transform.localPosition.x; rightY = (int)blockRight.transform.localPosition.y;

		leftMoving = true; rightMoving = true; centreMoving = true;

		while (leftSettled != true || rightSettled != true || centreSettled != true) {

			if (bundlePosition == 1 || bundlePosition == 2) {

				if (grid.grid [leftX, (leftY) - 1] == 0 && leftY > 1) {

					leftY = leftY - 1;
				} else {
					SetGrid (leftX, leftY, int.Parse (blockLeft.tag), blockLeft); leftSettled = true;
				}

				if (grid.grid [centreX, (centreY) - 1] == 0 && centreY > 1) {

					centreY = centreY - 1;
				} else {
					SetGrid (centreX, centreY, int.Parse (blockCentre.tag), blockCentre); centreSettled = true;
				}

				if (grid.grid [rightX, (rightY) - 1] == 0 && rightY > 1) {

					rightY = rightY - 1;
				} else {
					SetGrid (rightX, rightY, int.Parse (blockRight.tag), blockRight); rightSettled = true;
				}
			}

			if (bundlePosition == 3 || bundlePosition ==  0) {

				if (grid.grid [rightX, (rightY) - 1] == 0 && rightY > 1) {

					rightY = rightY - 1;
				} else {
					SetGrid (rightX, rightY, int.Parse (blockRight.tag), blockRight); rightSettled = true;
				}

				if (grid.grid [centreX, (centreY) - 1] == 0 && centreY > 1) {

					centreY = centreY - 1;
				} else {
					SetGrid (centreX, centreY, int.Parse (blockCentre.tag), blockCentre); centreSettled = true;
				}
					
				if (grid.grid [leftX, (leftY) - 1] == 0 && leftY > 1) {

					leftY = leftY - 1;
				} else {
					SetGrid (leftX, leftY, int.Parse (blockLeft.tag), blockLeft); leftSettled = true;
				}
			}
		}
	}

	public void SetGrid (int blockX, int blockY, int tag, GameObject block) {

		if (grid.grid [blockX, blockY] == 0) {
			
			if (tag == 1) {	grid.grid [blockX, blockY] = 1;}
			if (tag == 2) {	grid.grid [blockX, blockY] = 2;}
			if (tag == 3) {	grid.grid [blockX, blockY] = 3;}
			if (tag == 4) {	grid.grid [blockX, blockY] = 4;}
			if (tag == 5) {	grid.grid [blockX, blockY] = 5;}
			if (tag == 6) {	grid.grid [blockX, blockY] = 6;}
			if (tag == 7) {	grid.grid [blockX, blockY] = 7;}
			if (tag == 8) {	grid.grid [blockX, blockY] = 8;}
			if (tag == 9) {	grid.grid [blockX, blockY] = 9;}

			placedBlocks [blockX, blockY] = block;
		} 
	}

	void MoveBundle () {
	
		if (blockLeft.transform.localPosition.x > 0 && blockRight.transform.localPosition.x > 0) {
			if (Input.GetKeyDown (KeyCode.LeftArrow) || moveLeftTouch == true) {
				blockLeft.transform.localPosition += new Vector3 (-1, 0, 0f);
				blockRight.transform.localPosition += new Vector3 (-1, 0, 0f);
				blockCentre.transform.localPosition += new Vector3 (-1, 0, 0f);
				moveLeftTouch = false;
			}
		}
		if (blockLeft.transform.localPosition.x < 9 && blockRight.transform.localPosition.x < 9) {
			if (Input.GetKeyDown (KeyCode.RightArrow) || moveRightTouch == true) {
				blockLeft.transform.localPosition += new Vector3 (1, 0, 0f);
				blockRight.transform.localPosition += new Vector3 (1, 0, 0f);
				blockCentre.transform.localPosition += new Vector3 (1, 0, 0f);
				moveRightTouch = false;
			}
		}
	}

	void RotateBundle () {
		
		if (bundlePosition == 0) {
			if (Input.GetKeyDown (KeyCode.DownArrow) || (turnLeftTouch == true && turnRightTouch != true)) {
				soundManager.BlockTurn ();
				blockLeft.transform.localPosition += new Vector3 (1, -1, 0f);
				blockRight.transform.localPosition += new Vector3 (-1, 1, 0f);
			}
			if (Input.GetKeyDown (KeyCode.UpArrow) || (turnRightTouch == true && turnLeftTouch != true)) {
				soundManager.BlockTurn ();
				blockLeft.transform.localPosition += new Vector3 (1, 1, 0f);
				blockRight.transform.localPosition += new Vector3 (-1, -1, 0f);
			}
		}
		if (bundlePosition == 1 && blockCentre.transform.localPosition.x != 0 && blockCentre.transform.localPosition.x != 9) {
			if (Input.GetKeyDown (KeyCode.DownArrow) || (turnLeftTouch == true && turnRightTouch != true)) {
				soundManager.BlockTurn ();
				blockLeft.transform.localPosition += new Vector3 (1, 1, 0f);
				blockRight.transform.localPosition += new Vector3 (-1, -1, 0f);
			}
			if (Input.GetKeyDown (KeyCode.UpArrow) || (turnRightTouch == true && turnLeftTouch != true)) {
				soundManager.BlockTurn ();
				blockLeft.transform.localPosition += new Vector3 (-1, 1, 0f);
				blockRight.transform.localPosition += new Vector3 (1, -1, 0f);
			}
		}
		if (bundlePosition == 2) {
			if (Input.GetKeyDown (KeyCode.DownArrow) || (turnLeftTouch == true && turnRightTouch != true)) {
				soundManager.BlockTurn ();
				blockLeft.transform.localPosition += new Vector3 (-1, 1, 0f);
				blockRight.transform.localPosition += new Vector3 (1, -1, 0f);
			}
			if (Input.GetKeyDown (KeyCode.UpArrow) || (turnRightTouch == true && turnLeftTouch != true)) {
				soundManager.BlockTurn ();
				blockLeft.transform.localPosition += new Vector3 (-1, -1, 0f);
				blockRight.transform.localPosition += new Vector3 (1, 1, 0f);
			}
		}
		if (bundlePosition == 3 && blockCentre.transform.localPosition.x != 0 && blockCentre.transform.localPosition.x != 9) {
			if (Input.GetKeyDown (KeyCode.DownArrow) || (turnLeftTouch == true && turnRightTouch != true)) {
				soundManager.BlockTurn ();
				blockLeft.transform.localPosition += new Vector3 (-1, -1, 0f);
				blockRight.transform.localPosition += new Vector3 (1, 1, 0f);
			}
			if (Input.GetKeyDown (KeyCode.UpArrow) || (turnRightTouch == true && turnLeftTouch != true)) {
				soundManager.BlockTurn ();
				blockLeft.transform.localPosition += new Vector3 (1, -1, 0f);
				blockRight.transform.localPosition += new Vector3 (-1, 1, 0f);
			}
		}
		if ((Input.GetKeyDown (KeyCode.DownArrow) || turnLeftTouch == true) && blockCentre.transform.localPosition.x != 0 && blockCentre.transform.localPosition.x != 9) { AnticlockwiseBundle();}
		if ((Input.GetKeyDown (KeyCode.UpArrow) || turnRightTouch == true) && blockCentre.transform.localPosition.x != 0 && blockCentre.transform.localPosition.x != 9) { ClockwiseBundle();}
		turnLeftTouch = false;
		turnRightTouch = false;
	}

	void AnticlockwiseBundle () {

		if (bundlePosition < 3) { newBundlePosition++; ;}

		if (bundlePosition == 3) { newBundlePosition = 0; }

		bundlePosition = newBundlePosition;
	}

	void ClockwiseBundle () {

		if (bundlePosition > 0) { newBundlePosition--; }

		if (bundlePosition == 0) { newBundlePosition = 3; }

		bundlePosition = newBundlePosition;
	}

	void GameOverCheck () {
	
		int checkX;

		for (checkX = 0; checkX < 10; checkX++) {
		
			if (grid.grid [checkX, 13] != 0) {
			
				gameOver.isGameOver = true;
			}
		}
	}

	public void TouchMoveLeft () {

		moveLeftTouch = true;
	}

	public void TouchMoveRight () {

		moveRightTouch = true;
	}

	public void TouchTurnLeft () {

		turnLeftTouch = true;
	}

	public void TouchTurnRight () {

		turnRightTouch = true;
	}

	public void TouchDrop () {

		dropTouch = true;
	}
}