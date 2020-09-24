using TMPro;
using UnityEngine;

public class SpiderBotState : MonoBehaviour
{
    public int health;
    public Animator animator;
    public Rigidbody2D rb; 
    public SpriteRenderer spriteRenderer;
    public Vector2 repulsiveForce;



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("PlayerWeapon"))
        {
            TakeDamages(10);
            if(spriteRenderer.flipX)
            {
                repulsiveForce = new Vector2(500, 0);
            }
            else
            {
                repulsiveForce = new Vector2(-500, 0);
            }
            rb.AddForce(repulsiveForce);
        }
    }


    public void TakeDamages(int _damages)
    {
        health -= _damages;
        if(health <= 0)
        {
            DestroySpider();
        }
    }


    public void DestroySpider()
    {
        Destroy(gameObject);
    }
}
