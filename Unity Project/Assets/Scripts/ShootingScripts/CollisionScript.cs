using UnityEngine;
using System.Collections;

public class CollisionScript : MonoBehaviour
{
	public int damageOnHit = 15;
	float destroyTime = 15F;
	
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
			EntityScript player = col.gameObject.GetComponent (typeof (EntityScript)) as EntityScript; 
			player.AdjustCurrentHealth ( -damageOnHit );
		}

		turnControllerScript.EndTurn ();
	}
}
