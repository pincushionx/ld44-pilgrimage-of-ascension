using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A simple third person character controller
/// Got a start from https://www.reddit.com/r/Unity3D/comments/53129b/need_help_with_third_person_controller/
/// </summary>
namespace Pincushion.LD44
{
    [RequireComponent(typeof(CharacterController))]
    public class ThirdPersonController : MonoBehaviour
    {
        private CharacterController characterController;
        private Vector3 movement;
        private Vector2 lookDirection;
        private Camera thirdPersonCamera;


        private float baseMovementSpeed = 7.0f;
        private float baseJumpSpeed = 10.0f;
        private float baseFallSpeed = 30.0f;

        private float buffedMovementSpeed;
        private float buffedJumpSpeed;
        private float buffedDoubleJumpSpeed;
        private float buffedFallSpeed;
        private bool canBreakWall = false;
        private bool canDoubleJump = false;

        private void Awake()
        {
            UpdateBuffs();
        }
        private void Start()
        {
            movement = new Vector3();
            characterController = GetComponent<CharacterController>();
            thirdPersonCamera = GetComponentInChildren<Camera>();
            //jumping = false;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = true;
        }

        public void UpdateBuffs()
        {
            buffedMovementSpeed = baseMovementSpeed;
            buffedJumpSpeed = baseJumpSpeed;
            buffedFallSpeed = baseFallSpeed;

            PaperDoll paperDoll = SceneManager.Instance.paperDoll;
            if (paperDoll.leftLegSacrificed)
            {
                buffedMovementSpeed = 20f;
            }
            if (paperDoll.rightLegSacrificed)
            {
                buffedJumpSpeed = 15f;
            }

            if (paperDoll.leftLegSacrificed && paperDoll.rightLegSacrificed)
            {
                // crawling
            }
            else if (paperDoll.leftLegSacrificed || paperDoll.rightLegSacrificed)
            {
                // hopping
            }

            if (paperDoll.leftArmSacrificed)
            {
                canBreakWall = true;
            }
            if (paperDoll.rightArmSacrificed)
            {
                canDoubleJump = true;
                buffedDoubleJumpSpeed = buffedJumpSpeed * 1.5f;
            }
        }

        // Update is called once per frame
        private void Update()
        {
            if (SceneManager.Instance.Paused || SceneManager.Instance.modalMode)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                return;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }

            if (characterController.transform.position.y < -10f)
            {
                // player fell and died
                SceneManager.Instance.PlayerDied();
            }

            UpdateBuffs();


            // wall breaking
            if (Input.GetKeyDown(KeyCode.E))
            {
                BreakWall();
            }


            if (characterController.isGrounded == true)
            {

                movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

                movement = transform.TransformDirection(movement);
                movement *= buffedMovementSpeed;

                if (Input.GetButtonDown("Jump"))
                {
                    movement.y = buffedJumpSpeed;
                }
            }
            else if(canDoubleJump)
            {
                if (Input.GetButtonDown("Jump"))
                {
                    Vector3 direction = movement.normalized;
                    Ray ray = new Ray(gameObject.transform.position, direction);
                    RaycastHit hitInfo = new RaycastHit();

                    bool hit = Physics.Raycast(ray, out hitInfo, 2f, LayerMask.GetMask("InteractableObject"));

                    if (hit)
                    {
                        float dot = Vector3.Dot(hitInfo.normal, -direction);
                        Vector3 reflection = hitInfo.normal * (dot * 2); 
                        reflection = (reflection + direction).normalized;

                        movement.x = reflection.x * buffedMovementSpeed;
                        movement.y = buffedDoubleJumpSpeed;
                        movement.z = reflection.z * buffedMovementSpeed;
                    }
                }
            }

            movement.y -= buffedFallSpeed * Time.deltaTime;

            characterController.Move(movement * Time.deltaTime);

            Vector2 lookInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

            lookDirection += lookInput;
            Quaternion hRotation = Quaternion.Euler(-lookDirection.y, 0, 0.0f);
            Quaternion vRotation = Quaternion.Euler(0, lookDirection.x, 0.0f);

            thirdPersonCamera.transform.localRotation =
                    Quaternion.Slerp(thirdPersonCamera.transform.localRotation, hRotation, 10 * Time.deltaTime);

            gameObject.transform.localRotation =
                    Quaternion.Slerp(gameObject.transform.localRotation, vRotation, 10 * Time.deltaTime);

        }

       /* private bool IsApproachingWall()
        {
            Vector3 direction = movement.normalized;
            Ray ray = new Ray(gameObject.transform.position, direction);
            RaycastHit hitInfo = new RaycastHit();

            bool hit = Physics.Raycast(ray, out hitInfo, 2f, LayerMask.GetMask("InteractableObject"));

            if (hit)
            {
                float dot = Vector3.Dot(hitInfo.normal, -direction);
                dot *= 2;
                Vector3 reflection = hitInfo.normal * dot;
                reflection = reflection + direction;
                reflection.normalized;
            }

            //return Physics.Raycast(gameObject.transform.position, direction, 2f, LayerMask.GetMask("InteractableObject"));



        }*/
        private void BreakWall()
        {
            RaycastHit hitInfo = new RaycastHit();
            
            Ray cameraRay = thirdPersonCamera.ScreenPointToRay(Input.mousePosition);

            bool hit = Physics.Raycast(cameraRay, out hitInfo, 10f, LayerMask.GetMask("InteractableObject")); // 5f distance

            // reset targets
            BreakableWall wall = null;

            if (hit)
            {
                GameObject hitObject = hitInfo.collider.gameObject;
                if ((wall = hitObject.GetComponent<BreakableWall>()) != null)
                {
                    // ensure we didn;'t hit the top of the wall
                    if (hitInfo.normal.y == 0)
                    {
                        if (canBreakWall)
                        {
                            if (wall.breakable)
                            {
                                // break it
                                StartCoroutine(BreakWall(wall, -hitInfo.normal));
                            }
                        }
                        else
                        {
                            // lose blood, because it hurts
                        }
                    }
                }
            }
        }

        public IEnumerator BreakWall(BreakableWall wall, Vector3 direction)
        {
            Vector3 fromPos = wall.transform.position;
            Vector3 toPos = fromPos + direction * 10f;

            float elapsedTime = 0f;
            float transitionTime = 1f;

            while (elapsedTime < transitionTime)
            {
                float percent = elapsedTime / transitionTime;
                wall.transform.position = Vector3.Lerp(fromPos, toPos, percent);

                elapsedTime += Time.deltaTime;
                yield return 0;
            }

            Destroy(wall.gameObject);
        }

    }
}