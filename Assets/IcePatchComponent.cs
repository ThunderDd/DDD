﻿using Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcePatchComponent : CollisionMonoBehaviour
{
    private float currentLifeTime;
    public float maxLifeTime;
    public float slowValue;
    private MonoBehaviour IcePatchManager;
    public List<Collider> playersInContact;
    private int counter = 1;
    private void Start()
    {
        //IcePatchManager = Get
    }
    void Update()
    {
        if (currentLifeTime >= maxLifeTime)
        {
            foreach (var c in playersInContact)
                Effect(false, c);
            Destroy(gameObject);
        }
        currentLifeTime += Time.deltaTime;
    }
    private void OnTriggerExit(Collider other)
    {
        if (CollidesWithAppropriateLayer(other.gameObject.layer, collisionLayers) && playersInContact.Contains(other))
        {
            playersInContact.Remove(other);
            Effect(false, other);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (CollidesWithAppropriateLayer(other.gameObject.layer, collisionLayers) && !playersInContact.Contains(other))
        {
            Debug.Log(playersInContact.Contains(other));
            ++counter;
            Debug.Log(other.gameObject);
            playersInContact.Add(other);
            Effect(true, other);
        }
    }
    private void Effect(bool activate, Collider other)
    {
        if(activate)
            other.gameObject.GetComponentInParent<EffectHandlerComponent>().ApplyEffect((int)ModifiableStats.Speed, slowValue);
        else
            other.gameObject.GetComponentInParent<EffectHandlerComponent>().EndEffect((int)ModifiableStats.Speed, slowValue);
    }
}
