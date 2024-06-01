using UnityEngine;

public class Grenade : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D grenadePhyics;

    public Transform p;

    [SerializeField] private float moveSpeed = 120.0f;
    [SerializeField] private float angularVelocity = 100.0f;
    [SerializeField] private float grenadeLiftime = 1.0f;
    private Vector2 anguledVector;

    private void Start()
    {
        Physics2D.IgnoreCollision(p.GetComponent<Collider2D>(), GetComponent<Collider2D>());
    }

    private void Awake()
    {
        grenadePhyics = GetComponent<Rigidbody2D>();
        anguledVector = (PlayerController.direction == Vector3.right ? Quaternion.Euler(0, 0, 60) : Quaternion.Euler(0, 0, -60)) * PlayerController.direction.normalized;
    }

    public void Setup()
    {
        grenadePhyics.AddForce(anguledVector * moveSpeed * Time.fixedDeltaTime, ForceMode2D.Impulse);
        grenadePhyics.angularVelocity = angularVelocity;
        Invoke("DestroyGrenade", grenadeLiftime);
    }

    private void DestroyGrenade()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
        var animator = GetComponent<Animator>();
        animator.Play("Explosion");
        grenadePhyics.freezeRotation = true;
        Destroy(gameObject, animator.GetCurrentAnimatorStateInfo(0).length - 0.6f);
    }

    private int bounceCounter = 0;
    private void OnCollisionEnter2D(Collision2D other)
    {
        // FIXME: Physics2D.IgnoreCollision(other.collider, GetComponent<BoxCollider2D>());

        if (other.gameObject.tag == "Ground")
        {
            bounceCounter += 1;
            switch (bounceCounter)
            {
                case 1:
                    grenadePhyics.AddForce((anguledVector * moveSpeed * Time.fixedDeltaTime) * 0.25f, ForceMode2D.Impulse);
                    break;
                case 2:
                    grenadePhyics.AddForce((anguledVector * moveSpeed * Time.fixedDeltaTime) * 0.15f, ForceMode2D.Impulse);
                    break;
                default:
                    grenadePhyics.AddForce(Vector2.zero, ForceMode2D.Impulse);
                    break;
            }

        }

        if (other.gameObject.tag == "Enemy")
        {
            DestroyGrenade();
        }
    }
}
