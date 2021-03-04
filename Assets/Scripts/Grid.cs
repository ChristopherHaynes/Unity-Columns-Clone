using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;

public class Grid : MonoBehaviour {

	Spawner spawner;
	SoundManager soundManager;
	ScoreManager scoreManager;

	public GameObject particleGenerator;

	public float currentDrop = 0f;

	// Declare the requirements for the grid
	public static int w = 10;
	public static int h = 15;
	public int[,] grid = new int[w, h];
	public int[,] blankGrid = new int [w, h];

	public struct BlockData {

		public int blockX;
		public int blockY;
		public bool placed;

		public BlockData (int blockX, int blockY){

			this.blockX = blockX;
			this.blockY = blockY;
			placed = false;
		}

		public void SetPlaced () {

			placed = true;
		}
	}

	List<BlockData> blockData = new List<BlockData> ();
	List<BlockData> dropList = new List<BlockData> ();

	public bool startDrop = false;
	public bool readyToDrop = false;
	public bool droppedBox = false;

	void Awake () {
	
		spawner  = this.GetComponent<Spawner> ();
		soundManager = this.GetComponent<SoundManager> ();
		scoreManager = this.GetComponent<ScoreManager> ();

		for (int i = 0; i == 9; i++) {
			for (int j = 0; j == 15; j++) {
			
				grid [i, j] = 0;
			}
		}
	}

	void Update () {

		float dropSpeed = 6f;

		if (startDrop == true) {

			foreach(BlockData block in dropList)
			{
				if (currentDrop < 1) {
					spawner.placedBlocks [block.blockX, block.blockY].transform.localPosition += (new Vector3 (0, -dropSpeed, 0f) * Time.deltaTime); 

				}
				if (currentDrop > 1 && block.placed == false){ 
					spawner.placedBlocks [block.blockX, block.blockY].transform.position = new Vector3 (block.blockX, block.blockY - 1.01f, 0f);
					spawner.placedBlocks [block.blockX, block.blockY - 1] = spawner.placedBlocks [block.blockX, block.blockY];
					spawner.placedBlocks [block.blockX, block.blockY] = null;
					grid [block.blockX, block.blockY - 1] = grid [block.blockX, block.blockY];
					grid [block.blockX, block.blockY] = 0;

					if (spawner.placedBlocks [block.blockX, block.blockY - 1] != null || block.blockY < 2) {
					
						soundManager.BlockHit ();
					}

					block.SetPlaced ();
				}
			}

			if (currentDrop > 1) {

				dropList.Clear ();
				currentDrop = 0f - (dropSpeed * Time.deltaTime);
				startDrop = false;
				DropBlocks ();
			}

			currentDrop += (dropSpeed * Time.deltaTime);
		}

		if (readyToDrop == true && startDrop == false && droppedBox == false) {
		
			scoreManager.CalculateScore ();
			spawner.NextToPlay ();
			readyToDrop = false;

		}

		if (readyToDrop == true && startDrop == false && droppedBox == true) {

			readyToDrop = false;
			droppedBox = false;
			MatchCheck ();
		}
	}

	public void MatchCheck () {
	
		HorizontalCheck ();
		VerticalCheck ();
		DiagonalUpRightCheck ();
		DiagonalUpLeftCheck ();
		DestroyMatches ();
		DropBlocks ();
				
		readyToDrop = true;
	}

	void HorizontalCheck () {

		int colourCheck = 1;
		int checkX = 0, checkY = 1;
		int matchedBlocks = 0;

		for (colourCheck = 1; colourCheck < spawner.totalBlocksInPlay + 1; colourCheck++) {
		
			matchedBlocks = 0;

			for (checkY = 1; checkY < 13; checkY++) {

				matchedBlocks = 0;

				for (checkX = 0; checkX < 10; checkX++) {
				
					if (grid [checkX, checkY] == colourCheck) {
					
						matchedBlocks++;
						if (matchedBlocks == 3) { blockData.Add( new BlockData(checkX - 2, checkY));  blockData.Add( new BlockData(checkX - 1, checkY));  blockData.Add( new BlockData(checkX, checkY)); scoreManager.upDown3Matches++; scoreManager.numberOfMatches++; }
						if (matchedBlocks == 4) { blockData.Add( new BlockData(checkX, checkY)); scoreManager.upDown3Matches--; scoreManager.upDown4Matches++;}
						if (matchedBlocks == 5) { blockData.Add( new BlockData(checkX, checkY)); scoreManager.upDown4Matches--; scoreManager.upDown5Matches++;}
					} 
					else { matchedBlocks = 0; }
				}
			}
		}
	}

