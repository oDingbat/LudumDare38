using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {


	public BubbleGenerator bubbleGenerator;
	public WeaponsManager weaponsManager;

	public Camera gameCamera;

	public Transform cursor;

	public Vector2 velocityDesired;
	public bool swimming = false;

	float boostStrength = 5;
	bool boosting = false;

	public Weapon[] weapons;
	public int currentWeapon;

	public GameObject heldWeapon;

	void Start () {
		bubbleGenerator = GameObject.FindGameObjectWithTag ("BubbleGenerator").GetComponent<BubbleGenerator>();
		weaponsManager = GameObject.FindGameObjectWithTag ("WeaponsManager").GetComponent<WeaponsManager> ();
		StartCoroutine (BoostUpdate ());
	}

	void Update () {

		if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.Quit();
		}

		cursor.transform.position = gameCamera.ScreenToWorldPoint (Input.mousePosition / 4) + new Vector3(0, 0, 10);

		if (Input.GetButton ("Boost")) {
			boosting = true;
		} else {
			boosting = false;
		}

		if (Input.GetKeyDown (KeyCode.R)) {
			SceneManager.LoadScene ("Default");
		}
			

		velocityDesired.x = Input.GetAxis ("Horizontal");
		velocityDesired.y = Input.GetAxis ("Vertical");

		velocityDesired.Normalize ();

		velocityDesired *= (boosting ? 2.5f : 1);

		if (transform.position.y > 5) {
			if (swimming == true) {
				GetComponent<Rigidbody2D> ().velocity *= 2;
			} else {
				velocityDesired.y = -1f;
			}
			swimming = false;
		} else {
			swimming = true;
		}

		GetComponent<Rigidbody2D>().velocity = Vector2.Lerp (GetComponent<Rigidbody2D>().velocity, velocityDesired * 7, 7 * Time.deltaTime);



		transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler (0, 0, Mathf.Atan2 (GetComponent<Rigidbody2D> ().velocity.y, GetComponent<Rigidbody2D> ().velocity.x) * Mathf.Rad2Deg) * Quaternion.Euler(0, 0, -90), (boosting ? 30f : 15f) * Time.deltaTime);
	

		UpdateMouseInput ();
		UpdateWeapons ();
	}

	void UpdateMouseInput () {
		Vector2 aimDirection = (cursor.position - transform.position);

		if (Input.GetMouseButton (0)) {
			Cursor.visible = false;

			Cursor.lockState = CursorLockMode.Confined;

			if (weapons [currentWeapon] != null && weapons [currentWeapon].name != "" && weapons [currentWeapon].name != "None") {
				if (weapons [currentWeapon].weaponType == Weapon.WeaponType.Throwable) {
					GameObject newWeaponProjectile = (GameObject)Instantiate (weapons [currentWeapon].projectilePrefab, transform.position + ((Vector3)aimDirection.normalized * 0.5f), Quaternion.Euler (0, 0, Mathf.Atan2 (aimDirection.y, aimDirection.x) * Mathf.Rad2Deg));
					newWeaponProjectile.GetComponent<Rigidbody2D> ().velocity = aimDirection.normalized * weapons [currentWeapon].projectileVelocity;
					newWeaponProjectile.GetComponent<Projectile> ().weaponDrop = Resources.Load ("WeaponDrops/" + weapons [currentWeapon].name) as GameObject;
					weapons [currentWeapon].name = "None";
				} else if (weapons [currentWeapon].weaponType == Weapon.WeaponType.Gun) {
					weapons [currentWeapon].timeLastClicked = Time.timeSinceLevelLoad;
				} else if (weapons [currentWeapon].weaponType == Weapon.WeaponType.Key) {
					DropWeapon ();
				}
			}

		}

		if (weapons[currentWeapon] != null && weapons [currentWeapon].name != "" && weapons [currentWeapon].name != "None") {
			heldWeapon.GetComponent<SpriteRenderer> ().sprite = weapons [currentWeapon].weaponSprite;
			heldWeapon.transform.rotation = Quaternion.Euler (0, 0, Mathf.Atan2 (aimDirection.y, aimDirection.x) * Mathf.Rad2Deg);
		} else {
			heldWeapon.GetComponent<SpriteRenderer> ().sprite = null;
		}
	}

	void UpdateWeapons () {
		if (Input.GetButtonDown ("Pickup")) {
			Weapon desiredWeapon = weaponsManager.GetClosestWeapon ();
			if (desiredWeapon != null) {
				if (weapons [currentWeapon].name != "None" && weapons [currentWeapon].name != "") {
					DropWeapon ();
				}
				weapons [currentWeapon] = desiredWeapon;
			}
		}

		if (Input.GetButtonDown ("Drop")) {
			DropWeapon ();
		}

		AttemptWeaponFire ();
	}

	void AttemptWeaponFire () {
		Vector2 aimDirection = (cursor.position - transform.position);
		if (weapons [currentWeapon].weaponType == Weapon.WeaponType.Gun) {
			if (weapons [currentWeapon].ammoCurrent > 0) {
				if (weapons [currentWeapon].timeLastClicked + 0.05f > Time.timeSinceLevelLoad && weapons [currentWeapon].timeLastFired + (1 / weapons [currentWeapon].firerate) < Time.timeSinceLevelLoad) {
					weapons [currentWeapon].timeLastFired = Time.timeSinceLevelLoad;
					weapons [currentWeapon].ammoCurrent--;
					GameObject newWeaponProjectile = (GameObject)Instantiate (weapons [currentWeapon].projectilePrefab, transform.position + ((Vector3)aimDirection.normalized * 0.5f), Quaternion.Euler (0, 0, Mathf.Atan2 (aimDirection.y, aimDirection.x) * Mathf.Rad2Deg));
					newWeaponProjectile.GetComponent<Rigidbody2D> ().velocity = aimDirection.normalized * weapons [currentWeapon].projectileVelocity;
					newWeaponProjectile.GetComponent<Projectile> ().weaponDrop = Resources.Load ("WeaponDrops/" + weapons [currentWeapon].name) as GameObject;
				}
			}
		}
	}

	void DropWeapon () {
		Vector2 aimDirection = (cursor.position - transform.position).normalized;
		if (weapons [currentWeapon] != null && weapons [currentWeapon].name != "" && weapons [currentWeapon].name != "None") {
			GameObject newWeaponDrop = Instantiate (Resources.Load ("WeaponDrops/" + weapons[currentWeapon].name) as GameObject, transform.position + (Vector3)aimDirection, Quaternion.Euler (0, 0, Random.Range (0, 360)));
			newWeaponDrop.GetComponent<Rigidbody2D> ().velocity = aimDirection * 5;
			newWeaponDrop.GetComponent<WeaponDrop> ().weapon = new Weapon(weapons [currentWeapon]);
			weaponsManager.AddWeapon (newWeaponDrop);
			weapons [currentWeapon].name = "None";
		}
	}

	IEnumerator BoostUpdate () {
		while (true) {
			yield return new WaitForSeconds (0.025f);
			if (boosting == true) {
				StartCoroutine (bubbleGenerator.GenerateBubbles (Random.Range (1, 2), transform.position + (transform.up * -1), (transform.up * -10f))); 
			}
		}
	}

}
