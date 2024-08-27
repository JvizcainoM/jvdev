using UnityEngine;

namespace JvDev.UnityUtils.RayCasting
{
    public class RaycastSensor
    {
        private RaycastSensor(Transform t)
        {
            _transform = t;
        }

        public enum CastDirection
        {
            Forward,
            Right,
            Up,
            Backward,
            Left,
            Down
        }

        private readonly Transform _transform;
        private Vector3 _origin = Vector3.zero;
        private CastDirection _castDirection;
        private RaycastHit _hitInfo;

        public float CastLength { get; set; }
        public LayerMask LayerMask { get; set; }
        public QueryTriggerInteraction TriggerInteraction { get; set; }
        public bool HasDetectedHit { get; private set; }

        public void Cast()
        {
            var worldOrigin = _transform.TransformPoint(_origin);
            var worldDirection = GetCastDirection();

            HasDetectedHit = Physics.Raycast(worldOrigin, worldDirection, out _hitInfo, CastLength, LayerMask,
                TriggerInteraction);
        }

        public void SetCastOrigin(Vector3 point) => _origin = _transform.InverseTransformPoint(point);
        public void SetCastDirection(CastDirection direction) => _castDirection = direction;
        public float GetDistance() => _hitInfo.distance;
        public Vector3 GetNormal() => _hitInfo.normal;
        public Vector3 GetPosition() => _hitInfo.point;
        public Collider GetCollider() => _hitInfo.collider;
        public Transform GetTransform() => _hitInfo.transform;

        private Vector3 GetCastDirection()
        {
            return _castDirection switch
            {
                CastDirection.Forward => _transform.forward,
                CastDirection.Right => _transform.right,
                CastDirection.Up => _transform.up,
                CastDirection.Backward => -_transform.forward,
                CastDirection.Left => -_transform.right,
                CastDirection.Down => -_transform.up,
                _ => Vector3.one
            };
        }

        public void DrawDebug()
        {
            if (!HasDetectedHit) return;

            Debug.DrawRay(_hitInfo.point, _hitInfo.normal, Color.red, Time.deltaTime);
            const float markerSize = 0.2f;
            Debug.DrawLine(_hitInfo.point + Vector3.up * markerSize, _hitInfo.point - Vector3.up * markerSize,
                Color.green, Time.deltaTime);
            Debug.DrawLine(_hitInfo.point + Vector3.right * markerSize, _hitInfo.point - Vector3.right * markerSize,
                Color.green, Time.deltaTime);
            Debug.DrawLine(_hitInfo.point + Vector3.forward * markerSize,
                _hitInfo.point - Vector3.forward * markerSize, Color.green, Time.deltaTime);
        }

        public class Builder
        {
            private readonly Transform _transform;
            private float _castLength = 1f;
            private LayerMask _layerMask = 255;
            private QueryTriggerInteraction _triggerInteraction = QueryTriggerInteraction.Ignore;

            public Builder(Transform transform)
            {
                _transform = transform;
            }

            public Builder SetCastLength(float length)
            {
                _castLength = length;
                return this;
            }

            public Builder SetLayerMask(LayerMask mask)
            {
                _layerMask = mask;
                return this;
            }

            public Builder SetTriggerInteraction(QueryTriggerInteraction interaction)
            {
                _triggerInteraction = interaction;
                return this;
            }

            public RaycastSensor Build()
            {
                var sensor = new RaycastSensor(_transform)
                {
                    CastLength = _castLength,
                    LayerMask = _layerMask,
                    TriggerInteraction = _triggerInteraction
                };

                return sensor;
            }
        }
    }
}