using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleGenerator : MonoBehaviour {

	public GameObject[] bubblePool;

	public IEnumerator GenerateBubbles (int count, Vector2 bubblePosition, Vector2 bubbleVelocity) {
		for (int i = 0; i < count; i++) {
			GameObject newBubble = (GameObject)GameObject.Instantiate (bubblePool [Random.Range (0, bubblePool.Length)], bubblePosition, Quaternion.identity);
			newBubble.GetComponent<Rigidbody2D> ().velocity = bubbleVelocity;
			yield return new WaitForSeconds (Random.Range (0.05f, 0.15f));
		}
		yield return null;
	}

}
