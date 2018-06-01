using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDrop : MonoBehaviour {

	public Weapon weapon;

	void Update () {
		if (transform.position.y > 5) {
			GetComponent<Rigidbody2D> ().velocity += new Vector2 (0, -10 * Time.deltaTime);
		}
	}

}
