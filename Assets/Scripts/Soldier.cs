using UnityEngine;

public class Soldier : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Bullet")
            Die("DieByShot_soldier");

        if (other.gameObject.tag == "Grenade")
            Die("DieByExplosion_soldier");
    }

    private void Die(string animationName)
    {
        var animator = GetComponent<Animator>();
        var collider = GetComponent<BoxCollider2D>();
        var rigidbody = GetComponent<Rigidbody2D>();

        collider.enabled = false;
        rigidbody.gravityScale = 0;
        animator.Play(animationName);
        Destroy(gameObject, animator.GetCurrentAnimatorClipInfo(0).Length);
    }
}
