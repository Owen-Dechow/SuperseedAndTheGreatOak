using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : SpriteController
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float timeFalling;
    [SerializeField] private float timeJumpBtn;
    [SerializeField] private float jumpTime;

    [SerializeField] public PowerUps powerUps;

    private void Update()
    {
        CollisionInfo collisionInfo = ApplyPhysics();

        float x = Input.GetAxis("Horizontal");
        ChangeXVelocity(x * moveSpeed * Time.deltaTime);

        if (collisionInfo.Bottom) timeFalling = 0;
        else timeFalling += Time.deltaTime;

        if (collisionInfo.Top)
            SetYVelocity(physicsSpecs.Gravity / 10);

        float maxJumpTime = powerUps.highJump ? powerUps.highJumpTime : jumpTime;
        if (Input.GetAxisRaw("Vertical") >= 1)
        {
            timeJumpBtn += Time.deltaTime;

            if (timeFalling <= maxJumpTime && timeFalling >= 0.1f)
            {
                SetYVelocity(jumpForce);
            }
            else if (timeFalling < 0.1f && timeJumpBtn <= Time.deltaTime)
            {
                SetYVelocity(jumpForce);
            }
        }
        else
        {
            timeJumpBtn = 0;
            if (timeFalling > maxJumpTime)
                timeFalling = powerUps.highJumpTime + 1;
        }
    }

    [Serializable]
    public class PowerUps
    {
        public bool highJump;
        public float highJumpTime;
    }
}