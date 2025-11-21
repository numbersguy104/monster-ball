using UnityEngine;

public class BallAudio : MonoBehaviour
{
    [Header("Skill Sound")]
    public AudioSource skillSource; 
    

    public void PlaySkillSound()
    {
        if (skillSource != null && skillSource.clip != null)
            skillSource.PlayOneShot(skillSource.clip);
    }

}