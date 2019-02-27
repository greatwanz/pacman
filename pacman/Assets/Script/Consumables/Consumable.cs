using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An item that can be consumed by pacman
/// </summary>
public abstract class Consumable : MonoBehaviour
{
    public Constants constants;

    protected abstract void OnTriggerEnter(Collider col);
}
