using UnityEngine;

namespace pacman
{
    /// <summary>
    /// Ghost runs away from Pacman, while turning randomly.
    /// </summary>
    [CreateAssetMenu(menuName = "GhostState/Ghost Frightened State")]
    public class GhostFrightenedState : GhostState
    {
        //GhostState to switch to after frightened state expires
        public GhostState ghostChaseState;
        //Colour ghosts turn into when frightened
        public Color frightenedColour;

        public override void Init(Ghost g)
        {
            g.meshRenderer.material.color = frightenedColour;
        }

        public override void Execute(Ghost g)
        {

        }
    }
}