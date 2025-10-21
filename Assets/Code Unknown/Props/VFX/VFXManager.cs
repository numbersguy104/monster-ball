using UnityEngine;

public class VFXManager : MonoBehaviour
{
    public static VFXManager Instance;

    [Header("Prefabs")]
    public GameObject vfx_Implosion_01;
    public GameObject Electro_hit;
    public GameObject vfx_Impact_01;
    public GameObject vfx_Shockwave_01;
    public GameObject CFXR2_Sparks_Rain;
    public GameObject CFXR2_Firewall_A;
    public GameObject[] HitMonsterEffects;     
    public GameObject Star_hit;
    public GameObject CFXR3_Hit_Ice_B_Air;
    public GameObject Sparks_flashing_white;
    
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public GameObject PlayVFX(GameObject prefab, Vector3 pos, Quaternion rot, float lifetime = 5f)
    {
        if (prefab == null) return null;

        GameObject fx = Instantiate(prefab, pos, rot);

        Destroy(fx, lifetime);  // Automaticlly destroy
        return fx;
    }

    public GameObject PlayRandom(GameObject[] prefabs, Vector3 pos, Quaternion rot, float lifetime = 5f)
    {
        if (prefabs == null || prefabs.Length == 0) return null;

        int i = Random.Range(0, prefabs.Length);
        return PlayVFX(prefabs[i], pos, rot, lifetime);
    }
    public GameObject PlayAndPauseAtEnd(GameObject prefab, Vector3 pos, Quaternion rot)
    {
        if (prefab == null) return null;

        GameObject fx = Instantiate(prefab, pos, rot);
        ParticleSystem ps = fx.GetComponent<ParticleSystem>();
        if (ps != null)
        {
            var main = ps.main;
            main.loop = false;
            ps.Play();
            Instance.StartCoroutine(PauseAtEnd(ps));
        }
        return fx;
    }

    private System.Collections.IEnumerator PauseAtEnd(ParticleSystem ps)
    {
        yield return new WaitForSeconds(ps.main.duration);
        ps.Pause(); // stop in the last frame
    }
}
