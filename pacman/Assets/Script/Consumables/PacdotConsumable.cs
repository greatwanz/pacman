using UnityEngine;

public class PacdotConsumable : Consumable
{
    public static int wakaIndex;

    public AudioClip waSFX;
    public AudioClip kaSFX;

    protected override void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == "Pacman")
        {
            PacmanController p = col.GetComponent<PacmanController>();
            p.Score += constants.pacdotScoreValue;

            if (wakaIndex % 2 == 0)
                p.controllerAudioSource.PlayOneShot(waSFX);
            else
                p.controllerAudioSource.PlayOneShot(kaSFX);
            
            wakaIndex++;
            Destroy(gameObject);
        }
    }
}
