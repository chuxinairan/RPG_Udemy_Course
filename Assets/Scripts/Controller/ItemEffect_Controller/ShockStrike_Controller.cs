using UnityEngine;

public class ShockStrike_Controller : MonoBehaviour
{
    [SerializeField] public float speed;

    private CharacterStats targetStats;
    private int damage;

    private bool triggered = false;
    private Animator anim;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    public void Setup(int _damage, CharacterStats _targetStats)
    {
        damage = _damage;
        targetStats = _targetStats;
    }

    // Update is called once per frame
    void Update()
    {
        if (triggered)
            return;

        transform.position = Vector3.MoveTowards(transform.position, targetStats.transform.position, speed * Time.deltaTime);
        transform.up = targetStats.transform.position - transform.position;

        if(Vector2.Distance(transform.position, targetStats.transform.position) < 0.2f)
        {
            anim.transform.localPosition = new Vector3(0, 0.5f);
            transform.localRotation = Quaternion.identity;
            transform.localScale = new Vector3(3, 3);

            Invoke("DamageAndDestroy", .2f);
            triggered = true;
            anim.SetBool("Hit", true);
        }
    }

    private void DamageAndDestroy()
    {
        targetStats.ApplyShock();
        targetStats.TakeDamage(damage);
        Destroy(gameObject, .3f);
    }
}
