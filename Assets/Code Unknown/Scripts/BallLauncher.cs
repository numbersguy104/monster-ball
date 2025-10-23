using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.ProBuilder.MeshOperations;

public class BallLauncher : MonoBehaviour
{
    //Maximum velocity of the ball when launching it
    //This is multiplied by how long the launcher has been charged, percentage-wise
    [Tooltip("Maximum velocity to set the ball to (in meters per second)")]
    [SerializeField] float maxPower = 8.0f;

    //Maximum time for the launcher to be charged
    //Holding the button longer than this will not add any more power
    [Tooltip("Time to reach maximum charge (in seconds)")]
    [SerializeField] float maxCharge = 1.0f;

    [Tooltip("How close the ball needs to be to the launcher to be launched")]
    [SerializeField] float launchRange = 0.2f;

    [Tooltip("The visual object that displays the launcher's charge")]
    [SerializeField] GameObject graphic;

    [Tooltip("Relative size of the graphic")]
    [SerializeField] float graphicScale = 1.5f;

    [Header("Audio Settings")]
    [SerializeField] AudioSource chargeSource;   
    [SerializeField] AudioSource launchSource;
    AudioSource audioSource;

    GameObject chargeVfxInstance;
    GameObject activeChargeVFX;

    //How long the button to "pull back" the launcher has been held, in seconds
    float chargeTime = 0.0f;

    //charge one time
    private bool isCharging = false;
    InputAction chargeAction;

    //Return whether there is at least one pinball near the launcher
    //Use this to check if the launcher should be usable
    private bool CheckBall()
    {
        foreach (AbstractBall ball in FindObjectsByType<AbstractBall>(FindObjectsSortMode.None))
        {
            float distance = Vector3.Distance(transform.position, ball.transform.position);
            if (distance < launchRange)
            {
                return true;
            }
        }

        return false;
    }

    //Return whether there is at least one pinball ABOVE the launcher
    //Use this to check if a new ball should be spawned from the ball queue
    private bool CheckBallAbove()
    {
        foreach (AbstractBall ball in FindObjectsByType<AbstractBall>(FindObjectsSortMode.None))
        {
            //Process to get the ball's distance from the launcher's column
            //(In other words, distance ignoring the launcher's "transform.forward")
            //To do this, get the distance on the other two axes, then combine them
            Vector3 displacement = ball.transform.position - transform.position;
            float distanceX = Vector3.Dot(displacement, transform.right);
            float distanceY = Vector3.Dot(displacement, transform.up);
            float columnDistance = Mathf.Sqrt(distanceX * distanceX + distanceY * distanceY);

            if (columnDistance < launchRange)
            {
                return true;
            }
        }

        return false;
    }

    //Return all balls near the launcher
    //Use this to get which balls should be launched
    private List<AbstractBall> DetectBalls()
    {
        List<AbstractBall> result = new List<AbstractBall>();

        foreach (AbstractBall ball in FindObjectsByType<AbstractBall>(FindObjectsSortMode.None))
        {
            float distance = Vector3.Distance(transform.position, ball.transform.position);
            if (distance < launchRange)
            {
                result.Add(ball);
            }
        }

        return result;
    }

    void Start()
    {
        chargeAction = InputSystem.actions.FindAction("Charge");
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;
        audioSource.loop = false;
    }

    void Update()
    {
        //Did the player just activate the launcher?
        bool chargeUsed = chargeAction.WasPerformedThisFrame();

        //Is the player charging the launcher?
        bool chargeHeld = chargeAction.IsPressed();

        if (chargeUsed)
        {
            bool shouldStartCharging = false;
            if (CheckBall())
            {
                shouldStartCharging = true;
            }
            else if (PinballQueue.Instance.ballQueue.Count > 0 && !CheckBallAbove())
            {
                PinballQueue.Instance.NextBall();
                shouldStartCharging = true;
            }

            if (shouldStartCharging)
            {
                isCharging = true;
                StartChargingVFX();

                //SFX
                if (chargeSource != null && !chargeSource.isPlaying)
                {
                    chargeSource.volume = SoundManager.Instance.launcherLoopVolume;
                    chargeSource.Play();
                }
            }
        }

        if (isCharging)
        {
            if (chargeHeld)
            {
                chargeTime += Time.deltaTime;
            }
            else if (chargeTime > Mathf.Epsilon)
            {
                if (chargeSource != null && chargeSource.isPlaying)
                    chargeSource.Stop();
                // end loop stop audio
                if (chargeSource != null)
                {
                    launchSource.volume = SoundManager.Instance.launcherVolume;
                    launchSource.Play();
                }
                LaunchBall();

                //Force on the ball scales with charge time, up to the maximum
                float power = Mathf.Min(chargeTime, maxCharge) / maxCharge * maxPower;

                //Determine which balls should be launched by this use of the launcher
                List<AbstractBall> launchBalls = DetectBalls();

                //Launch the balls!
                if (launchBalls != null && launchBalls.Count > 0)
                {
                    foreach (AbstractBall ball in launchBalls)
                    {
                        ball.SetVelocity(0, 0, power);
                    }
                }

                chargeTime = 0;
                isCharging = false;
            }
        }

        //Update the launcher graphic if one exists
        if (graphic != null)
        {
            Vector3 scale = graphic.transform.localScale;
            scale.y = Mathf.Min(chargeTime, maxCharge) / maxCharge * graphicScale;
            graphic.transform.localScale = scale;
            graphic.transform.localPosition = new Vector3(0, 0, -scale.y / graphicScale);
        }
    }

    // VFx charging and launching 
    void StartChargingVFX()
    {
        if (activeChargeVFX == null)
        {
            // sustain until launch
            activeChargeVFX = VFXManager.Instance.PlayVFX(
                VFXManager.Instance.vfx_Implosion_01,
                transform.position,
                Quaternion.identity);
        }
    }

    void LaunchBall()
    {
        // launch vFX
        VFXManager.Instance.PlayVFX(
            VFXManager.Instance.Electro_hit,
            transform.position,
            Quaternion.identity,
            0.5f
        );

        // stop
        if (activeChargeVFX != null)
        {
            Destroy(activeChargeVFX);
            activeChargeVFX = null;
        }
    }
}