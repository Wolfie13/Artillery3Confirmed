using UnityEngine;
using System.Collections;

public class EntityScript : MonoBehaviour
{
	// TODO: Add stamina/fuel

	// health variables
	public int maxHealth;
	int currentHealth = 0;
	TextMesh healthNumbers;

	// stamina variables
	public int maxStamina;
	int currentStamina = 0;

	public GameObject tankCannon;
	TankGunRotation gunRotation;

	bool dead = false;

	public void PassInput (InputCode inputCode)
	{
		// pass input for firing and all
		if (inputCode == InputCode.Up)
		{
			gunRotation.Rotate (1);
		}

		else if (inputCode == InputCode.Down)
		{
			gunRotation.Rotate (-1);
		}
	}

	public bool IsDead ()
	{
		return dead;
	}

	public void AdjustCurrentHealth (int adj)
	{
		currentHealth += adj;
		
		if (currentHealth < 0) {currentHealth = 0;}
		if (currentHealth == 0) {dead = true;}

		if (currentHealth > maxHealth) {currentHealth = maxHealth;}
		if (maxHealth < 1) {maxHealth = 1;}
	}

	// Use this for initialization
	void Start ()
	{
		gunRotation = tankCannon.GetComponent (typeof (TankGunRotation)) as TankGunRotation;

		healthNumbers = gameObject.GetComponentInChildren (typeof (TextMesh)) as TextMesh;
		healthNumbers.transform.position = new Vector3 (transform.position.x,
		                                                transform.position.y + 4,
		                                                transform.position.z);
		currentHealth = maxHealth;
		currentStamina = maxStamina;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!dead)
		{
			AdjustCurrentHealth (0);
			healthNumbers.text = currentHealth.ToString ();
		}
	}
}