	void VerticalCheck () {

		int colourCheck = 1;
		int checkX = 0, checkY = 1;
		int matchedBlocks = 0;

		for (colourCheck = 1; colourCheck < spawner.totalBlocksInPlay + 1; colourCheck++) {

			matchedBlocks = 0;

			for (checkX = 0; checkX < 10; checkX++) {
			
				matchedBlocks = 0;

				for (checkY = 1; checkY < 13; checkY++) {

					if (grid [checkX, checkY] == colourCheck) {

						matchedBlocks++;
						if (matchedBlocks == 3) { blockData.Add( new BlockData(checkX, checkY - 2));  blockData.Add( new BlockData(checkX, checkY - 1));  blockData.Add( new BlockData(checkX, checkY)); scoreManager.upDown3Matches++; scoreManager.numberOfMatches++;}
						if (matchedBlocks == 4) { blockData.Add( new BlockData(checkX, checkY)); scoreManager.upDown3Matches--; scoreManager.upDown4Matches++; }
						if (matchedBlocks == 5) { blockData.Add( new BlockData(checkX, checkY)); scoreManager.upDown4Matches--; scoreManager.upDown5Matches++;}
					} 
					else { matchedBlocks = 0; }
				}
			}
		}
	}

	void DiagonalUpRightCheck () {

		int colourCheck = 1;
		int checkX = 0, checkY = 1;
		int matchedBlocks = 0;

		for (colourCheck = 1; colourCheck < spawner.totalBlocksInPlay + 1; colourCheck++) {

			matchedBlocks = 0;

			for (checkY = 1; checkY < 13; checkY++) {

				int newCheckY; checkX = 0; matchedBlocks = 0;

				for (newCheckY = checkY; newCheckY < 13 && checkX < 10; newCheckY++) {
				
					if (grid [checkX, newCheckY] == colourCheck) {

						matchedBlocks++;
						if (matchedBlocks == 3) { blockData.Add( new BlockData(checkX - 2, newCheckY - 2));  blockData.Add( new BlockData(checkX - 1, newCheckY - 1));  blockData.Add( new BlockData(checkX, newCheckY)); scoreManager.diagonal3Matches++; scoreManager.numberOfMatches++;}
						if (matchedBlocks == 4) { blockData.Add( new BlockData(checkX, newCheckY)); scoreManager.diagonal3Matches--; scoreManager.diagonal4Matches++; }
						if (matchedBlocks == 5) { blockData.Add( new BlockData(checkX, newCheckY)); scoreManager.diagonal4Matches--; scoreManager.diagonal5Matches++;  }
					} 
					else { matchedBlocks = 0; }

					checkX++;
				}
			}

			for (checkX = 1; checkX < 10; checkX++) {

				int newCheckX; checkY = 1; matchedBlocks = 0;

				for (newCheckX = checkX; newCheckX < 10; newCheckX++) {

					if (grid [newCheckX, checkY] == colourCheck) {

						matchedBlocks++;
						if (matchedBlocks == 3) { blockData.Add( new BlockData(newCheckX - 2, checkY - 2));  blockData.Add( new BlockData(newCheckX - 1, checkY - 1));  blockData.Add( new BlockData(newCheckX, checkY)); scoreManager.diagonal3Matches++; scoreManager.numberOfMatches++; }
						if (matchedBlocks == 4) { blockData.Add( new BlockData(newCheckX, checkY)); scoreManager.diagonal3Matches--; scoreManager.diagonal4Matches++;  }
						if (matchedBlocks == 5) { blockData.Add( new BlockData(newCheckX, checkY)); scoreManager.diagonal4Matches--; scoreManager.diagonal5Matches++;  }
					} 
					else { matchedBlocks = 0; }

					checkY++;
				}
			}
		}
	}

