using UnityEngine;
using System.Collections;

public class CollisionScript : MonoBehaviour
{
	public int damageOnHit = 15;
	float destroyTime = 15F;
	public GameObject Explosion;
	
	TurnController turnControllerScript;

	void Start ()
	{
		GameObject turnController = GameObject.FindGameObjectWithTag ("TurnController");
		turnControllerScript = turnController.GetComponent (typeof (TurnController)) as TurnController;
	}

	// Update is called once per frame
	void Update ()
	{
		Destroy (gameObject, destroyTime);
	}

	void OnCollisionEnter (Collision col)
	{
		if(col.gameObject.tag == "Player")
		{
			EntityScript player = gameObject.GetComponent (typeof (EntityScript)) as EntityScript; 
			player.AdjustCurrentHealth ( -damageOnHit );
		}
		GameObject Boom = (GameObject)Instantiate (Explosion, transform.position, transform.rotation);
		Destroy (Boom, 4f);
		turnControllerScript.EndTurn ();
	}
}
