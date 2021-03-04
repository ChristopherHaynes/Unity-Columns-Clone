using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

	private AudioSource source;

	public AudioClip explosion;
	public AudioClip rockDrop;
	public AudioClip blockTurn;

	void Start () {
	
		source = GetComponent<AudioSource> ();
	}

	void Update () {
	
	}

	public void BlockDestruction () {
	
		source.PlayOneShot (explosion, 0.5f);
	}

	public void BlockHit () {
	
		source.PlayOneShot (rockDrop, 1f);
	}

	public void BlockTurn () {
	
		source.PlayOneShot (blockTurn, 1f);
	}
}
