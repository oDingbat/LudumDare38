using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour {

	void OnCollisionEnter2D (Collision2D coll) {
		if (coll.transform.gameObject.GetComponent<Player> ()) {
			if (coll.transform.gameObject.GetComponent<Player> ().weapons [coll.transform.gameObject.GetComponent<Player> ().currentWeapon].name == "Key") {
				coll.transform.gameObject.GetComponent<Player> ().weapons [coll.transform.gameObject.GetComponent<Player> ().currentWeapon].name = "None";
				Destroy (gameObject);
			}
		} else {
			if (coll.transform.gameObject.GetComponent<WeaponDrop>() != null && coll.transform.gameObject.GetComponent<WeaponDrop>().weapon.name == "Key") {
				Destroy (coll.transform.gameObject);
				Destroy (gameObject);
			}
		}
	}

}
