using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 public class PlayerInputController : MonoBehaviour
    {
        public float rotationSpeed = 3F;

        private Vector2 firstPressPosition;
        private Vector2 secondPressPosition;
        private Vector2 dragVector;
        private Vector3 tempPosition;
        private bool playing;
        private bool rotating;

        [HideInInspector] public bool allowInput = true;

        [SerializeField] private Transform platformTr;
        
        // Properties
        private static Vector3 MousePosition => Input.mousePosition;

        #region MonoBehaviour functions

        private void Start()
        {
            SetPlaying(true);
        }
        

        public void LevelStarted()
        {
            SetPlaying(true);
        }


        public void LevelCompleted()
        {
            SetPlaying(false);
        }


        public void SetPlaying(bool value)
        {
            playing = value;
        }


        private void Update()
        {
            if (!allowInput)
            {
                return;
            }

            HandleInput();
        }

        #endregion


        #region Input handling

        private void HandleInput()
        {
#if UNITY_EDITOR
            if (Input.GetMouseButton(0))
            {
                var yAngle = Input.GetAxis("Mouse X") * -rotationSpeed * Time.deltaTime;
                UpdateTransform(yAngle);
            }
#endif

#if UNITY_IOS || UNITY_ANDROID
            if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                var yAngle = Input.GetTouch(0).deltaPosition * rotationSpeed * Time.deltaTime;
                transform.Rotate(0f, yAngle, 0f);
            }
#endif
        } //FIXME burayi daha universal yapabilirsin
        
        private void UpdateTransform(float yAngle)
        {
            platformTr.Rotate(0f, yAngle, 0f);
        }

        #endregion
    }