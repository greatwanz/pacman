using UnityEngine;

namespace pacman
{
    /// <summary>
    /// Ghost chase state. Ghost chases Pacman.
    /// </summary>
    [CreateAssetMenu(menuName = "GhostState/Ghost Chase State")]
    public class GhostChaseState : GhostState
    {
        public override void Init(Ghost g)
        {
            Debug.Log("test");
            g.meshRenderer.material.color = g.defaultColour;
        }

        public override void Execute(Ghost g)
        {
        }
    }
}