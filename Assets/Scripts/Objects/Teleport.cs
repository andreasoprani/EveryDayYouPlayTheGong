﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Teleport Destination;
    public Vector3 Offset;

    [Header("Fade")]
    public float TimeIn = 0.4f;
    public float Pause = 0.3f;
    public float TimeOut = 0.5f;
    
    
    private bool _teleportActive = true;
    private bool _needDeactivation;

    private void Start()
    {
        _needDeactivation = !(Offset.sqrMagnitude > 0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_teleportActive && other.CompareTag("Player"))
        {
            StartCoroutine(ApplyTeleport(other));
            if(_needDeactivation)
                Destination.StopTeleport();
            
            //other.gameObject.transform.position = Destination.transform.position + Offset;
            //GameObject.FindGameObjectWithTag("MainCamera").SendMessage("TeleportCamera");
            
        }
    }
    
    /// <summary>
    /// Deactivate the teleport
    /// </summary>
    private void StopTeleport()
    {
        _teleportActive = false;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        _teleportActive = true;
    }

    private IEnumerator ApplyTeleport(Collider2D other)
    {
        //TODO Stop player movement
        yield return StartCoroutine(CameraFade.Instance.FadeTo(TimeIn, 1f));
        

        other.gameObject.transform.position = Destination.transform.position + Offset;
        GameObject.FindGameObjectWithTag("MainCamera").SendMessage("TeleportCamera");
        
        yield return new WaitForSeconds(Pause);
        
        yield return StartCoroutine(CameraFade.Instance.FadeTo(TimeOut, 0f));
        
        //TODO Restart player movement
    }
}
