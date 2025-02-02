using Zenject;
using UnityEngine;

namespace _Project.Scripts.Player
{
    public class InputService : ITickable
    {
        public Vector3 input { get; private set; }
        public Vector2 mouseDelta { get; private set; }
        public Vector2 _smoothMouse { get; private set; }
        public bool wantingToSprint { get; private set; }
        public bool wantingToCrouch { get; private set; }
        public bool wantingToJump { get; private set; }

        public bool IsStop { get; set; } = false;

        private bool _lockCursor;
        private InputModel _model;

        public InputService(InputModel model) =>
            (_model) = (model);

        public void Tick()
        {
            input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

            wantingToJump = Input.GetKey(_model.jump);
            wantingToCrouch = Input.GetKey(_model.crouch);
            wantingToSprint = Input.GetKey(_model.sprint);

            RotateCamera();
            LockCursor();
        }

        private void RotateCamera()
        {
            mouseDelta = Vector2.Scale(
                new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")),
                new Vector2(_model.sensitivity.x * _model.smoothing.x, _model.sensitivity.y * _model.smoothing.y));

            var x_smoo = Mathf.Lerp(_smoothMouse.x, mouseDelta.x, 1f / _model.smoothing.x);
            var y_smoo = Mathf.Lerp(_smoothMouse.y, mouseDelta.y, 1f / _model.smoothing.y);
            _smoothMouse = new(x_smoo, y_smoo);
        }

        private void LockCursor()
        {
            if (Input.GetKeyDown(_model.lockToggle))
            {
                _lockCursor = !_lockCursor;

                if (_lockCursor)
                    Cursor.lockState = CursorLockMode.Locked;
                else
                    Cursor.lockState = CursorLockMode.None;
            }
        }
    }
}