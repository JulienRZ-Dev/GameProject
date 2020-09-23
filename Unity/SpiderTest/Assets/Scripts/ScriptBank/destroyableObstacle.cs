using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
//using UnityEngine.ParticleSystemModule;

public class destroyableObstacle : MonoBehaviour
{
    public float pushBackForce = 220f;

    void DestroyTile(Collision2D collision)
    {
        Tilemap tilemap = GetComponent<Tilemap>();
        Vector3 hitPosition = Vector3.zero;
        foreach (ContactPoint2D hit in collision.contacts)
        {
            hitPosition.x = hit.point.x - 0.1f;
            hitPosition.y = hit.point.y - 0.1f;
            Vector3Int cell = new Vector3Int((int)hitPosition.x, (int)hitPosition.y, 0);

            tilemap.SetTile(tilemap.WorldToCell(hitPosition), null);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && (collision.relativeVelocity.magnitude > 5))
        {
            DestroyTile(collision);
            collision.rigidbody.AddForce(new Vector2(0f, pushBackForce));
            collision.gameObject.GetComponent<Animator>().SetBool("isJumping", true);
        }
    }

}
