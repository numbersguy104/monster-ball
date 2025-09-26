using System;
using UnityEngine;
using DG.Tweening;

public class CactusPA : MonoBehaviour
{
    public Animator animator;
    private void Start()
    {
        animator.Play("Cactus_IdleBattle");
        var targetPosition = transform.position;
        targetPosition.x += -0.2f;
        transform.DOMove(targetPosition, 1f)
            .SetLoops(-1, LoopType.Yoyo)  
            .SetEase(Ease.Linear);
    }
}
