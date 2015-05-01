using UnityEngine;
using System.Collections;

public class CollisionScript : MonoBehaviour
{
	public int damageOnHit = 15;
	float destroyTime = 15F;

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
	}
}
