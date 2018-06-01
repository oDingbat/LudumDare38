using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WeaponsManager : MonoBehaviour {

	public List<GameObject> allWeapons;
	public GameObject player;
	public GameObject pressE;

	void Start () {
		allWeapons = GameObject.FindGameObjectsWithTag ("Weapon").ToList();
	}

	void Update () {
		if (allWeapons.Count > 0) {
			float closestDistance = Mathf.Infinity;
			GameObject closestWeapon = null;
			for (int i = 0; i < allWeapons.Count; i++) {
				if (Vector2.Distance (player.transform.position, allWeapons [i].transform.position) < closestDistance) {
					closestWeapon = allWeapons [i];
					closestDistance = Vector2.Distance (player.transform.position, allWeapons [i].transform.position);
				}
			}
			if (closestWeapon != null && Vector2.Distance (player.transform.position, closestWeapon.transform.position) < 1.5f) {
				pressE.transform.position = (Vector2)closestWeapon.transform.position + new Vector2 (0, 1f);
				pressE.GetComponent<SpriteRenderer> ().enabled = true;
			} else {
				pressE.GetComponent<SpriteRenderer> ().enabled = false;
			}
		} else {
			pressE.GetComponent<SpriteRenderer> ().enabled = false;
		}
	}

	public void AddWeapon (GameObject newWeapon) {
		allWeapons.Add (newWeapon);
	}

	public Weapon GetClosestWeapon () {
		if (allWeapons.Count > 0) {
			float closestDistance = Mathf.Infinity;
			GameObject closestWeapon = null;
			for (int i = 0; i < allWeapons.Count; i++) {
				if (Vector2.Distance (player.transform.position, allWeapons [i].transform.position) < closestDistance) {
					closestWeapon = allWeapons [i];
					closestDistance = Vector2.Distance (player.transform.position, allWeapons [i].transform.position);
				}
			}
			if (closestWeapon != null && Vector2.Distance (player.transform.position, closestWeapon.transform.position) < 1.5f) {
				allWeapons.Remove (closestWeapon);
				Weapon weaponCopy = new Weapon (closestWeapon.GetComponent<WeaponDrop> ().weapon);
				GameObject.Destroy (closestWeapon);

				return new Weapon(weaponCopy);
			} else {
				return null;
			}
		} else {
			return null;
		}
	}
}
