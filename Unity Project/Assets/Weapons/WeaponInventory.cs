using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// A container class for equippable weapons for the tank.
/// </summary>
public class WeaponInventory : MonoBehaviour
{
    #region Variables

    // Unity modifiable variables.
    [SerializeField] private List<Weapon> m_weapons = new List<Weapon>(); //!< The list of available weapons.

    // Local data.
    private int m_currentWeapon = 0; //!< The currently selected weapon.

    #endregion


    #region Properties

    /// <summary>
    /// Gets or sets the currently equipped weapon index. This will never overflow to ensure the index never exceeds the count.
    /// </summary>
    public int weaponIndex
    {
        get { return m_currentWeapon; }
        set
        {
            m_currentWeapon = value % m_weapons.Count;
        }
    }

    /// <summary>
    /// Gets the currently equipped Weapon.
    /// </summary>
    public Weapon equippedWeapon
    {
        get { return m_weapons[m_currentWeapon]; }
    }

    #endregion


    #region Functionality

    /// <summary>
    /// Removes invalid weapons and checks if the count is valid.
    /// </summary>
	void Awake() 
    {
	    for (int i = 0; i < m_weapons.Count; ++i)
        {
            if (m_weapons[i] == null)
            {
                m_weapons.RemoveAt (i);
                --i;
            }
        }

        if (m_weapons.Count == 0)
        {
            throw new System.NullReferenceException (name + ".WeaponInventory: m_weapons contains no weapons.");
        }
    }

    #endregion
}
