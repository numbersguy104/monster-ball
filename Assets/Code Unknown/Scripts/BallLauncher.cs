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

    [Tooltip("Cooldown between uses of the launcher")]
    [SerializeField] float maxCooldown = 1.0f;

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
    float cooldown = 0.0f;

    //How long the button to "pull back" the launcher has been held, in seconds
    float chargeTime = 0.0f;
    //charge one time
    private bool isCharging = false;
    InputAction chargeAction;

    
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
        //If on cooldown, do nothing else, just tick down the cooldown timer
        if (cooldown > 0.0f)
        {
            cooldown = Mathf.Max(cooldown - Time.deltaTime, 0.0f);
        }
        else
        {
            bool chargeHeld = chargeAction.IsPressed();

            if (chargeHeld)
            {
                if (!isCharging)
                {
                    //start charge play once
                    isCharging = true;
                    if (chargeTime <= 0.0f && chargeSource != null && !chargeSource.isPlaying)
                    {
                        chargeSource.volume = SoundManager.Instance.launcherLoopVolume;
                        chargeSource.Play();
                    }
                    StartCharging();
                }
                chargeTime = chargeTime + Time.deltaTime;

                
            }
            else if (isCharging && chargeTime > Mathf.Epsilon)
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
                List<AbstractBall> launchBalls = new List<AbstractBall>();

                //Get the closest ball to the launcher, within launchRange
                AbstractBall firstBall = null;
                float shortestDistance = launchRange;
                foreach (AbstractBall ball in FindObjectsByType<AbstractBall>(FindObjectsSortMode.None))
                {
                    float distance = Vector3.Distance(transform.position, ball.transform.position);
                    if (distance < shortestDistance)
                    {
                        firstBall = ball;
                        shortestDistance = distance;
                    }
                }

                //Recursively store the balls close to and behind the first one
                //This handles multiple balls stacked in the launcher
                if (firstBall != null)
                {
                    AbstractBall currentBall = firstBall;
                    do
                    {
                        launchBalls.Add(currentBall);

                        Transform currentTransform = currentBall.transform;
                        Collider[] nearbyColliders = Physics.OverlapSphere(currentTransform.position, currentTransform.lossyScale.x * 1.1f);
                        currentBall = null;
                        foreach (Collider col in nearbyColliders)
                        {
                            if (col.transform.position.z < currentTransform.position.z)
                            {
                                AbstractBall ball = col.GetComponent<AbstractBall>();
                                currentBall = ball;
                            }
                        }
                    } while (currentBall != null);

                    //Launch the balls!
                    foreach (AbstractBall ball in launchBalls)
                    {
                        ball.SetVelocity(0, 0, power);
                    }
                }

                chargeTime = 0;
                cooldown = maxCooldown;
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
    void StartCharging()
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