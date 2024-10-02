using UnityEngine;

public class Arrow_Controller : MonoBehaviour
{
    [SerializeField] private string targetLayerName = "Player";

    [SerializeField] private Vector2 flySpeed;
    private Rigidbody2D rb;
    private CharacterStats archerStats;

    [SerializeField] private bool canMove = true;
    [SerializeField] private bool flipped = false;

    private bool isStuck = false;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (canMove)
        {
            rb.velocity = flySpeed;
            transform.right = rb.velocity;
        }

        if (isStuck)
        {
            Invoke("BecomeTransparentAndDestroyArrow", Random.Range(3, 5));
        }

        Invoke("BecomeTransparentAndDestroyArrow", 10f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Arrow collided with " + collision.gameObject.name);

        if (collision.gameObject.layer == LayerMask.NameToLayer(targetLayerName))
        {
            if (collision.GetComponent<CharacterStats>() != null)
            {
                archerStats.DoDamage(collision.GetComponent<CharacterStats>());
                StuckIntoCollidedObject(collision);
            }

        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            StuckIntoCollidedObject(collision);
        }
    }

    public void SetupArrow(CharacterStats _archerStats, bool _isRight)
    {
        if (_isRight == false)
            flySpeed.x *= -1;

        if (flySpeed.x < 0)
            transform.Rotate(0, 180, 0);

        archerStats = _archerStats;
    }

    private void StuckIntoCollidedObject(Collider2D collision)
    {
        GetComponent<CapsuleCollider2D>().enabled = false;

        canMove = false;
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.parent = collision.transform;

        isStuck = true;
    }

    private void BecomeTransparentAndDestroyArrow()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, sr.color.a - (5 * Time.deltaTime));

        if (sr.color.a <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void FlipArrow()
    {
        if (flipped)
        {
            return;
        }

        flySpeed.x *= -1;
        flySpeed.y *= -1;
        transform.Rotate(0, 180, 0);
        flipped = true;

        targetLayerName = "Enemy";
    }
}