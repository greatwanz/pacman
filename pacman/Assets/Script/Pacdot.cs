using UnityEngine;

namespace pacman
{
    /// <summary>
    /// A consumable pacdot
    /// </summary>
    public class Pacdot : MonoBehaviour
    {
        [AssertNotNull]public Constants constants;
        [AssertNotNull]public Variables variables;

        void OnTriggerEnter(Collider col)
        {
            PacmanController p = col.GetComponent<PacmanController>();

            if (p != null)
            {
                //increment score
                p.Score += constants.pacdotScoreValue;

                //Choose clip to play
                AudioClip sfxClip;
                sfxClip = !p.isKa ? p.waSFX : p.kaSFX;
                AudioManager.PlaySFX(sfxClip);

                //To next sfx
                p.isKa = !p.isKa;
                Destroy(gameObject);
            }
        }
    }

}