using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
   [SerializeField] private Transform target;
   [SerializeField] private float speed = 0.5f;

   private void LateUpdate()
   {
       
       Vector3 newPosition = Vector3.Lerp(transform.position, target.position, speed * Time.deltaTime);
       newPosition.z = transform.position.z;
       transform.position = newPosition;
   }
}