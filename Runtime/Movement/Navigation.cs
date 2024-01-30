using System;
using UnityEngine;

namespace JvDev.Movement
{
    public abstract class Navigation : MonoBehaviour, IPhysicsBody
    {
        protected Vector3 _velocity;
        public bool useGravity = true;

        public CharacterController Controller;
        [Min(0)] public float movementSpeed = 5;

        [Min(0.1f)] public float mass = 1;

        [Min(0)] public float acceleration = 1;
        [Min(0)] public float deceleration;

        [Min(0)] public float friction;
        [Min(0)] public float drag;

        [Space(3)] public float velocityPower = 1.2f;

        [Space(10), Header("Jump"), Min(0.1f)] public float jumpHeight = 3f;
        [Min(0.1f)] public float timeToJumpApex = 0.5f;
        [Range(0, 1)] public float jumpCut = 0.1f;
        [Range(0, 1)] public float jumpHangThreshold = 0.1f;
        [Range(0, 1)] public float jumpHang = 0.5f;

        [Space(3)] public Vector2 wallJump;
        public Vector2 wallLeap;

        [Space(5)] public float jumpCoyoteTime = 0.2f;
        public float jumpBufferTime = 0.2f;

        [Header("Extra"), Space(5)] public float fallGravity = 20f;
        [Min(0)] public float maxFallSpeed = 50f;
        public float wallSlideSpeedMax = 8f;
        [Range(0, 0.3f)] public float wallStickTime = 0.2f;

        protected float _gravity;
        protected float _jumpSpeed;
        protected float _timeToWallUnstick;

        protected Vector3 _movementInput;

        protected float _lastGroundedTime;
        protected float _lastJumpTime;

        private Vector3 _facingDirection = Vector3.right;

        public bool IsOnGround => (Controller.collisionFlags & CollisionFlags.Below) != 0;
        public bool IsOnCoyoteTime => _lastGroundedTime > 0;
        public bool IsOnWall => (Controller.collisionFlags & CollisionFlags.Sides) != 0;
        public bool IsJumping { get; protected set; }
        public bool IsWallSliding { get; protected set; }
        public bool IsActive { get; set; }
        public bool IsFalling => _velocity.y < 0;

        public Vector3 FacingDirection => _facingDirection;
        public Vector3 Velocity => _velocity;
        public float HorizontalVelocity => _velocity.x;
        public float VerticalVelocity => _velocity.y;
        public float Gravity => _gravity;

        public event Action TouchWall;
        public event Action TouchGround;

        protected virtual void OnEnable() => OnValidate();

        protected void OnValidate()
        {
            _gravity = (2 * jumpHeight) / (timeToJumpApex * timeToJumpApex);
            _jumpSpeed = _gravity * timeToJumpApex;
        }

        public virtual void Step()
        {
            SetFaceDirection();
            Move();
            ResetState();
        }

        private void SetFaceDirection()
        {
            if (_movementInput.x != 0)
                _facingDirection.x = Mathf.Sign(_movementInput.x);
        }

        private void Move()
        {
            _velocity.z = -transform.position.z;
            if (Controller.enabled && gameObject.activeSelf)
            {
                Controller.Move(_velocity * Time.deltaTime);
            }
        }

        public abstract void AddForce(Vector3 force, ForceMode mode = 0);

        public void ClearVelocity()
        {
            _velocity *= 0;
        }

        public void ClearVerticalVelocity()
        {
            _velocity.y *= 0;
        }

        public void ClearHorizontalVelocity()
        {
            _velocity.x *= 0;
        }

        public abstract void SetMovementInput(Vector2 input);
        public abstract void UpdateVelocity();
        public abstract void CalculateDisplacement();
        protected abstract void Jump();
        public void OnJumpStart() => _lastJumpTime = jumpBufferTime;

        public void OnJumpEnd()
        {
            if (_velocity.y > 0 && IsJumping)
            {
                _velocity.y -= _velocity.y * (1 - jumpCut);
            }

            IsJumping = false;
            _lastJumpTime = 0;
        }

        protected void CheckJump()
        {
            if (_lastGroundedTime > 0 && _lastJumpTime > 0 && !IsJumping)
                Jump();
        }

        public void ResetState()
        {
            if ((Controller.collisionFlags & CollisionFlags.Above) != 0)
            {
                _velocity.y = _velocity.y > 0 ? 0 : _velocity.y;
            }

            if ((Controller.collisionFlags & CollisionFlags.Below) != 0)
            {
                _velocity.y = -1;
            }

            IsJumping = !IsOnGround && IsJumping; // && !IsWallSliding;

            if (IsOnGround || IsWallSliding)
                _lastGroundedTime = jumpCoyoteTime;
            else
                _lastGroundedTime -= Time.deltaTime;

            _lastJumpTime -= Time.deltaTime;
        }

        protected void OnOnTouchGround()
        {
            TouchGround?.Invoke();
        }

        protected void OnTouchWall()
        {
            TouchWall?.Invoke();
        }
    }
}