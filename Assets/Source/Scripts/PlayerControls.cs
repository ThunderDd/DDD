﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public float movementSpeed = 5f;
    public GameObject fireball;
    public LayerMask rayCastHitLayers;
    public LayerMask selectableEntities; // Entities that can be selected with mouse click.

    public KeyCode[] keys;
    private KeyBindings keyBindings;

    // For demo ONLY
    private float fireballCooldown = 2f;
    private float fireballCurrentCooldown = 0;
    private bool fireballCasted = false;

    void Awake()
    {
        keyBindings = new KeyBindings(
            new Action[] // KeyboardBindings
            {
                ()=> {
                    if (! fireballCasted)
                        CastFireball();
                },
                () => Debug.Log("Wassup"),
                // Click Bindings
                () => {
                    var playerSelected = GetPlayerAtMousePosition();
                    if (playerSelected.activeSelf)
                        Debug.Log(playerSelected.name);
                    else
                        Destroy(playerSelected);
                },
                () => Debug.Log("Left Click"),
                () => Debug.Log("Middle Click")
            }, keys
        );
    }
    
    void Update()
    {
        keyBindings.CallBindings();

        if (fireballCurrentCooldown >= fireballCooldown)
        {
            fireballCasted = false;
            fireballCurrentCooldown = 0f;
        }
        else
            fireballCurrentCooldown += Time.deltaTime;
    }
        
    private Vector3 GetMouseDirection() =>
        (GetMousePositionOn2DPlane() - transform.position).normalized;

    private Vector3 GetMousePositionOn2DPlane()
    {
        var position = Vector3.zero;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, rayCastHitLayers))
            position = hit.point;

        return position;
    }

    private GameObject GetPlayerAtMousePosition()
    {
        // Create disabled GO.
        GameObject GO = new GameObject();
        GO.SetActive(false);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Fill GO with hit value.
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, rayCastHitLayers))
            GO = hit.collider.gameObject;

        // In function call always check:
        // if (GO.activeSelf) Do stuff;
        return GO;
    }

    private void Move(Vector3 direction) =>
        transform.Translate(direction * Time.deltaTime * movementSpeed);

    private void CastFireball()
    {
        // OffSet needed to create the fireball out of the playerObject,
        // so that it doesn't collide with it right away.
        var offSet = 2f;

        var fireballObject = Instantiate(
            fireball,
            transform.position + offSet * GetMouseDirection(),
            Quaternion.identity
        );

        fireballObject
            .GetComponent<MoveTowardsDirection>()
            .direction = GetMouseDirection();

        fireballCasted = true;
    }
}
