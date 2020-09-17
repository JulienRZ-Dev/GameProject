using UnityEngine;

public class SpiderBotMovement : MonoBehaviour
{
    public float maxSpeed;
    public Rigidbody2D rb; // the spider rigidbody
    public GameObject target; // the target of the spide (i.e the player)
    public SpriteRenderer spriteRenderer; // the spider sprite renderer, will be use to flip the sprite
    public Animator animator;

    private Vector3 velocity = Vector3.zero; // ???
    private Vector3 targetVelocity; // The spider velocity
    private float currentSpeed;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentSpeed = maxSpeed;
    }


    void FixedUpdate()
    {
        Flip();
        targetVelocity = new Vector2(currentSpeed * Time.deltaTime, 0f); // Set the new velocity
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, 1f); // apply the movement
        animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x)); // trigger the animation transition, gives as parameter the absolute speed
    }


    public void Flip() // flip the sprite and adapt the speed
    {
        if (target.transform.position.x < transform.position.x)
        {
            spriteRenderer.flipX = true;
            currentSpeed = -maxSpeed;
        }
        else
        {
            spriteRenderer.flipX = false;
            currentSpeed = maxSpeed;
        }
    }
}
