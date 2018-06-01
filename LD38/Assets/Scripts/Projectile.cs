using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	public BubbleGenerator bubbleGenerator;
	public WeaponsManager weaponsManager;

	public ProjectileType projectileType;
	public enum ProjectileType { Weapon, Bullet }
	public GameObject weaponDrop;

	public float gravity;
	public bool selfDestructed;

	float timeCreated;
	float lifespan = 10;

	void Start () {
		timeCreated = Time.timeSinceLevelLoad;
		bubbleGenerator = GameObject.FindGameObjectWithTag ("BubbleGenerator").GetComponent<BubbleGenerator>();
		weaponsManager = GameObject.FindGameObjectWithTag ("WeaponsManager").GetComponent<WeaponsManager> ();
		StartCoroutine (BubblePath ());
	}

	IEnumerator BubblePath() {
		while (true) {
			bubbleGenerator.GenerateBubbles ((int)Random.Range (1f, 3f), transform.position + (transform.right * -1), GetComponent<Rigidbody2D>().velocity * -10f);
			yield return new WaitForSeconds (10 / GetComponent<Rigidbody2D>().velocity.magnitude);
		}
	}

	void Update () {
		if (transform.position.y > 5) {
			GetComponent<Rigidbody2D> ().velocity += new Vector2 (0, -10 * Time.deltaTime);
		}
		if (GetComponent<Rigidbody2D> ().velocity.magnitude < 1) {
			if (projectileType == ProjectileType.Bullet) {
				Destroy (gameObject);
			}
		}

		transform.rotation = Quaternion.Euler (0, 0, Mathf.Atan2(GetComponent<Rigidbody2D> ().velocity.y, GetComponent<Rigidbody2D> ().velocity.x) * Mathf.Rad2Deg);
	}

	void OnCollisionEnter2D (Collision2D coll) {
		if (selfDestructed == false) {
			if (coll.transform.tag != "Player" && coll.gameObject.layer != LayerMask.NameToLayer ("Bubbles")) {
				SelfDestruct (coll);
			}
		}
	}

	void SelfDestruct (Collision2D coll) {
		selfDestructed = true;
		if (projectileType == ProjectileType.Bullet) {

		} else {
			if (coll.transform.gameObject.GetComponent<Goldfish> ()) {
				coll.transform.gameObject.GetComponent<Goldfish> ().Die ();
			}
			GameObject newWeaponDrop = (GameObject)Instantiate (weaponDrop, transform.position, transform.rotation);
			Destroy(newWeaponDrop.transform.GetComponent<Rigidbody2D>());
			newWeaponDrop.transform.parent = coll.transform;
			weaponsManager.AddWeapon (newWeaponDrop);
			Destroy (gameObject);
		}
	}

}
