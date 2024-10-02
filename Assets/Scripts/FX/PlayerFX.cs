using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerFX : EntityFX
{
    [Header("Screen Impluse")]
    private CinemachineImpulseSource impluseSource;
    [SerializeField] private Vector3 implusePower;
    [SerializeField] private float impluseMultiplier;

    protected override void Start()
    {
        base.Start();
        impluseSource = GetComponent<CinemachineImpulseSource>();
    }

    public void ScreenImpluse()
    {
        impluseSource.GenerateImpulse(new Vector3(-entity.facingDir * implusePower.x, implusePower.y) * impluseMultiplier);
    }
}
