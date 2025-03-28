using UnityEngine;

namespace WinterUniverse
{
    public class CameraManager : BasicComponent
    {
        [SerializeField] private float _followSpeed = 10f;
        [SerializeField] private Vector3 _offset;

        private PlayerController _player;

        public Camera Camera { get; private set; }

        public override void Initialize()
        {
            base.Initialize();
            Camera = GetComponentInChildren<Camera>();
            _player = GameManager.StaticInstance.Player;
        }

        public override void OnLateUpdate()
        {
            base.OnLateUpdate();
            transform.position = Vector3.Lerp(transform.position, _player.transform.position + _offset, _followSpeed * Time.deltaTime);
        }
    }
}