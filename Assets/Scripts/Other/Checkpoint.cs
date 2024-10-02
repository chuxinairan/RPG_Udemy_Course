using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private Animator anim;
    public bool activated;
    public string id;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    [ContextMenu("Generate checkpoint id")]
    public void GenerateID()
    {
        id = System.Guid.NewGuid().ToString();
    }

    public void ActivateCheckpoint()
    {
        if (activated == true)
            return;

        AudioManager.instance.PlaySFX(5, null);
        activated = true;
        anim.SetTrigger("active");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
            ActivateCheckpoint();
    }
}
