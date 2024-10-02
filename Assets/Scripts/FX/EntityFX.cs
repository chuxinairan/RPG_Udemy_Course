using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    [Header("Flash FX")]
    [SerializeField] public float flashDuration;
    [SerializeField] public Material hitMat;

    [Header("Aliment FX")]
    [SerializeField] private Color[] chillColor;
    [SerializeField] private Color[] igniteColor;
    [SerializeField] private Color[] shockColor;

    [Header("Aliment particle")]
    [SerializeField] private ParticleSystem igniteFX;
    [SerializeField] private ParticleSystem chillFX;
    [SerializeField] private ParticleSystem shockFX;

    [Header("Hit FX")]
    [SerializeField] private GameObject hitFX;
    [SerializeField] private GameObject hitCriticalFX;

    [Header("Popup Text FX")]
    [SerializeField] private GameObject popupText;

    private SpriteRenderer sp;
    private Material orginMat;
    protected Entity entity;

    protected virtual void Start()
    {
        entity = GetComponent<Entity>();
        sp = GetComponentInChildren<SpriteRenderer>();
        orginMat = sp.material;
    }
    public void MakeTransparent(bool _transparent)
    {
        if (_transparent)
            sp.color = Color.clear;
        else
            sp.color = Color.white;
    }

    public IEnumerator FlashFX()
    {
        sp.material = hitMat;
        Color currentColor = sp.color;
        sp.color = Color.white;
        yield return new WaitForSeconds(flashDuration);
        sp.color = currentColor;
        sp.material = orginMat;
    }

    public void RedWhiteBlink()
    {
        if (sp.color != Color.red)
            sp.color = Color.red;
        else
            sp.color = Color.white;
    }

    public void CancleColorChange()
    {
        CancelInvoke();
        sp.color = Color.white;
        igniteFX.Stop();
        chillFX.Stop();
        shockFX.Stop();
    }

    public void InvokeChillFor(float _seconds)
    {
        chillFX.Play();
        InvokeRepeating("ChillColorFx", 0, .3f);
        Invoke("CancleColorChange", _seconds);
    }

    public void InvokeIgniteFor(float _seconds)
    {
        igniteFX.Play();
        InvokeRepeating("IgniteColorFx", 0, .3f);
        Invoke("CancleColorChange", _seconds);
    }

    public void InvokeShockFor(float _seconds)
    {
        shockFX.Play();
        InvokeRepeating("ShockColorFx", 0, .3f);
        Invoke("CancleColorChange", _seconds);
    }

    private void ChillColorFx()
    {
        if (sp.color != chillColor[0])
            sp.color = chillColor[0];
        else
            sp.color = chillColor[1];
    }

    private void IgniteColorFx()
    {
        if (sp.color != igniteColor[0])
            sp.color = igniteColor[0];
        else
            sp.color = igniteColor[1];
    }

    private void ShockColorFx()
    {
        if (sp.color != shockColor[0])
            sp.color = shockColor[0];
        else
            sp.color = shockColor[1];
    }

    public void CreateHitFX(Transform _target, bool _isCritical)
    {
        float rotationZ = Random.Range(-90, 90);
        float positionX = Random.Range(-0.5f, 0.5f);
        float positionY = Random.Range(0f, 1f);

        GameObject hitObj = hitFX;
        if (_isCritical)
            hitObj = hitCriticalFX;

        GameObject hit_fx = Instantiate(hitObj, _target.position + new Vector3(positionX, positionY, 0), Quaternion.identity);
        if (_isCritical)
            hit_fx.transform.localScale = new Vector3(entity.facingDir, 1, 1);
        else
            hit_fx.transform.Rotate(new Vector3(0, 0, rotationZ));

        Destroy(hit_fx, 0.5f);
    }

    public void CreatePopupText(Transform _target, string _text, Color _textColor)
    {
        Vector3 offset = new Vector3(Random.Range(-1, 1), Random.Range(0.1f, 0.5f), 0);
        GameObject textObj = Instantiate(popupText, _target.position + offset, Quaternion.identity);

        TextMeshPro textMesh = textObj.GetComponent<TextMeshPro>();
        textMesh.text = _text;
        textMesh.color = _textColor;
    }
}
