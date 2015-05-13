using UnityEngine;
using System.Collections;


/// <summary>
/// A simple projectile class for firing out of tank-like cannons.
/// </summary>
[RequireComponent (typeof (Rigidbody))]
public sealed class Projectile : MonoBehaviour 
{
    #region Variables 

    // Explosive properties.
	[SerializeField]                    private float m_damage         = 1f;    //!< The damage the projectile deals on hit.
    [SerializeField]                    private float m_explosiveForce = 10f;   //!< The force to be applied to spawned projectiles and colliding objects.
    [SerializeField, Range (0.1f, 50f)] private float m_range          = 10f;   //!< The area of effect for the projectile.
    [SerializeField, Range (0.1f, 5f)]  private float m_maxDamageRange = 0.5f;  //!< The range at which maximum, unscaled damage will be applied.
    [SerializeField, Range (0.1f, 60f)] private float m_explodeTimer   = 10f;   //!< Destroy the projectile after the specified time.
    [SerializeField]                    private bool  m_explodeOnHit   = true;  //!< Causes an explosion upon hitting a collidable object.
    
    // Spawning properties.
    [SerializeField]                 private Projectile m_spawnOnDetonation = null; //!< The projectile to spawn on detonation.
    [SerializeField, Range (0, 10)]  private int        m_spawnCount        = 0;    //!< How many projectiles to spawn on detonation.
    [SerializeField, Range (0f, 1f)] private float      m_maintainVelocity  = 0.5f; //!< How much velocity will be maintained by the spawned projectiles.
    
    // Special effects.
    [SerializeField] private GameObject m_explosion = null; //!< The object created upon collision with another object.

    // Misc crap.
    private const int collidableMask = (1 << Layers.terrain) | (1 << Layers.tank);  //!< The layer mask to use when using Physics.OverlapSphere().

	#endregion


    #region Properties

    /// <summary>
    /// Gets or sets the maximum damage of the projectile. Can be positive or negative.
    /// </summary>
    public float damage
    {
        get { return m_damage; }
        set { m_damage = value; }
    }

    /// <summary>
    /// Gets or sets how much explosive force is applied to nearby objects.
    /// </summary>
    public float explosiveForce
    {
        get { return m_explosiveForce; }
        set { m_explosiveForce = value; }
    }

    /// <summary>
    /// Gets or sets the range of the area of effect upon explosion. Clamped to 0.1 - 50.
    /// </summary>
    public float range
    {
        get { return m_range; }
        set 
        { 
            m_range = Mathf.Clamp (value, 0.1f, 50f); 
        }
    }
    
    /// <summary>
    /// Gets or sets the time to wait until self-destruction. Clamped to 0.1 - 60.
    /// </summary>
    public float explodeTimer
    {
        get { return m_explodeTimer; }
        set 
        { 
            m_explodeTimer = Mathf.Clamp (value, 0.1f, 60f);
        }
    }

    /// <summary>
    /// Gets or sets whether the projectile should explode upon contact with another object.
    /// </summary>
    public bool explodesOnHit
    {
        get { return m_explodeOnHit; }
        set { m_explodeOnHit = value; }
    }

    #endregion


    #region Monobehaviour functionality

    /// <summary>
    /// Simply starts the detonation countdown.
    /// </summary>
    private void Awake()
    {
        StartCoroutine (Countdown());
    }

    /// <summary>
    /// Handles the OnCollisionEnter event, causing detonation if the projectile is set to explode on hit.
    /// </summary>
    /// <param name="other"> Completely unused. </param>
    private void OnCollisionEnter (Collision other)
    {
        if (m_explodeOnHit)
        {
            Detonate();
        }
    }

    #endregion


    #region Detonation

    /// <summary>
    /// A coroutine which triggers detonation of the projectile after the timer has expired.
    /// </summary>
    /// <returns> Nothing like a good boy. </returns>
    private IEnumerator Countdown()
    {
        // Wait until we should detonate.
        float time = 0f;

        while (time < m_explodeTimer)
        {
            time += Time.deltaTime;
            yield return null;
        }

        // We've detonated captain!
        Detonate();
    }

    /// <summary>
    /// Runs the detonation sequence of the projectile.
    /// </summary>
    private void Detonate()
    {
        ApplyDamage();
        SpawnProjectiles();
        SpecialEffects();
        Destroy (gameObject);
    }

    /// <summary>
    /// Apply damage to surrounding area, deforming terrain and hurting tanks.
    /// </summary>
    private void ApplyDamage()
    {
        // We need to determine if anything is nearby, if so then damage them appropriately.
        Collider[] collisions = Physics.OverlapSphere (rigidbody.position, m_range, collidableMask);

        if (collisions != null)
        {
            foreach (Collider collision in collisions)
            {
                switch (collision.gameObject.layer)
                {
                    case Layers.terrain:
                        DamageTerrain (collision);
                        break;

                    case Layers.tank:
                        DamageTank (collision);
                        break;
                        
                    default:
                        Debug.LogWarning ("Projectile::ApplyDamage(), someones done a goof!");
                        break;
                }
            }
        }        
    }

    private void DamageTerrain (Collider collider)
    {
        // Obtain the terrain script and damage the collision mask.
        Terrain terrain = collider.gameObject.GetComponent<Terrain>();
        terrain.DamageTerrain (rigidbody.position, m_range);
    }

    private void DamageTank (Collider collider)
    {
        // Obtain the controller.
        TankController tank = collider.gameObject.GetComponent<TankController>();
        
        // Use the closest point on the boundary to determine the damage distance.
        float distance = (collider.ClosestPointOnBounds (rigidbody.position) - rigidbody.position).normalized.magnitude;

        // Reduce the distance by the max damage range to determine how much to scale by.
        float damageDistance = Mathf.Max (0f, distance - m_maxDamageRange);
        float maxRange       = Mathf.Max (0f, m_range - m_maxDamageRange);

        // Perform a simple lerp to find the desired damage.
        float finalDamage = Mathf.Lerp (m_damage, 0f, damageDistance / maxRange);

        // Apply damage.
        tank.Damage (finalDamage);
    }

    /// <summary>
    /// Spawn the number of desired child projectiles.
    /// </summary>
    private void SpawnProjectiles()
    {
        // Don't do anything if the prefab doesn't exist.
        if (m_spawnOnDetonation)
        {
            // Calculate how much velocity to maintain once.
            Vector3 parentVelocity = rigidbody.velocity * m_maintainVelocity;

            // Spawn the new projectiles.
            for (int i = 0; i < m_spawnCount; ++i)
            {
                // Spawn new projectiles combining the current projectiles velocity with a random force.
                Vector3 direction = new Vector2 (Random.value, Random.value).normalized;
                Vector3 velocity  = parentVelocity + direction * (Random.value * m_explosiveForce);
                Vector3 position  = rigidbody.position + direction * 0.2f;

                Instantiate (m_spawnOnDetonation, position, Random.rotation);
            }
        }
    }

    /// <summary>
    /// Instansiate an explosion effect and play audio.
    /// </summary>
    private void SpecialEffects()
    {
        // Don't spawn an explosion if none exists.
        if (m_explosion)
        {
            Instantiate (m_explosion, rigidbody.position, rigidbody.rotation);
        }
    }

    #endregion
}
