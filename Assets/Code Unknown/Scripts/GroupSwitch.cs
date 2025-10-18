using System.Collections.Generic;
using UnityEngine;

public class GroupSwitch : MonoBehaviour
{
    //Materials to display when the switch is on or off
    [Tooltip("Material to use when the switch is on")]
    [SerializeField] Material onMaterial;
    [Tooltip("Material to use when the switch is off")]
    [SerializeField] Material offMaterial;

    //Whether the switch is lit up or not
    //(true if lit up, false if not)
    bool lit = false;

    //List of GroupSwitch components (including this one) in the same switch group
    List<GroupSwitch> switches = new List<GroupSwitch>();

    PointsTracker pt;

    void Start()
    {
        pt = FindAnyObjectByType<PointsTracker>();

        Transform group = transform.parent.parent;
        foreach (Transform switchParent in group)
        {
            foreach (Transform controller in switchParent)
            {
                GroupSwitch s = controller.gameObject.GetComponent<GroupSwitch>();
                if (s != null)
                {
                    switches.Add(s);
                }
            }
        }
    }

    public void SetState(bool state)
    {
        lit = state;

        Transform parent = transform.parent;
        foreach (Transform graphic in parent)
        {
            MeshRenderer mr = graphic.gameObject.GetComponent<MeshRenderer>();
            if (mr != null)
            {
                if (state == true)
                {
                    mr.material = onMaterial;
                }
                else
                {
                    mr.material = offMaterial;
                }
            }
        }
    }

    public bool GetState()
    {
        return lit;
    }

    void OnTriggerEnter(Collider other)
    {
        if (lit == false)
        {
            AbstractBall ball = other.gameObject.GetComponent<AbstractBall>();
            if (ball != null)
            {
                pt.AddTerrainPoints(PointsTracker.PointSources.SwitchOne);
                SetState(true);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        AbstractBall ball = other.gameObject.GetComponent<AbstractBall>();
        if (ball != null)
        {
            //If any switch in the group is off, do nothing
            foreach (GroupSwitch s in switches)
            {
                if (s.GetState() == false)
                {
                    return;
                }
            }

            //Otherwise, award extra points and reset all switches in the group
            foreach (GroupSwitch s in switches)
            {
                pt.AddTerrainPoints(PointsTracker.PointSources.SwitchGroup);
                s.SetState(false);
            }
        }
    }
}
