using UnityEngine;
using UnityEngine.UI;

public class MonsterHealthBar : MonoBehaviour
{
    [Header("UI References")]
    public Image redBar;   // ��ɫѪ���������½���
    public Image yellowBar; // ��ɫѪ�����ӳ��½���

    [Header("Settings")]
    public float yellowBarSpeed = 0.5f; // �����½��ٶȣ�Խ��Խ�죩

    private MonsterController monster;
    private float targetFill = 1f; // ����Ŀ�����
    private float yellowFill = 1f; // ������ǰ����

    void Start()
    {
        monster = GetComponentInParent<MonsterController>();
        if (monster == null)
        {
            Debug.LogWarning("MonsterHealthBar: û�ҵ� MonsterController��");
        }
    }

    void Update()
    {
        if (monster == null) return;

        // ����Ѫ������
        float hpRatio = Mathf.Clamp01((float)monster.hp / monster.GetMaxHP());

        // ������������
        targetFill = hpRatio;
        redBar.fillAmount = targetFill;

        // ��������׷��
        if (yellowBar.fillAmount > targetFill)
        {
            yellowBar.fillAmount = Mathf.MoveTowards(
                yellowBar.fillAmount,
                targetFill,
                yellowBarSpeed * Time.deltaTime
            );
        }
        else
        {
            yellowBar.fillAmount = targetFill;
        }
    }
}
