using UnityEngine;

public class FullBall : AbstractAbilityBall
{
    /*[SerializeField] IceZone iceZonePrefab;
    [Header("Ball Settings")]
    [Tooltip("Ice zone duration")]
    [SerializeField] private float zoneDuration = 5.0f;
    [Tooltip("Ice zone size ralated to ball")]
    [SerializeField] private float zoneScale = 1.0f;*/

    protected override void Skill()
    {
        //CreateTrail();
    }

    protected override void Awake()
    {
        BallInit("FullBall");
        //zoneScale *= GameStatsManager.Instance.Iceball_ZoneSize;
    }

    private void CreateTrail()
    {
       /* var trail = Instantiate(iceZonePrefab, transform.position, transform.rotation);
        trail.transform.up = GetVelocity();
        float size = transform.lossyScale.x * zoneScale;
        trail.transform.localScale = new Vector3(size, size, size);
        trail.transform.localRotation = Quaternion.identity;
        trail.GetComponent<IceZone>().SetDuration(zoneDuration);*/
    }

}
