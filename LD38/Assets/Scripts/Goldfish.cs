using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goldfish : MonoBehaviour {

	public LayerMask visionMask;

	public Sprite headIdle;
	public Sprite headAttacking;
	public Sprite headDead;

	public GameObject player;
	public Vector2 playerLastSeen;

	public float agroTime;		// Amount of time the fish has seen the player

	public bool isDead = false;

	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
	}

	void Update () {
		if (transform.position.y > 5) {
			GetComponent<Rigidbody2D> ().gravityScale = 2;
		} else {
			GetComponent<Rigidbody2D> ().gravityScale = 0;
		}


		if (isDead == false) {
			RaycastHit2D hit = Physics2D.Raycast (transform.position, player.transform.position - transform.position, Mathf.Infinity, visionMask);
			if (hit) {
				if (hit.transform.gameObject.tag == "Player") {
					agroTime = Mathf.Clamp (agroTime + Time.deltaTime, 0, 3);
					playerLastSeen = hit.transform.position;
				} else {
					agroTime = Mathf.Clamp (agroTime - Time.deltaTime, 0, 3);
				}
			} else {
				agroTime = Mathf.Clamp (agroTime - Time.deltaTime, 0, 3);
			}
		}
	}

	public void Die () {

	}


}
