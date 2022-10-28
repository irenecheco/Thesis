using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

namespace VRatPolito.PrattiToolkit.VR
{
    public class JoystickMoveProvider : ContinuousMoveProviderBase
    {
        [SerializeField] private float _runSpeed;

        [SerializeField] protected bool _useControllerAsForward;
        [Tooltip("Unlock Z axis movement")]
        [SerializeField] protected bool _flyMode = false;

        protected Transform _rightHand, _leftHand;
        protected bool _isRightHand, _isLeftHand;

        [SerializeField]
        [Tooltip("The Input System Action that will be used to read Move data from the left hand controller. Must be a Value Vector2 Control.")]
        InputActionProperty m_LeftHandMoveAction;

        /// <summary>
        /// The Input System Action that Unity uses to read Move data from the left hand controller. Must be a <see cref="InputActionType.Value"/> <see cref="Vector2Control"/> Control.
        /// </summary>
        public InputActionProperty leftHandMoveAction
        {
            get => m_LeftHandMoveAction;
            set => SetInputActionProperty(ref m_LeftHandMoveAction, value);
        }

        
        [SerializeField]
        [Tooltip("The Input System Action that will be used to read Move data from the right hand controller. Must be a Value Vector2 Control.")]
        InputActionProperty m_RightHandMoveAction;
        /// <summary>
        /// The Input System Action that Unity uses to read Move data from the right hand controller. Must be a <see cref="InputActionType.Value"/> <see cref="Vector2Control"/> Control.
        /// </summary>
        public InputActionProperty rightHandMoveAction
        {
            get => m_RightHandMoveAction;
            set => SetInputActionProperty(ref m_RightHandMoveAction, value);
        }

        [SerializeField]
        InputActionProperty m_LeftHandRunAction;
        [SerializeField]
        InputActionProperty m_RightHandRunAction;

        private float _cachedMoveSpeed;
        private Vector3 _inputMove;


        /// <summary>
        /// See <see cref="MonoBehaviour"/>.
        /// </summary>
        protected void OnEnable()
        {
            m_LeftHandMoveAction.EnableDirectAction();
            m_RightHandMoveAction.EnableDirectAction();
        }

        /// <summary>
        /// See <see cref="MonoBehaviour"/>.
        /// </summary>
        protected void OnDisable()
        {
            m_LeftHandMoveAction.DisableDirectAction();
            m_RightHandMoveAction.DisableDirectAction();
        }

        protected void Awake()
        {
            base.Awake();
            _cachedMoveSpeed = moveSpeed;
            _leftHand = system.xrOrigin?.transform.GetChildRecursive("LeftHand Controller");
            _rightHand = system.xrOrigin?.transform.GetChildRecursive("RightHand Controller");
        }

        protected void Update()
        {
            AssignForward();
            base.Update();
        }

        /// <inheritdoc />
        protected override Vector2 ReadInput()
        {
            var leftHandValue = m_LeftHandMoveAction.action?.ReadValue<Vector2>() ?? Vector2.zero;
            var rightHandValue = m_RightHandMoveAction.action?.ReadValue<Vector2>() ?? Vector2.zero;

            _isLeftHand = leftHandValue.magnitude > 0;
            _isRightHand = rightHandValue.magnitude > 0;

            moveSpeed = _cachedMoveSpeed;
            if ((_isLeftHand && (bool)m_LeftHandRunAction.action?.IsPressed()) || (_isRightHand && (bool)m_RightHandRunAction.action?.IsPressed()))
            {
                moveSpeed = _runSpeed;
            }


            return leftHandValue + rightHandValue;
        }

        protected override Vector3 ComputeDesiredMove(Vector2 input)
        {
            if (input == Vector2.zero)
                return Vector3.zero;

            if (system.xrOrigin == null)
                return Vector3.zero;

            // Assumes that the input axes are in the range [-1, 1].
            // Clamps the magnitude of the input direction to prevent faster speed when moving diagonally,
            // while still allowing for analog input to move slower (which would be lost if simply normalizing).
            _inputMove = Vector3.ClampMagnitude(new Vector3(enableStrafe ? input.x : 0f, 0f, input.y), 1f);


            // Determine frame of reference for what the input direction is relative to
            var forwardSourceTransform = forwardSource == null ? system.xrOrigin?.Camera.transform : forwardSource;
            var inputForwardInWorldSpace = forwardSourceTransform.forward;
            if (!_flyMode && Mathf.Approximately(Mathf.Abs(Vector3.Dot(inputForwardInWorldSpace, system.xrOrigin.Origin.transform.up)), 1f))
            {
                // When the input forward direction is parallel with the rig normal,
                // it will probably feel better for the player to move along the same direction
                // as if they tilted forward or up some rather than moving in the rig forward direction.
                // It also will probably be a better experience to at least move in a direction
                // rather than stopping if the head/controller is oriented such that it is perpendicular with the rig.
                inputForwardInWorldSpace = -forwardSourceTransform.up;
            }

            if(!_flyMode)
                inputForwardInWorldSpace = Vector3.ProjectOnPlane(inputForwardInWorldSpace, system.xrOrigin.Origin.transform.up);

            return system.xrOrigin.Origin.transform.TransformDirection(
                Quaternion.FromToRotation(system.xrOrigin.Origin.transform.forward, inputForwardInWorldSpace)
                * _inputMove * (moveSpeed * Time.deltaTime));
        }

        void SetInputActionProperty(ref InputActionProperty property, InputActionProperty value)
        {
            if (Application.isPlaying)
                property.DisableDirectAction();

            property = value;

            if (Application.isPlaying && isActiveAndEnabled)
                property.EnableDirectAction();
        }

 

        private void AssignForward()
        {

            if (!_useControllerAsForward) return;
            forwardSource = null;

            if (_isLeftHand)
                forwardSource = _leftHand;
            else if (_isRightHand)
                forwardSource = _rightHand;
        }

    }
}
