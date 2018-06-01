using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour {

	public AudioController audioController;

	public float swayCurrent;
	public float timeCreated;
	public float lifeSpan;

	public Sprite popSprite;
	public bool popped = false;

	void Start () {
		lifeSpan = Random.Range (2f, 5f);
		timeCreated = Time.timeSinceLevelLoad;
		StartCoroutine (BubbleSway ());
		audioController = GameObject.Find ("Main Camera").GetComponent<AudioController> ();
	}

	IEnumerator BubbleSway () {
		while (true) {
			yield return new WaitForSeconds (0.4f);
			swayCurrent = 3;
			yield return new WaitForSeconds (0.4f);
			swayCurrent = 0;
			yield return new WaitForSeconds (0.4f);
			swayCurrent = -3;
			yield return new WaitForSeconds (0.4f);
			swayCurrent = 0;
		}
	}

	void Update () {
		if (popped == false) {
			GetComponent<Rigidbody2D> ().velocity = Vector2.Lerp (GetComponent<Rigidbody2D> ().velocity, new Vector2 (swayCurrent, 4), 7 * Time.deltaTime);
		}
		if (Time.timeSinceLevelLoad - timeCreated > lifeSpan || transform.position.y > 5) {
			StartCoroutine(Pop ());
		}
	}

	void OnCollisionEnter2D (Collision2D coll) {
		if (coll.transform.tag != "Bubble" && coll.gameObject.layer != LayerMask.NameToLayer("Plant") && coll.gameObject.layer != LayerMask.NameToLayer("Player Tail") && coll.gameObject.layer != LayerMask.NameToLayer("Rocks")  && coll.gameObject.layer != LayerMask.NameToLayer("Buildings")  && coll.gameObject.layer != LayerMask.NameToLayer("Tank"))  {
			StartCoroutine(Pop ());
		}
	}

	IEnumerator Pop () {
		if (popped == false) {
			popped = true;
			GetComponent<SpriteRenderer> ().sprite = popSprite;
			GetComponent<CircleCollider2D> ().isTrigger = true;
			audioController.PlayBubblePop ();
			yield return new WaitForSeconds (0.05f);
			Destroy (gameObject);
		}
	}

}
