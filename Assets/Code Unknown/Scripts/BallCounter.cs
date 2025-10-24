using UnityEngine;

public class BallCounter : MonoBehaviour
{
    public int countActive = 0;
    public int countTotal = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<AbstractBall>() != null)
        {
            countActive++;
            countTotal++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<AbstractBall>() != null)
        {
            countActive--;
        }
    }
}