	void DiagonalUpLeftCheck () {

		int colourCheck = 1;
		int checkX = 0, checkY = 1;
		int matchedBlocks = 0;

		for (colourCheck = 1; colourCheck < spawner.totalBlocksInPlay + 1; colourCheck++) {

			matchedBlocks = 0;

			for (checkY = 1; checkY < 13; checkY++) {

				int newCheckY; checkX = 9; matchedBlocks = 0;

				for (newCheckY = checkY; newCheckY < 13 && checkX > -1; newCheckY++) {

					if (grid [checkX, newCheckY] == colourCheck) {

						matchedBlocks++;
						if (matchedBlocks == 3) { blockData.Add( new BlockData(checkX + 2, newCheckY - 2));  blockData.Add( new BlockData(checkX + 1, newCheckY - 1));  blockData.Add( new BlockData(checkX, newCheckY));  scoreManager.diagonal3Matches++; scoreManager.numberOfMatches++;}
						if (matchedBlocks == 4) { blockData.Add( new BlockData(checkX, newCheckY)); scoreManager.diagonal3Matches--; scoreManager.diagonal4Matches++; }
						if (matchedBlocks == 5) { blockData.Add( new BlockData(checkX, newCheckY)); scoreManager.diagonal4Matches--; scoreManager.diagonal5Matches++; }
					} 
					else { matchedBlocks = 0; }

					checkX--;
				}
			}

			for (checkX = 9; checkX > 0; checkX--) {

				int newCheckX; checkY = 1; matchedBlocks = 0;

				for (newCheckX = checkX; newCheckX > -1; newCheckX--) {

					if (grid [newCheckX, checkY] == colourCheck) {

						matchedBlocks++;
						if (matchedBlocks == 3) { blockData.Add( new BlockData(newCheckX + 2, checkY - 2));  blockData.Add( new BlockData(newCheckX + 1, checkY - 1));  blockData.Add( new BlockData(newCheckX, checkY)); scoreManager.diagonal3Matches++; scoreManager.numberOfMatches++; }
						if (matchedBlocks == 4) { blockData.Add( new BlockData(newCheckX, checkY)); scoreManager.diagonal3Matches--; scoreManager.diagonal4Matches++; }
						if (matchedBlocks == 5) { blockData.Add( new BlockData(newCheckX, checkY)); scoreManager.diagonal4Matches--; scoreManager.diagonal5Matches++; }
					} 
					else { matchedBlocks = 0; }

					checkY++;
				}
			}
		}
	}

	void DestroyMatches () {
	
		var noDupesBlockData = new HashSet<BlockData>(blockData);

		foreach(BlockData block in noDupesBlockData)
		{
			if (spawner.placedBlocks [block.blockX, block.blockY] != null && grid [block.blockX, block.blockY] != 0) {

				StartCoroutine (ParticleHandler (block.blockX, block.blockY, grid[block.blockX, block.blockY]));

				soundManager.BlockDestruction ();
				Destroy (spawner.placedBlocks [block.blockX, block.blockY]);
				grid [block.blockX, block.blockY] = 0;

			} else {
			
				Debug.Log ("WARNING! GRID/BLOCK MISMATCH");
			}

		}
		if (blockData.Count > 0) {

			blockData.Clear ();
		}
	}

	IEnumerator ParticleHandler (int blockX, int blockY, int tag) {

		ParticleSystem particleSys;
		GameObject activeParticleGenerator;
		Color particleColor;

		activeParticleGenerator = (GameObject)Instantiate (particleGenerator, new Vector3 (blockX, blockY, 0f), Quaternion.identity); 
		particleSys = activeParticleGenerator.GetComponent<ParticleSystem> ();
		ParticleSystem.EmissionModule emitter = particleSys.emission;

		if (tag == 1) { particleColor = new Color32 (91, 110, 225, 255); particleSys.startColor = particleColor; }
		if (tag == 2) { particleColor = new Color32 (217, 160, 102, 255); particleSys.startColor = particleColor; }
		if (tag == 3) { particleColor = new Color32 (55, 148, 110, 255); particleSys.startColor = particleColor;; }
		if (tag == 4) { particleColor = new Color32 (223, 113, 38, 255); particleSys.startColor = particleColor; }
		if (tag == 5) { particleColor = new Color32 (118, 66, 138, 255); particleSys.startColor = particleColor; }
		if (tag == 6) { particleColor = new Color32 (172, 50, 50, 255); particleSys.startColor = particleColor; }
		if (tag == 7) { particleColor = new Color32 (132, 126, 135, 255); particleSys.startColor = particleColor; }
		if (tag == 8) { particleColor = new Color32 (185, 78, 99, 255); particleSys.startColor = particleColor; }
		if (tag == 9) { particleColor = new Color32 (140, 92, 70, 255); particleSys.startColor = particleColor; }

		emitter.enabled = true;
		particleSys.Stop ();
		particleSys.Play ();

		yield return new WaitForSeconds (4);

		Destroy (activeParticleGenerator);
	}
		
	void DropBlocks () {

		int checkX = 0, checkY = 1;
		bool foundSpace = false;

		for (checkX = 0; checkX < 10; checkX++) {
			foundSpace = false;

			for (checkY = 1; checkY < 13; checkY++) {

				if (grid [checkX, checkY] != 0 && foundSpace == true) {
				
					dropList.Add (new BlockData (checkX, checkY)); droppedBox = true;
				}

				if (grid [checkX, checkY] == 0) {

					foundSpace = true;
				}
			}
		}

		if (dropList.Count != 0) { startDrop = true; }
	}

