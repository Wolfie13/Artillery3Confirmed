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
	public Texture2D fuelTexture;
	public int maxFuel;
	float currentFuel = 1.0F;

	public GameObject tankCannon;
	public GameObject firingPoint;
	TankGunRotation gunRotation;
	Shooting gunShooting;

	// active?
	bool isActive = false;
	bool isDead = false;

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

		if (inputCode == InputCode.Fire)
		{
			gunShooting.Shoot (0);
			//turnControllerScript.EndTurn ();
		}
	}

	public bool IsDead ()
	{
		return isDead;
	}

	public void AdjustCurrentHealth (int adj)
	{
		currentHealth += adj;
		
		if (currentHealth < 0) {currentHealth = 0;}
		if (currentHealth == 0) {isDead = true;}

		if (currentHealth > maxHealth) {currentHealth = maxHealth;}
		if (maxHealth < 1) {maxHealth = 1;}
	}

	public void ActiveSwitch ()
	{
		isActive = !isActive;

		if (isActive)
		{
			currentFuel = 1.0F;
		}
	}

	// Use this for initialization
	void Start ()
	{
		gunRotation = tankCannon.GetComponent (typeof (TankGunRotation)) as TankGunRotation;
		gunShooting = firingPoint.GetComponent (typeof (Shooting)) as Shooting;

		healthNumbers = gameObject.GetComponentInChildren (typeof (TextMesh)) as TextMesh;
		healthNumbers.transform.position = new Vector3 (transform.position.x,
		                                                transform.position.y + 4,
		                                                transform.position.z);
		currentHealth = maxHealth;
		currentFuel = maxFuel;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!isDead)
		{
			AdjustCurrentHealth (0);
			healthNumbers.text = currentHealth.ToString ();
		}
	}

	void OnGUI ()
	{
		if (isActive)
		{
			GUILayout.BeginArea (new Rect (5, Screen.height - 25, maxFuel, 20));

				GUILayout.BeginArea (new Rect (0, 0, currentFuel * maxFuel, 20));

				GUI.Box (new Rect (0, 0, maxFuel, 20), fuelTexture);

				GUILayout.EndArea ();

			GUILayout.EndArea ();
		}
	}
}
