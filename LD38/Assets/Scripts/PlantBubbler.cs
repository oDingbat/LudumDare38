using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantBubbler : MonoBehaviour {

	public BubbleGenerator bubbleGenerator;
	public Transform[] plantParts;

	void Start () {
		bubbleGenerator = GameObject.FindGameObjectWithTag ("BubbleGenerator").GetComponent<BubbleGenerator>();
		plantParts = GetComponentsInChildren<Transform> ();
		StartCoroutine (PlantBubbleTimer());
	}

	IEnumerator PlantBubbleTimer () {
		while (true) {
			yield return new WaitForSeconds (Random.Range (3f, 6f));
			StartCoroutine(bubbleGenerator.GenerateBubbles(Random.Range(1, 3), plantParts[Random.Range(0, plantParts.Length - 1)].transform.position, Vector2.zero)); 
		}
	}

}
