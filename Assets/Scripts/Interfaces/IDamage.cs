using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamage
{

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dmg">Decreases Ovject's Health based on Damage passed in Destroys or Handles object death</param>
    void TakeDamage(int dmg);

}
