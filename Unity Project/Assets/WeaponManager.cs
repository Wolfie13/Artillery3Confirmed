using UnityEngine;
using System.Collections;

public class WeaponManager : MonoBehaviour {

	public int WeaponChoice;
	public GameObject[] Explosions;
	public GameObject ClusterProjectile;
	bool spawncheck = true;

	float timer = 0.5f; 
	bool explodeOnHit = false;
	
	// Use this for initialization
	void Start () {
		this.gameObject.tag = "Projectile";
	}
	
	// Update is called once per frame
	void Update () {
		if(timer < 0)
		{
			explodeOnHit = true;
		}

		else
		{
			timer -= Time.deltaTime;
		}
	}

	IEnumerator OnCollisionStay(Collision C){
			if (C.gameObject.tag != "Projectile" && explodeOnHit) {
				switch (WeaponChoice) {
	
				case 1: // Rocket, Rail gun (Things that dont need to do anything else on impact)
					CallExplosion ();
					break;
				case 2: // Cluster Bomb
					CallExplosion ();
					GenerateCluster ();
					break;
				case 3: // Granade
					yield return new WaitForSeconds (3);
					CallExplosion ();
					break;
				case 4: // 
					CallExplosion ();
					break;
				}
				GameObject.Destroy (gameObject);
			}
	}

	void CallExplosion (){
		GameObject Boom = (GameObject)Instantiate (Explosions[WeaponChoice - 1], transform.position, transform.rotation);
		Destroy (Boom, 4f);
	}

	void GenerateCluster (){
		for (int x = 0; x < 4; x++){
			GameObject instantiatedProjectile = Instantiate(ClusterProjectile, transform.position, Quaternion.identity) as GameObject;
			instantiatedProjectile.rigidbody.velocity = transform.TransformDirection(Vector3.up * 35);
		}
	}
}
