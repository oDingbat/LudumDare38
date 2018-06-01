using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LevelManager : MonoBehaviour {

	public GameObject[] tanksSmall;
	public GameObject[] tanksMedium;
	public GameObject[] tanksLarge;

	public GameObject[] plants;
	public GameObject[] castlesSmall;
	public GameObject[] castlesMedium;
	public GameObject[] castlesLarge;

	public int levelCounter = 1;

	public GameObject currentTank;

	public GameObject gatePrefab;

	public GameObject keyPrefab;
	public GameObject tridentPrefab;

	void Start () {
		NextLevel ();
	}

	void NextLevel () {
		if (currentTank != null) {
			Destroy (currentTank);
		}

		GameObject[] tankPool = (levelCounter < 3 ? tanksSmall : (levelCounter < 6 ? tanksMedium : tanksLarge));
		GameObject[] castlePool = (levelCounter < 3 ? castlesSmall : (levelCounter < 6 ? castlesMedium : castlesLarge));

		GameObject newTank = (GameObject)Instantiate (tankPool [Random.Range (0, tankPool.Length)], Vector2.zero, Quaternion.identity);
		currentTank = newTank;
		List<Transform> tankSpawnOptions = new List<Transform> ();
		foreach (Transform t in newTank.gameObject.GetComponentsInChildren<Transform> ().ToList()) {
			if (t.gameObject.tag == "SpawnOption") {
				tankSpawnOptions.Add (t);
			}
		}

		Transform chosenSpawnOption = tankSpawnOptions [Random.Range (0, tankSpawnOptions.Count)];

		List<Transform> castleSpawns = new List<Transform> ();
		List<Transform> vegetationSpawns = new List<Transform> ();
		List<Transform> keySpawns = new List<Transform> ();
		GameObject randomVegetation = plants [Random.Range (0, plants.Length)];

		foreach (Transform c in chosenSpawnOption.gameObject.GetComponentsInChildren<Transform> ().ToList()) {
			if (c.gameObject.tag == "CastleSpawn") {
				castleSpawns.Add (c);
			} else if (c.gameObject.tag == "VegetationSpawn") {
				vegetationSpawns.Add (c);
			} else if (c.gameObject.tag == "KeySpawn") {
				keySpawns.Add (c);
			}
		}

		foreach (Transform castle in castleSpawns) {
			GameObject newCastle = (GameObject)Instantiate(castlePool [Random.Range (0, castlePool.Length)], castle.position, castle.rotation * Quaternion.Euler(0, 0, Random.Range(-5f, 5f)));
			newCastle.transform.parent = currentTank.transform;
		}

		foreach (Transform vegetation in vegetationSpawns) {
			GameObject newVegetation = (GameObject)Instantiate(randomVegetation, vegetation.position, vegetation.rotation * Quaternion.Euler(0, 0, Random.Range(-5f, 5f)));
			newVegetation.transform.parent = currentTank.transform;
		}

		Transform randomKeySpawn = keySpawns [Random.Range (0, keySpawns.Count)];
		GameObject newKey = (GameObject)Instantiate (keyPrefab, randomKeySpawn.position, Quaternion.Euler (0, 0, Random.Range (0, 360)));

		if (Random.Range (0, 100) > 75) {
			Transform randomTridentSpawn = keySpawns [Random.Range (0, keySpawns.Count)];
			GameObject randomTrident = (GameObject)Instantiate (tridentPrefab, randomTridentSpawn.position, Quaternion.Euler (0, 0, Random.Range (0, 360)));
		}

		List<GameObject> allDoors = GameObject.FindGameObjectsWithTag("Door").ToList();

		GameObject randomDoor = allDoors [Random.Range (0, allDoors.Count)];
		allDoors.Remove (randomDoor);
		randomDoor.GetComponent<DoorController> ().isLevelDoor = true;

		GameObject newGate = (GameObject)Instantiate (gatePrefab, randomDoor.transform.position, randomDoor.transform.rotation);
		newGate.transform.localScale = randomDoor.transform.localScale;

		GameObject startingDoor = allDoors [Random.Range (0, allDoors.Count)];
		GameObject.FindGameObjectWithTag("Player").transform.position = startingDoor.transform.position + (startingDoor.transform.up * 0.75f) + (startingDoor.transform.right * 0.7f * startingDoor.transform.localScale.x);
		GameObject.FindGameObjectWithTag ("Player").GetComponent<Rigidbody2D> ().velocity = startingDoor.transform.right * -10f;

		Debug.Log ("Doors" + allDoors.Count);
		for (int i = allDoors.Count; i > 0; i--) {
			if (allDoors.Count > 0) {
				Debug.Log ("Attempt " + allDoors.Count);
				List<GameObject> randomDoors = GameObject.FindGameObjectsWithTag("Door").ToList();
				randomDoors.Remove (randomDoor);
				randomDoors.Remove(allDoors[0]);
				for (int j = 0; j < randomDoors.Count; j++) {
					int randomIndex = Random.Range (0, randomDoors.Count);
					if (allDoors [0].transform.localScale.x != randomDoors [j].transform.localScale.x) {
						allDoors [0].GetComponent<DoorController> ().doorExit = randomDoors [j].GetComponent<DoorController> ();
						break;
					} else {
						randomDoors.RemoveAt (randomIndex);
					}
				}
				allDoors.RemoveAt (0);

			}
		}
			
		
	}

}
