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
    public GameObject[] HitMonsterEffects;     // 随机播放红色撞击 or Ground Hit
    public GameObject Star_hit;
    public GameObject CFXR3_Hit_Ice_B_Air;
    public GameObject Sparks_flashing_white;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void PlayVFX(GameObject prefab, Vector3 pos, Quaternion rot)
    {
        if (prefab == null) return;
        GameObject fx = Instantiate(prefab, pos, rot);
        Destroy(fx, 5f); // 自动销毁
    }

    public void PlayRandom(GameObject[] prefabs, Vector3 pos, Quaternion rot)
    {
        if (prefabs == null || prefabs.Length == 0) return;
        int i = Random.Range(0, prefabs.Length);
        PlayVFX(prefabs[i], pos, rot);
    }
}
