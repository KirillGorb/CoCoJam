using System;
using UnityEngine;

namespace CodeScripts.PlayerMove.Config.Data
{
    [Serializable]
    public class ConfigMoveY
    {
        public float timeJump;
        public float velocityJump;

        public float poolTimeClick;
        public int countDopJump;

        [Header("Check Ground")] 
        public LayerMask groundLayerMask;
        public float groundCheckRadius;
        public Vector2 groundCheckOffset;
        public float gravityScale;
        public float slopeCheckOffset;
        public float groundExitCooldown = 0.1f;
        public float timeOnDownMove = 0.04f;

        [Header("Auto Up")] 
        public float ledgeHeight = 0.2f;
        public float ledgeCheckDistance = 0.5f;
        public float upOffsetHeight;
    }
}