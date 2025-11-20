using UnityEngine;

public class MonsterAudio : MonoBehaviour
{
    public AudioSource deathSource;

    public void PlayDeathSound()
    {
        if (deathSource != null && deathSource.clip != null)
            deathSource.PlayOneShot(deathSource.clip);
    }
}