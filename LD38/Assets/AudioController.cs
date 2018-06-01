using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour {

	AudioSource audioSource;
	public AudioClip bubblePop;

	void Start () {
		audioSource = GetComponent<AudioSource> ();
	}

	public void PlayBubblePop () {
		audioSource.pitch = Random.Range (0.9f, 1.1f);
		audioSource.PlayOneShot (bubblePop);
	}

}
