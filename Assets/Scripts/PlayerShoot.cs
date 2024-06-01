using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private Transform bulletPr;
    [SerializeField] private Transform grenadePr;
    [SerializeField] private Transform bulletSpawnZone;

    [Header("Animators")]
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private Animator upperBodyAnimator;
    [SerializeField] private Animator lowerBodyAnimator;

    private float shootTimer;
    private float greanadeThrowTimer;
    [SerializeField] private float fireRate = 0.2f;
    [SerializeField] private float grenadeThrowRate = 0.3f;
    private bool canShoot => shootTimer > fireRate;
    private bool canThrow => greanadeThrowTimer > grenadeThrowRate;

    void Start()
    {
        playerAnimator = gameObject.GetComponent<Animator>();
    }

    void Update()
    {

        shootTimer += Time.deltaTime;
        greanadeThrowTimer += Time.deltaTime;

        if (Input.GetButtonDown("Fire1") && canShoot)
        {
            var newBullet = Instantiate(bulletPr, bulletSpawnZone.position, Quaternion.identity);
            newBullet.GetComponent<Bullet>().Setup();

            upperBodyAnimator.SetBool("isShooting", true);
            playerAnimator.SetBool("isShooting", true);
            shootTimer = 0.0f;
            Invoke("CutShootAnimation", 0.4f);
        }


        if (Input.GetButtonDown("Fire2") && canThrow && Input.GetAxisRaw("Vertical") >= 0)
        {
            var newBullet = Instantiate(grenadePr, transform.position, Quaternion.identity);
            newBullet.GetComponent<Grenade>().Setup();

            upperBodyAnimator.SetBool("isThrowing", true);
            greanadeThrowTimer = 0.0f;
            Invoke("CutGrenadeThrowAnimation", grenadeThrowRate);
        }

        if (Input.GetButtonDown("Fire2") && canThrow)
        {
            playerAnimator.SetBool("isThrowing", true);
            greanadeThrowTimer = 0.0f;
            Invoke("CutGrenadeThrowAnimation", grenadeThrowRate);
        }
    }

    private void CutShootAnimation()
    {
        upperBodyAnimator.SetBool("isShooting", false);
        playerAnimator.SetBool("isShooting", false);
    }

    private void CutGrenadeThrowAnimation()
    {
        upperBodyAnimator.SetBool("isThrowing", false);
        playerAnimator.SetBool("isThrowing", false);
    }
}
