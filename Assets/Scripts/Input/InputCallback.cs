using UniRx;
using UnityEngine;

namespace CodeScripts.PlayerInputs
{
    public class InputCallback : MonoBehaviour
    {
        private PlayerInput _input;

        public static float HorizontalInput { get; private set; }
        public static float DirHorizontalInput { get; private set; }
        public static float RotateInput { get; private set; }
        public static bool JumpInput { get; private set; }
        public static bool SharpDescentInput { get; private set; }
        public static bool PuckUpInput { get; private set; }
        public static bool SedInput { get; private set; }
        
        public readonly static BoolReactiveProperty Rallback = new();
        public readonly static BoolReactiveProperty Skill = new();

        private void Awake()
        {
            _input = new PlayerInput();
        }

        private void OnEnable()
        {
            _input.Enable();
        }

        private void Start()
        {
            _input.Movement.Jump.performed += _ => JumpInput = true;
            _input.Movement.Jump.canceled += _ => JumpInput = false;

            _input.Movement.SharpDescent.performed += _ => SharpDescentInput = true;
            _input.Movement.SharpDescent.canceled += _ => SharpDescentInput = false;

            _input.Movement.PuckUp.performed += _ => PuckUpInput = true;
            _input.Movement.PuckUp.canceled += _ => PuckUpInput = false;

            _input.Movement.PuckUp.performed += _ => SedInput = true;
            _input.Movement.PuckUp.canceled += _ => SedInput = false;

            _input.Movement.Horizontal.performed += e => DirHorizontalInput = HorizontalInput = e.ReadValue<float>();
            _input.Movement.Horizontal.canceled += _ => HorizontalInput = 0;

            _input.Movement.Rotate.performed += e => RotateInput = e.ReadValue<float>();
            _input.Movement.Rotate.canceled += _ => RotateInput = 0;


            _input.Controll.Rallback.performed += _ => Rallback.Value = true;
            _input.Controll.Rallback.canceled += _ => Rallback.Value = false;
        }

        private void OnDisable()
        {
            _input.Disable();
        }
    }
}