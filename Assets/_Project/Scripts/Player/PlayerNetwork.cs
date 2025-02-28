using Unity.Netcode;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{
    private NetworkVariable<PlayerNetworkData> _netState = new(
        writePerm: NetworkVariableWritePermission.Owner,
        readPerm: NetworkVariableReadPermission.Everyone);

    [SerializeField] private float _interpolationSpeed = 10f;
    [SerializeField] private float _positionThreshold = 0.1f;
    [SerializeField] private float _rotationThreshold = 5f;

    private Vector3 _targetPosition;
    private Quaternion _targetRotation;
    private Vector3 _velocity;
    private double _lastNetworkUpdateTime;

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            _targetPosition = transform.position;
            _targetRotation = transform.rotation;
            UpdateNetworkState();
        }
        else
        {
            _netState.OnValueChanged += HandleNetworkStateChanged;
        }
    }

    private void Update()
    {
        if (IsOwner)
        {
            OwnerUpdate();
        }
        else
        {
            ClientUpdate();
        }
    }

    private void OwnerUpdate()
    {
        if (Vector3.Distance(transform.position, _targetPosition) > _positionThreshold ||
            Quaternion.Angle(transform.rotation, _targetRotation) > _rotationThreshold)
        {
            UpdateNetworkState();
        }
    }

    private void UpdateNetworkState()
    {
        _targetPosition = transform.position;
        _targetRotation = transform.rotation;
        
        _netState.Value = new PlayerNetworkData
        {
            Position = _targetPosition,
            Rotation = _targetRotation,
            Timestamp = NetworkManager.Singleton.ServerTime.Time
        };
    }

    private void HandleNetworkStateChanged(PlayerNetworkData previous, PlayerNetworkData current)
    {
        if (IsOwner) return;

        // Calculate velocity based on time between updates
        double timeSinceLastUpdate = current.Timestamp - previous.Timestamp;
        if (timeSinceLastUpdate > 0)
        {
            _velocity = (current.Position - previous.Position) / (float)timeSinceLastUpdate;
        }

        _targetPosition = current.Position + _velocity * (float)(NetworkManager.Singleton.ServerTime.Time - current.Timestamp);
        _targetRotation = current.Rotation;
        _lastNetworkUpdateTime = NetworkManager.Singleton.ServerTime.Time;
    }

    private void ClientUpdate()
    {
        float interpolationFactor = Mathf.Clamp01(Time.deltaTime * _interpolationSpeed);
        transform.position = Vector3.Lerp(transform.position, _targetPosition, interpolationFactor);
        transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation, interpolationFactor);
    }

    struct PlayerNetworkData : INetworkSerializable
    {
        private Vector3 _position;
        private Quaternion _rotation;
        private double _timestamp;

        internal Vector3 Position
        {
            get => _position;
            set => _position = value;
        }

        internal Quaternion Rotation
        {
            get => _rotation;
            set => _rotation = value;
        }

        internal double Timestamp
        {
            get => _timestamp;
            set => _timestamp = value;
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref _position);
            serializer.SerializeValue(ref _rotation);
            serializer.SerializeValue(ref _timestamp);
        }
    }
}