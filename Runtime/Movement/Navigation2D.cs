using UnityEngine;

namespace JvDev.Movement
{
    public sealed class Navigation2D : Navigation
    {
        private int _wallDirX;
        private byte _rayDirX;
        public bool IsUnstickingFromWall => !IsOnWall && _timeToWallUnstick > 0 && _rayDirX != 0;

        [SerializeField] private LayerMask _wallLayerMask;
        [SerializeField] private LayerMask _groundLayerMask;

        public LayerMask GroundLayerMask => _groundLayerMask;

        public override void Step()
        {
            if (!IsActive) return;
            CalculateDisplacement();
            UpdateVelocity();
            CheckWallRaycast();
            HandleWallSliding();
            CheckJump();

            base.Step();
        }

        public override void SetMovementInput(Vector2 input) => _movementInput.x = input.x;

        private void CheckWallRaycast()
        {
            var t = transform;
            var ray = new Ray(t.position, t.right);
            _rayDirX = 0b00;

            if (Physics.Raycast(ray, out _, 1.5f, _wallLayerMask))
            {
                _rayDirX = 0b01;
            }

            ray.direction = -ray.direction;

            if (Physics.Raycast(ray, out _, 1.5f, _wallLayerMask))
            {
                _rayDirX |= 0b10;
            }
        }

        protected override void Jump()
        {
            var jumpVelocity = _velocity;
            var dirX = (int)Mathf.Sign(_movementInput.x);

            if (IsOnGround)
            {
                jumpVelocity.y = _jumpSpeed;
                _velocity = jumpVelocity;
            }
            else if (IsWallSliding && _rayDirX != 0)
            {
                if (dirX == _wallDirX && _rayDirX != 0)
                {
                    _velocity.x = -_wallDirX * wallJump.x;
                    _velocity.y = wallJump.y;
                }
            }
            else if (IsUnstickingFromWall)
            {
                _velocity.x = _wallDirX * wallLeap.x;
                _velocity.y = wallLeap.y;
            }


            IsJumping = true;
            _lastGroundedTime = 0;
            _lastJumpTime = 0;
            _timeToWallUnstick = 0;
        }

        private void HandleWallSliding()
        {
            // TODO: Check if this is the best way to handle wall sliding, now is causing weird behavior
            IsWallSliding = false;

            if (IsUnstickingFromWall)
            {
                _timeToWallUnstick -= Time.deltaTime;

                if ((int)Mathf.Sign(_movementInput.x) != _wallDirX)
                {
                    _velocity.x = 0;
                }

                if (_velocity.y < -wallSlideSpeedMax && _rayDirX != 0)
                    _velocity.y = -wallSlideSpeedMax;

                _wallDirX = IsOnWall ? (int)Mathf.Sign(_movementInput.x) : _wallDirX;
                return;
            }

            _wallDirX = IsOnWall ? (int)Mathf.Sign(_movementInput.x) : _wallDirX;

            if (!IsOnWall || IsOnGround || !IsFalling)
                return;

            IsWallSliding = true;

            if (_velocity.y < -wallSlideSpeedMax && _rayDirX != 0)
                _velocity.y = -wallSlideSpeedMax;

            _velocity.x = _wallDirX;
            _timeToWallUnstick = wallStickTime;
        }

        public override void CalculateDisplacement()
        {
            var targetSpeed = _movementInput * movementSpeed;
            targetSpeed.y = 0;
            targetSpeed.z = 0;

            var speedDelta = targetSpeed - _velocity;
            speedDelta.y = 0;

            var accelerationRate = (targetSpeed.sqrMagnitude > 0f ? acceleration : deceleration);

            if (IsJumping && Mathf.Abs(_velocity.y) < jumpHangThreshold)
            {
                _velocity.y += _gravity * jumpHang * Time.deltaTime;
                accelerationRate *= 1.4f;
            }

            var movement = Mathf.Pow(speedDelta.magnitude * accelerationRate, velocityPower);
            _velocity += movement * Time.deltaTime * speedDelta.normalized;
        }

        public override void UpdateVelocity()
        {
            if (useGravity)
            {
                if (_velocity.y >= 0)
                    _velocity.y += -_gravity * Time.deltaTime;
                else
                    _velocity.y += -fallGravity * Time.deltaTime;
            }

            _velocity.y = Mathf.Max(_velocity.y, -maxFallSpeed);
            _velocity.z = 0;

            CalculateDrag();
            CalculateFriction();
            ClampVelocity();
        }

        public override void AddForce(Vector3 force, ForceMode mode = 0)
        {
            force.z = 0;
            _velocity += mode switch
            {
                ForceMode.Force => (force / mass) * Time.deltaTime,
                ForceMode.Acceleration => force * Time.deltaTime,
                ForceMode.Impulse => (force / mass),
                ForceMode.VelocityChange => (force - _velocity),
                _ => Vector3.zero
            };
        }

        private void CalculateFriction()
        {
            if (friction <= 0) return;

            var frictionForce = Mathf.Min(Mathf.Abs(_velocity.x), friction);
            var frX = frictionForce * Mathf.Sign(_velocity.x);
            AddForce(new Vector3(frX, 0) * -frictionForce, ForceMode.Impulse);
        }

        private void CalculateDrag()
        {
            if (drag <= 0) return;

            var dragForce = drag * -_velocity.sqrMagnitude;
            AddForce(new Vector3(dragForce, dragForce), ForceMode.Acceleration);
        }

        private void ClampVelocity()
        {
            if (Mathf.Abs(_velocity.x) < .05f)
            {
                _velocity.x = 0;
            }
        }
    }
}