using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float depth = 0.7f;
    
    void Update()
    {
        transform.position = target.position * depth;
    }
}
