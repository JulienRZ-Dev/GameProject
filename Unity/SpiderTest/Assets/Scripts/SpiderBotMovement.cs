using UnityEngine;
using System.Collections;

public class SpiderBotMovement : MonoBehaviour
{
    // UNITY COMPONENTS
    public Rigidbody2D rb; // the spider rigidbody
    public SpriteRenderer spriteRenderer; // the spider sprite renderer, will be use to flip the sprite
    public Animator animator;
    public LineRenderer aim; //the spider starts aiming the player to show that he is in its sight

    // TARGET
    public Transform target; // the current target of the spider
    public Transform playerTransform; // the player Transform
    public Transform[] waypoints; // the waypoints for the spider rest movement
    public int destPointIndex; // the current destination point index  

    //  BOUNDS
    public Transform[] bounds; // two points determines the spider x boundaries
    
    // SPEED AND MOVEMENT
    private Vector3 velocity = Vector3.zero; // ???
    private Vector3 targetVelocity; // The spider velocity
    public float maxSpeed; // speed when the spider is in seek mode
    public float casualSpeed; // speed when the spider is in rest mode
    private float currentSpeed;
    private bool speedIsLocked; // prevent the speed from changing when the spider should freeze

    // SMOOTH DAMP
    private float smoothDampTime; // the smoothDampTime value (to smooth the movement more or less)

    // FOCUS
    private bool hasFocusPlayer; // true when the spider focus the player
    private bool stopSeeking; // the spider should not seek and replace to its zone 
    public float spiderFocusRange; // focus range for the spider ( detection zone )
    public Transform spiderLaserEye; // The origin of the laser

    // SPIDER BOUNDS
    private bool isOutOfBounds;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        aim.startWidth = 0.05f;
        aim.endWidth = 0.5f;
        Color clear_red = Color.red;
        clear_red.a = 0.5f;
        aim.startColor = clear_red;
        aim.endColor = clear_red;
        aim.enabled = false;

        destPointIndex = 1;
        target = waypoints[destPointIndex];

        currentSpeed = casualSpeed;
        speedIsLocked = false;

        smoothDampTime = 0.1f;

        hasFocusPlayer = false;
        stopSeeking = false;

        isOutOfBounds = false;
    }


    void FixedUpdate()
    {
        if (!speedIsLocked) // If the speed is locked, that means that the spider should freeze (i.e. don't move)
        {
            OutOfBoundsManager(); // check if the spider is out of its zone, and replace it if yes

            UpdateFocus(); // focus or unfocus the player, change the movement behaviour

            Flip(); // check if the spider should flip according to it's speed

            if (hasFocusPlayer)
            {
                Seek(); // the spider is seeking the player
            }
            else
            {
                Move(); // the spider moves casually, normalize the current speed to casaul speed ( to get the (-) )
            }

            targetVelocity = new Vector2(currentSpeed * Time.fixedDeltaTime, 0f); // Set the new velocity
            rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, smoothDampTime); // apply the movement
        }
        animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x)); // trigger the animation transition, gives as parameter the absolute speed
    }


    public void OutOfBoundsManager()
    {
        if(transform.position.x > bounds[0].position.x && transform.position.x < bounds[1].position.x)
        {
            isOutOfBounds = false;
        }
        else if (transform.position.x < bounds[0].position.x) // check if the spider is out of left bounds
        {
            stopSeeking = true; // the spider gives up the seek
            isOutOfBounds = true;
            destPointIndex = 1; // target the right waypoint
        }
        else if (transform.position.x > bounds[1].position.x) // check if the spider is out of right bounds
        {
            stopSeeking = true; // the spider gives up the seek
            isOutOfBounds = true;
            destPointIndex = 0; // target the left waypoint
        }
    }


    public void UpdateFocus() // focus or unfocus the player, change the movement behaviour 
    {
        if(Mathf.Abs(playerTransform.position.x - transform.position.x) < spiderFocusRange && !isOutOfBounds && !stopSeeking) // the player enters the spider's focus range
        {
            if((playerTransform.position.x > transform.position.x && currentSpeed > 0) || (playerTransform.position.x < transform.position.x && currentSpeed < 0)) // the spider only focus when its in front of the player
            {
                if(!hasFocusPlayer)
                    StartCoroutine(Aim());
                hasFocusPlayer = true;
            }
        }
        else 
        {
            hasFocusPlayer = false;
        }
    }


    public IEnumerator Aim()
    {
        speedIsLocked = true;
        yield return new WaitForSeconds(0.2f);
        for (int i = 0; i < 6; i++)
        {
            UpdateLaserPosition();
            aim.enabled = !aim.enabled;
            yield return new WaitForSeconds(0.1f);
        }
        speedIsLocked = false;
    }


    public void UpdateLaserPosition()
    {
        aim.SetPosition(0, spiderLaserEye.position);
        aim.SetPosition(1, playerTransform.position);
    }


    public void Flip()
    {
        if(currentSpeed > 0.1)
        {
            spriteRenderer.flipX = false;
        }
        else if(currentSpeed < -0.1)
        {
            spriteRenderer.flipX = true;
        }
    }


    public void Move() // the spider moves casually
    {
        target = waypoints[destPointIndex];
        smoothDampTime = 0f;

        if (waypoints[destPointIndex].position.x < transform.position.x) // check where the spider should go and adapt the speed
        {
            currentSpeed = -casualSpeed;
        }
        else
        {
            currentSpeed = casualSpeed;
        }

        if (Mathf.Abs(transform.position.x - target.position.x) < 0.3f) // Trigger just before the snake arrives to its waypoint
        {
            destPointIndex = (destPointIndex + 1) % waypoints.Length; // toggle the waypoint index
            target = waypoints[destPointIndex]; // Toggle the target waypoint

            if (stopSeeking) // if the spider was replacing to its zone, ( get ready to seek again when a waypoint is reached )
            {
                stopSeeking = false; // the spider is ready to seek again
            }
        }
    }


    public void Seek() // flip the sprite and adapt the speed
    {
        target = playerTransform;
        smoothDampTime = 0.2f;

        if (target.position.x < transform.position.x)
        {
            currentSpeed = -maxSpeed;
        }
        else
        {
            currentSpeed = maxSpeed;
        }
    }
}
