using UnityEngine;

namespace WinterUniverse
{
    public class GameManager : Singleton<GameManager>
    {
        public CameraManager Camera { get; private set; }
        public PlayerController Player { get; private set; }
        public UIManager UI { get; private set; }
        public InputMode InputMode { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            GetComponents();
        }

        private void Start()
        {
            InitializeComponents();
            EnableComponents();
        }

        private void OnDestroy()
        {
            DisableComponents();
            DestroyComponents();
        }

        private void Update()
        {
            UpdateComponents();
        }

        private void FixedUpdate()
        {
            FixedUpdateComponents();
        }

        private void LateUpdate()
        {
            LateUpdateComponents();
        }

        private void GetComponents()
        {
            Camera = FindFirstObjectByType<CameraManager>();
            Player = FindFirstObjectByType<PlayerController>();
            UI = FindFirstObjectByType<UIManager>();
        }

        private void InitializeComponents()
        {
            Camera.Initialize();
            Player.Initialize();
            UI.Initialize();
            ToggleInputMode(InputMode.Game);
        }

        private void DestroyComponents()
        {
            Camera.Destroy();
            Player.Destroy();
            UI.Destroy();
        }

        private void EnableComponents()
        {
            Camera.Enable();
            Player.Enable();
            UI.Enable();
            Player.Status.Revive();// test
        }

        private void DisableComponents()
        {
            Camera.Disable();
            Player.Disable();
            UI.Disable();
        }

        private void UpdateComponents()
        {

        }

        private void FixedUpdateComponents()
        {
            Player.OnFixedUpdate();
        }

        private void LateUpdateComponents()
        {
            Camera.OnLateUpdate();
        }

        public void ToggleInputMode(InputMode mode)
        {
            InputMode = mode;
            if (InputMode == InputMode.Game)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else if (InputMode == InputMode.UI)
            {
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
            }
        }
    }
}