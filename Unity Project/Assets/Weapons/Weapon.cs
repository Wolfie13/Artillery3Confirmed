using UnityEngine;
using System.Collections;


/// <summary>
/// A wrapper for projectiles to be used by some kind of inventory/manager.
/// </summary>
[System.Serializable]
public class Weapon
{
                        public string       name        = "Bob";    //!< The name of the weapon to be displayed by the GUI.
    [Range (0.1f, 60f)] public float        timer       = 60f;      //!< How long until the weapon should self-destruct.
                        public Projectile   projectile  = null;     //!< The projectile to fire from the weapon.


    /// <summary>
    /// Fires the projectile from the weapon.
    /// </summary>
    /// <param name="position"> The position to spawn the projectile. </param>
    /// <param name="velocity"> The velocity to apply to the projectile. </param>
    /// <param name="rotation"> The rotation to apply to the projectile. </param>
    public void Fire (Vector3 position, Vector3 velocity, Quaternion rotation)
    {
        // Create the object in the scene.
        Projectile spawnedProjectile = GameObject.Instantiate (projectile, position, rotation) as Projectile;
        
        // Set the projectile up correctly.
        spawnedProjectile.explodeTimer = timer;
        spawnedProjectile.rigidbody.velocity = velocity;
    }
}
