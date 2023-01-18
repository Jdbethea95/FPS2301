using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveData
{
    void Load(GameData data);

    void Save(ref GameData data);
}
