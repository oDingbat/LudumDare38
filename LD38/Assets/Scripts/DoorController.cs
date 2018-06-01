using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorController : MonoBehaviour {

	public DoorController doorExit;

	public bool isLevelDoor;

	void OnTriggerEnter2D (Collider2D coll) {
		if (coll.transform.GetComponent<Rigidbody2D>() != null) {
			if (coll.gameObject.GetComponent<HingeJoint2D>() == null) {
				if (isLevelDoor == false) {
					if (doorExit != null) {
						if (Mathf.Sign (coll.gameObject.GetComponent<Rigidbody2D> ().velocity.x) == Mathf.Sign (transform.localScale.x)) {
							Vector2 startPos = coll.transform.position;
							Vector2 endPos = doorExit.transform.position + (doorExit.transform.up * 0.75f) + (doorExit.transform.right * 0.7f * doorExit.transform.localScale.x);
							Transform[] collChildren = coll.transform.GetComponentsInChildren<Transform> ();
							Debug.Log (collChildren.Length);

							coll.transform.position += (Vector3)(endPos - startPos);
						}
					}
				} else {
					if (coll.gameObject.tag == "Player") {
						SceneManager.LoadScene ("Default");
					}
				}
			}
		}
	}

}
