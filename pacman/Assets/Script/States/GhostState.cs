using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GhostState : ScriptableObject
{
    public Constants constants;
    public AudioResources audioResources;

    public abstract void Init(Ghost g);

    public abstract void Execute(Ghost g);
}
