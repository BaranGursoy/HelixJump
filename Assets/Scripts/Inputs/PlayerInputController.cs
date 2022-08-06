using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 public class PlayerInputController : MonoBehaviour
    {
        [SerializeField] private float rotationSpeed = 1.5F;
        private bool playing;
        private bool rotating;

        private Vector2 lastMousePos;

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
            if (Input.GetMouseButton(0))
            {
                Vector2 currentMousePos = MousePosition;

                if (lastMousePos == Vector2.zero)
                {
                    lastMousePos = currentMousePos;
                }

                var deltaMouseX = lastMousePos.x - currentMousePos.x;
                lastMousePos = currentMousePos;
                
                UpdateTransform(deltaMouseX);
            }

            if (Input.GetMouseButtonUp(0))
            {
                lastMousePos = Vector2.zero;
            }
            
        } //FIXME burayi daha universal yapabilirsin
        
        private void UpdateTransform(float deltaMouseX)
        {
            platformTr.Rotate(Vector3.up * (deltaMouseX * rotationSpeed * Time.deltaTime));
        }

        #endregion
    }