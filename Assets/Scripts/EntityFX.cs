using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    [SerializeField] public float flashDuration;
    [SerializeField] public Material hitMat;

    private SpriteRenderer sp;
    private Material orginMat;

    private void Start()
    {
        sp = GetComponentInChildren<SpriteRenderer>();
        orginMat = sp.material;
    }

    public IEnumerator FlashFX()
    {
        sp.material = hitMat;
        yield return new WaitForSeconds(flashDuration);
        sp.material = orginMat;
    }

    public void RedWhiteBlink()
    {
        if (sp.color != Color.red)
            sp.color = Color.red;
        else
            sp.color = Color.white;
    }

    public void CancelBlink()
    {
        CancelInvoke();
        sp.color = Color.white;
    }
}
