using UnityEngine;
using System.Collections;

public class TankController : MonoBehaviour {
	public bool activePlayer = false;
	public bool shot = false;
	private float health;
	private GameObject gun;
	public GameObject bullet;

	// Use this for initialization
	void Start () {
		foreach (Transform child in this.gameObject.transform) {
			if (child.name.Equals("Gun"))
			{
				this.gun = child.gameObject;
			}
		}
	}

	public void BeginTurn()
	{
		activePlayer = true;
		shot = false;
	}

	public void Endturn()
	{
		activePlayer = false;
	}

	public bool HasShot()
	{
		return shot;
	}

	public bool IsDead()
	{
		return this.health < 0;
	}

	// Update is called once per frame
	void Update () {
		if (this.activePlayer)
		{
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
				this.Shoot();
			}
		}
	}

	private void Shoot()
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
			if (this.health < 0)
			{
				//TODO: Die
			}
			Destroy (pc.gameObject);
		}
	}
}
