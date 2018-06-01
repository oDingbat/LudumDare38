using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Weapon {

	public string name;
	public int rarity;

	public WeaponType weaponType;
	public enum WeaponType { Throwable, Melee, Gun, Key }

	public bool automatic;
	public int ammoCurrent;
	public int ammoMax;
	public float accuracy;
	public int damage;
	public float firerate;
	public float burstDelay;
	public int burstCount;
	public float timeLastFired;
	public float timeLastClicked;

	public GameObject projectilePrefab;
	public float projectileVelocity;

	public Sprite weaponSprite;

	public AudioClip soundFireNormal;
	public AudioClip soundFireLow;

	public Weapon (Weapon copyWeapon) {
		name = copyWeapon.name;
		rarity = copyWeapon.rarity;

		weaponType = copyWeapon.weaponType;

		automatic = copyWeapon.automatic;
		ammoCurrent = copyWeapon.ammoCurrent;
		ammoMax = copyWeapon.ammoMax;
		accuracy = copyWeapon.accuracy;
		damage = copyWeapon.damage;
		firerate = copyWeapon.firerate;
		burstDelay = copyWeapon.burstDelay;
		burstCount = copyWeapon.burstCount;
		timeLastFired = copyWeapon.timeLastFired;
		timeLastClicked = copyWeapon.timeLastClicked;

		projectilePrefab = copyWeapon.projectilePrefab;
		projectileVelocity = copyWeapon.projectileVelocity;

		weaponSprite = copyWeapon.weaponSprite;

		soundFireNormal = copyWeapon.soundFireNormal;
		soundFireLow = copyWeapon.soundFireLow;
	}

	public Weapon () {
		name = "None";
	}

}
