using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopupTextFX : MonoBehaviour
{
    private TextMeshPro myText;

    [SerializeField] private float speed;
    [SerializeField] private float disappearSpeed;
    [SerializeField] private float colorDecreaseSpeed;
    [SerializeField] private float lifeTime;

    private float lifeTimer;

    void Start()
    {
        myText = GetComponent<TextMeshPro>();
        lifeTimer = lifeTime;
    }

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, transform.position.y + 1), speed * Time.deltaTime);
        
        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0)
        {
            float newAlpha = myText.alpha - colorDecreaseSpeed * Time.deltaTime;
            myText.alpha = newAlpha;
            if (myText.alpha < 0.16f)
                speed = disappearSpeed;
            if (myText.alpha <= 0)
                Destroy(gameObject);
        }
    }
}
