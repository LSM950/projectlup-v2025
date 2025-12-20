using TMPro.Examples;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using TouchPhase = UnityEngine.TouchPhase;

namespace LUP.PCR
{
    public class PCRCameraController : MonoBehaviour
    {
        [SerializeField] private static PCRCameraController Instance; // @TODO : 싱글톤 필요없으면 삭제하기
        [SerializeField] private BoxCollider mapArea;
        [SerializeField] private Camera cam;
        
        private Vector2 lastInputPosition;
        private bool isDragging = false;

        [SerializeField] private float dragSpeed = 10f;
        //[SerializeField] private float padding = 5f; // 맵 끝에서 얼마나 여유를 둘지 (0이면 딱 끝까지 감)

        private void Awake()
        {
            if (cam == null)
            {
                cam = Camera.main;
            }
        }

        private void Update()
        {
            HandleInput();

           if (mapArea != null)
            {
                ClampCameraPosition();
            }
        }


        private void HandleInput()
        {
            // 모바일 입력 처리
            if (Input.touchCount == 1)
            {

            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                isDragging = true;
                lastInputPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                MoveCamera(touch.position);
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                isDragging = false;
            }

            }
            // 마우스 입력 처리 (에디터 테스트용)
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    isDragging = true;
                    lastInputPosition = Input.mousePosition;
                }
                else if (Input.GetMouseButton(0) && isDragging)
                {
                    MoveCamera(Input.mousePosition);
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    isDragging = false;
                }
            }
        }

        private void MoveCamera(Vector2 currentInputPosition)
        {
            // 이전 프레임 좌표와 현재 좌표의 차이(Delta) 계산
            Vector2 inputDelta = lastInputPosition - currentInputPosition;

            Vector3 moveDir = new Vector3(inputDelta.x, 0, inputDelta.y);

            transform.Translate(moveDir * dragSpeed * Time.deltaTime, Space.World);

            lastInputPosition = currentInputPosition;
        }


        // 이동이 끝난 후, 현재 위치가 영역 밖이라면 강제로 안으로 넣음
        private void ClampCameraPosition()
        {
            Bounds bounds = mapArea.bounds;
            Vector3 currentPos = transform.position;

            // 0
            float distanceToGround = transform.position.y;

            // FOV = 시야각
            //  Tan(FOV의 절반) * 거리 = 화면 높이 절반
            float halfHeight = distanceToGround * Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad);

            // 화면 비율 (Aspect Ratio) 로 가로 절반 크기 도출
            float halfWidth = halfHeight * cam.aspect;

            float minX = bounds.min.x + halfWidth;
            float maxX = bounds.max.x - halfWidth;
            float minZ = bounds.min.z + halfHeight;
            float maxZ = bounds.max.z - halfHeight;

            // 제한 영역 계산 (맵 전체 크기에서 카메라 반쪽 크기만큼 안쪽으로 줄임)
            float clampedX = Mathf.Clamp(currentPos.x, minX, maxX);
            float clampedZ = Mathf.Clamp(currentPos.z, minZ, maxZ);

           transform.position = new Vector3(clampedX, currentPos.y, clampedZ);
        }
    }
}
