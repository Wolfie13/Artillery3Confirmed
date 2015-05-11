using UnityEngine;
using System.Collections;

public class TankController : MonoBehaviour {
	public bool activePlayer;
	public bool shot = false;
	private float health;
	private GameObject gun;
	public GameObject bullet;

	// Use this for initialization
	void Start () {
		activePlayer = true;
		foreach (Transform child in this.gameObject.transform) {
			if (child.name.Equals("Gun"))
			{
				this.gun = child.gameObject;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (this.activePlayer) {
			if (Input.GetKey ("w"))
			{
				this.gun.transform.Rotate(Vector3.up, Time.deltaTime * 20.0f);
			}
			else if (Input.GetKey ("s"))
			{
				this.gun.transform.Rotate(Vector3.up, Time.deltaTime * -20.0f);
			}
			
			if (Input.GetKey ("a"))
			{
				transform.position += transform.right * Time.deltaTime * 3;
			}			
			else if (Input.GetKey ("d"))
			{
				transform.position += transform.right * Time.deltaTime * -3;
			}

			if (Input.GetKey("space") && !shot)
			{
				this.shot = true;
				this.shoot();
			}
		}
	}

	private void shoot()
	{
		Vector3 dir = gun.transform.forward;
		Vector3 pos = gun.transform.position + (gun.transform.forward * 3.0f);
		GameObject instantiatedProjectile = Instantiate(bullet, pos, UnityEngine.Quaternion.Euler(dir)) as GameObject;
		instantiatedProjectile.rigidbody.velocity = dir * 10.0f;
	}

	void OnCollisionEnter (Collision col)
	{
		if (col.gameObject.CompareTag ("Projectile")) {
			ProjectileController pc = col.gameObject.GetComponent<ProjectileController>();
			this.health -= pc.damage;
			Destroy (pc.gameObject);
		}
	}
}
