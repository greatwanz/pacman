using UnityEngine;

namespace pacman
{
    /// <summary>
    /// A consumable pacdot
    /// </summary>
    public class PacdotConsumable : Consumable
    {
        //'Wa' sfx
        [AssertNotNull]public AudioClip waSFX;
        //'Ka' sfx
        [AssertNotNull]public AudioClip kaSFX;

        protected override void OnTriggerEnter(Collider col)
        {
            PacmanController p = col.GetComponent<PacmanController>();

            if (p != null)
            {
                //increment score
                p.Score += constants.pacdotScoreValue;

                //Choose clip to play
                AudioClip sfxClip;
                sfxClip = !p.isKa ? waSFX : kaSFX;
                AudioManager.PlaySFX(sfxClip);

                //To next sfx
                p.isKa = !p.isKa;
                Destroy(gameObject);
            }
        }
    }

}