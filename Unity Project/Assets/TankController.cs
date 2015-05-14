using UnityEngine;
using System.Collections;

public class TankController : MonoBehaviour {
	public bool activePlayer = false;
	public bool shot = false;
	public float health = 100;
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
		return new Color (Random.value, Random.value, Random.value);
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
		float d = Vector3.Dot (transform.up.normalized, Vector3.up);

		if (d > 0.5 || d < -0.5) {
			this.gameObject.rigidbody.velocity = (Vector3.up * 20);
		}
	}
    public void Damage (float damage)
    {
        this.health -= damage;
    }

	// Update is called once per frame
	void Update () {
		if (!this.IsDead()) 
		{
			Vector3 pos = this.transform.position;
			pos.z = 0;
			this.transform.position = pos;
		}

		if (this.activePlayer)
		{
			if (Input.GetKey ("q"))
			{
				this.gunAngle += Time.deltaTime * 20.0f;
				this.gunAngle = Mathf.Clamp(this.gunAngle, -80, 80);
				this.gun.transform.localRotation = Quaternion.AngleAxis(this.gunAngle, Vector3.up);
			}
			else if (Input.GetKey ("e"))
			{
				this.gunAngle -= Time.deltaTime * 20.0f;
				this.gunAngle = Mathf.Clamp(this.gunAngle, -80, 80);
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
}