	public void Save (bool alsoQuit) {

		if (spawner.saveAvailable == true) {

			Canvas canvas = FindObjectOfType<Canvas>() ;
			canvas.GetComponent<Animator> ().SetTrigger ("Save");
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/saveState.dat", FileMode.OpenOrCreate);

			SaveStateData data = new SaveStateData ();
			data.grid = grid;
			data.score = scoreManager.totalScore;

            data.matches3 = scoreManager.tot3Match;
            data.matches4 = scoreManager.tot4Match;
            data.matches5 = scoreManager.tot5Match;
            data.matchesHV = scoreManager.totHVMatch;
            data.matchesD = scoreManager.totDMatch;

            data.leftBlockTag = int.Parse (spawner.blockLeft.tag);
			data.rightBlockTag = int.Parse (spawner.blockRight.tag);
			data.centreBlockTag = int.Parse (spawner.blockCentre.tag);

			data.leftNextTag = int.Parse (spawner.nextBlockLeft.tag);
			data.rightNextTag = int.Parse (spawner.nextBlockRight.tag);
			data.centreNextTag = int.Parse (spawner.nextBlockCentre.tag);

			bf.Serialize (file, data);
			file.Close ();

			if (alsoQuit == true) {

                SceneManager.LoadScene("MainMenu");
            }
		}
	}

	public void SaveClear () {

		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Open (Application.persistentDataPath + "/saveState.dat", FileMode.OpenOrCreate);

		SaveStateData data = new SaveStateData ();

		data.grid = blankGrid;
		data.score = 0;

        data.matches3 = 0;
        data.matches4 = 0;
        data.matches5 = 0;
        data.matchesHV = 0;
        data.matchesD = 0;

        data.leftBlockTag = Random.Range (1, 7);
		data.rightBlockTag = Random.Range (1, 7);
		data.centreBlockTag = Random.Range (1, 7);

		data.leftNextTag = Random.Range (1, 7);
		data.rightNextTag = Random.Range (1, 7);
		data.centreNextTag = Random.Range (1, 7);

		bf.Serialize (file, data);
		file.Close ();
	}

	public void Load () {
	
		if (File.Exists (Application.persistentDataPath + "/saveState.dat")) {

			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/saveState.dat", FileMode.Open);
			SaveStateData data = (SaveStateData)bf.Deserialize (file);
			file.Close ();

			if (grid != null) { grid = data.grid; }
			scoreManager.totalScore = data.score;
			scoreManager.SetScoreOnLoad ();

			for (int x = 0; x < 10; x++) {
				for (int y = 0; y < 12; y++){
					if (grid [x, y] != 0) {
					
						spawner.placedBlocks[x,y] = (GameObject)Instantiate (spawner.blocks [grid [x, y] - 1], new Vector3 (x, y, 0f), Quaternion.identity);
					}
				}
			}

            scoreManager.tot3Match = data.matches3;
            scoreManager.tot4Match = data.matches4;
            scoreManager.tot5Match = data.matches5;
            scoreManager.totHVMatch = data.matchesHV;
            scoreManager.totDMatch = data.matchesD;

            spawner.blockLeft = (GameObject)Instantiate (spawner.blocks [data.leftBlockTag - 1], new Vector3 (4, 14, 0f), Quaternion.identity);
			spawner.blockRight = (GameObject)Instantiate (spawner.blocks [data.rightBlockTag - 1], new Vector3 (6, 14, 0f), Quaternion.identity);
			spawner.blockCentre = (GameObject)Instantiate (spawner.blocks [data.centreBlockTag - 1], new Vector3 (5, 14, 0f), Quaternion.identity);

			spawner.nextBlockLeft = (GameObject)Instantiate (spawner.blocks [data.leftNextTag - 1], new Vector3 (11.5f, 12, 0f), Quaternion.identity);
			spawner.nextBlockRight = (GameObject)Instantiate (spawner.blocks [data.rightNextTag - 1], new Vector3 (13.5f, 12, 0f), Quaternion.identity);
			spawner.nextBlockCentre = (GameObject)Instantiate (spawner.blocks [data.centreNextTag - 1], new Vector3 (12.5f, 12, 0f), Quaternion.identity);
		}
	}

	[System.Serializable]
	class SaveStateData {

		public int[,] grid;
		public int score;

        public int matches3;
        public int matches4;
        public int matches5;
        public int matchesHV;
        public int matchesD;

        public int leftBlockTag;
		public int rightBlockTag;
		public int centreBlockTag;

		public int leftNextTag;
		public int rightNextTag;
		public int centreNextTag;
	}
}

