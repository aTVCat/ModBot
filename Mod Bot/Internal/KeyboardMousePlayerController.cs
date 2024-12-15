using Autohand;
using ModLibrary.Internal;
using UnityEngine;
using VR.PowerGloves;

namespace InternalModBot
{
    public class KeyboardMousePlayerController : MonoBehaviour
    {
        public Vector3 MinHandRBounds = new Vector3(-1.2f, 1f, 0f);

        public Vector3 MaxHandRBounds = new Vector3(1.2f, 2.2f, 1f);

        private VRPlayerCharacter _character;

        private float _timeToPokeHeadCamera;

        private void Awake()
        {
            _timeToPokeHeadCamera = Time.time + 0.5f;

            _character = base.GetComponent<VRPlayerCharacter>();
            _character.HandCanvasPointerLeft.enabled = false;
            _character.HandCanvasPointerRight.gameObject.AddComponent<HandCanvasPointerClicker>();
            _character.PlayerCotnrollerLink.enabled = false;
            _character.AutoHandPlayer.snapTurning = false;
        }

        private void Update()
        {
            if (_timeToPokeHeadCamera > 0f && Time.time > _timeToPokeHeadCamera)
            {
                _character.AutoHandPlayer.headCamera.transform.position += new Vector3(0f, 0.01f, 0f);
                _timeToPokeHeadCamera = -1f;
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                VRPauseMenu pauseMenu =FindObjectOfType<VRPauseMenu>();
                if (pauseMenu)
                {
                    pauseMenu.TogglePauseMenu();
                }
            }

            if (Singleton<VRInputManager>.Instance.IsThumbstickMovementEnabled())
            {
                Vector3 point = new Vector3(0f, 0f, 0f);
                if (Input.GetKey(KeyCode.W))
                {
                    point.z = 1f;
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    point.z = -1f;
                }

                if (Input.GetKey(KeyCode.A))
                {
                    point.x = -1f;
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    point.x = 1f;
                }

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    _character.TryJump();
                }
                point.Normalize();
                _character.AutoHandPlayer.AddMove(_character.AutoHandPlayer.forwardFollow.rotation * point * _character.AutoHandPlayer.moveSpeed * Time.deltaTime, false);
            }
            _character.HandRTarget.Rotate(new Vector3(Input.mouseScrollDelta.y * 550f * Time.unscaledDeltaTime, 0f, 0f), Space.Self);

            if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.LeftArrow))
            {
                _character.AutoHandPlayer.Turn(-0.7f, false);
            }
            else if (Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.RightArrow))
            {
                _character.AutoHandPlayer.Turn(0.7f, false);
            }
            else
            {
                _character.AutoHandPlayer.Turn(0f, false);
            }

            if (Input.GetKey(KeyCode.DownArrow))
            {
                _character.HeadTransform.Rotate(60f * Time.unscaledDeltaTime, 0f, 0f);
            }
            else if (Input.GetKey(KeyCode.UpArrow))
            {
                _character.HeadTransform.Rotate(-60f * Time.unscaledDeltaTime, 0f, 0f);
            }

            if (_character.HasRightHand())
            {
                if (Input.GetKeyDown(KeyCode.F) && _character.IsAlive())
                {
                    if (_character.HandR.GetHeld() != null)
                    {
                        _character.HandR.ForceReleaseGrab();
                    }
                    else
                    {
                        _character.HandR.Grab(GrabType.GrabbableToHand);
                    }
                }
                float axis = Input.GetAxis("Mouse X");
                float axis2 = Input.GetAxis("Mouse Y");
                float num = 0.1f;
                Vector3 b = (_character.HeadTransform.right * (axis * num)) + (Vector3.up * (axis2 * num));
                _character.HandRTarget.position += b;
                _character.HandRTarget.localPosition = new Vector3(Mathf.Clamp(_character.HandRTarget.localPosition.x, MinHandRBounds.x, MaxHandRBounds.x), Mathf.Clamp(_character.HandRTarget.localPosition.y, MinHandRBounds.y, MaxHandRBounds.y), Mathf.Clamp(_character.HandRTarget.localPosition.z, MinHandRBounds.z, MaxHandRBounds.z));
                if (Input.GetKeyDown(KeyCode.Alpha3) && _character.IsAlive())
                {
                    PowerGlove equippedPowerGlove = _character.GetEquippedPowerGlove(false);
                    if (equippedPowerGlove)
                    {
                        equippedPowerGlove.OnButtonDown();
                    }
                }
                if (Input.GetKeyUp(KeyCode.Alpha3) && _character.IsAlive())
                {
                    PowerGlove equippedPowerGlove2 = _character.GetEquippedPowerGlove(false);
                    if (equippedPowerGlove2)
                    {
                        equippedPowerGlove2.OnButtonUp();
                    }
                }
            }

            if (_character.HasLeftHand())
            {
                Vector3 zero = Vector3.zero;
                if (Input.GetKey(KeyCode.Keypad6))
                {
                    zero.x = 1f;
                }
                else if (Input.GetKey(KeyCode.Keypad4))
                {
                    zero.x = -1f;
                }
                if (Input.GetKey(KeyCode.Keypad8))
                {
                    zero.y = 1f;
                }
                else if (Input.GetKey(KeyCode.Keypad2))
                {
                    zero.y = -1f;
                }
                else if (Input.GetKey(KeyCode.Keypad9))
                {
                    zero.z = 1f;
                }
                else if (Input.GetKey(KeyCode.Keypad3))
                {
                    zero.z = -1f;
                }

                float num2 = 1f;
                _character.HandLTarget.position += _character.HeadTransform.right * (zero.x * Time.deltaTime * num2);
                _character.HandLTarget.position += Vector3.up * (zero.y * Time.deltaTime * num2);
                _character.HandLTarget.position += _character.HeadTransform.forward * (zero.z * Time.deltaTime * num2);
                _character.HandLTarget.localPosition = new Vector3(Mathf.Clamp(_character.HandLTarget.localPosition.x, MinHandRBounds.x, MaxHandRBounds.x), Mathf.Clamp(_character.HandLTarget.localPosition.y, MinHandRBounds.y, MaxHandRBounds.y), Mathf.Clamp(_character.HandLTarget.localPosition.z, MinHandRBounds.z, MaxHandRBounds.z));
                if (Input.GetKeyDown(KeyCode.KeypadEnter) && _character.IsAlive())
                {
                    if (_character.HandL.GetHeld() != null)
                    {
                        _character.HandL.ForceReleaseGrab();
                        return;
                    }
                    _character.HandL.Grab(GrabType.GrabbableToHand);
                }
                if (Input.GetKeyDown(KeyCode.Alpha1) && _character.IsAlive())
                {
                    PowerGlove equippedPowerGlove3 = _character.GetEquippedPowerGlove(true);
                    if (equippedPowerGlove3)
                    {
                        equippedPowerGlove3.OnButtonDown();
                    }
                }
                if (Input.GetKeyUp(KeyCode.Alpha1) && _character.IsAlive())
                {
                    PowerGlove equippedPowerGlove4 = _character.GetEquippedPowerGlove(true);
                    if (equippedPowerGlove4)
                    {
                        equippedPowerGlove4.OnButtonUp();
                    }
                }
            }
        }
    }
}