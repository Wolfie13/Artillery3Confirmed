using UnityEngine;
using System.Collections;

public class TankController : MonoBehaviour {
	public bool activePlayer = false;
	public bool shot = false;
	private float health;
	private GameObject gun;
	public GameObject bullet;

	private float gunAngle = 0f;


	// Use this for initialization
	void Start () {
		foreach (Transform child in this.gameObject.transform) {
			if (child.name.Equals("Gun"))
			{
				this.gun = child.gameObject;
			}
		}
		//Clone the material so we can recolour this tank individually.
		Material newMat = Instantiate (this.renderer.material) as Material;
		newMat.SetColor("_Color", RandomColor());
		this.renderer.material = newMat;
	}

	private static Color RandomColor() {
		return new Color (Random.Range (0, 255), Random.Range (0, 255), Random.Range (0, 255));
	}

	public void BeginTurn()
	{
		activePlayer = true;
		shot = false;
	}

	public void Endturn()
	{
		activePlayer = false;
		SelfRight ();
	}

	public bool HasShot()
	{
		return shot;
	}

	public bool IsDead()
	{
		return this.health < 0;
	}

	public void SelfRight()
	{
		if (Vector3.Dot(transform.up,Vector3.up) < 0) {
			this.gameObject.rigidbody.velocity = (Vector3.up * -10);
		}
	}

	// Update is called once per frame
	void Update () {
		if (this.activePlayer)
		{
			if (Input.GetKey ("q"))
			{
				this.gunAngle += Time.deltaTime * 20.0f;
				this.gun.transform.localRotation = Quaternion.AngleAxis(this.gunAngle, Vector3.up);
			}
			else if (Input.GetKey ("e"))
			{
				this.gunAngle -= Time.deltaTime * 20.0f;
				this.gun.transform.localRotation = Quaternion.AngleAxis(this.gunAngle, Vector3.up);
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
		instantiatedProjectile.rigidbody.velocity = dir * 40.0f;
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
