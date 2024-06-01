using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Transform bulletSpawnZone; 
    [SerializeField] private float moveSpeed = 120.0f;
    [SerializeField] private float bulletLifetime = 2.0f;

    public void Setup()
    {
        var bulletPhysics = GetComponent<Rigidbody2D>();
        bulletPhysics.AddForce(PlayerController.direction.normalized * moveSpeed * Time.fixedDeltaTime, ForceMode2D.Impulse);
        Destroy(gameObject, bulletLifetime);

        if (PlayerController.direction.y != 0) 
            transform.eulerAngles = new Vector3(0, 0, 90);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Destroy(gameObject);
        }
    }
}
