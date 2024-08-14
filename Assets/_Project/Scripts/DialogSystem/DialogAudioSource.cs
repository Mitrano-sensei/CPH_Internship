using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DialogAudioSource : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform target;
    [SerializeField] private float range = 1f;
    
    void Update()
    {
        var pos = target.position - player.position;
        var distance = pos.magnitude;
        pos.Normalize();
        pos *= Mathf.Min(range, distance);
        
        transform.position = player.position + pos;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(player.position, range);
    }
}
