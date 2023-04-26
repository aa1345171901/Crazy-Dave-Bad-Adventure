using UnityEngine;

// TopDownPlate �еĴ����ǿɸ��õģ������Ŀ���ֻ�и���Ŀ��������δ���������ռ��趨
namespace TopDownPlate
{
    [RequireComponent(typeof(Camera))]
    [AddComponentMenu("TopDownPlate/Camera/Camera Controller")]
    public class CameraControler : MonoBehaviour
    {
        [Space(10)]
        [Tooltip("������ĸ����ٶ�")]
        public float CameraSpeed = 0.3f;

        [Space(10)]
        [Header("Camera Zoom")]
        [Range(1, 20)]
        [Tooltip("�����������Сsize")]
        public float MinimumZoom = 5f;
        [Range(1, 20)]
        [Tooltip("������������size")]
        public float MaximumZoom = 10f;
        [Tooltip("�������С�����ٶ�")]
        public float ZoomSpeed = 0.4f;

        [Space(10)]
        [Header("Info")]
        [ReadOnly]
        [Tooltip("�������ǰ�����Ŀ��")]
        public Transform Target;
        [ReadOnly]
        public CharacterController TargetController;

        protected Bounds levelBounds;
        protected float xMin;
        protected float xMax;
        protected float yMin;
        protected float yMax;

        protected float offsetZ; // ʹ�����Target z�����λ�ò���
        protected Vector3 lastTargetPosition;
        protected Vector3 currentVelocity;

        protected float currentZoom;
        protected Camera _camera;

        private void Start()
        {
            Initialization();
        }

        protected virtual void Initialization()
        {
            _camera = this.gameObject.GetComponent<Camera>();

            currentZoom = MinimumZoom;

            if (GameManager.Instance.Player == null)
            {
                Debug.LogWarning("LevelManager.Instance.PlayerΪ��");
                return;
            }
            AssignTarget();

            lastTargetPosition = Target.position;
            offsetZ = (transform.position - Target.position).z;
            transform.parent = null;
        }

        protected virtual void AssignTarget()
        {
            Target = GameManager.Instance.Player.transform;
            if (Target.GetComponent<CharacterController>() == null)
            {
                Debug.LogWarning("����δ���������CharacterController");
                return;
            }

            TargetController = Target.GetComponent<CharacterController>();
        }

        protected virtual void LateUpdate()
        {
            GetLevelBounds();
            Zoom();
            FollowPlayer();
        }

        protected virtual void FollowPlayer()
        {
            Vector3 aheadTargetPos = Target.position + Vector3.forward * offsetZ;

            if (levelBounds.size != Vector3.zero)
            {
                aheadTargetPos.x = Mathf.Clamp(aheadTargetPos.x, xMin, xMax);
                aheadTargetPos.y = Mathf.Clamp(aheadTargetPos.y, yMin, yMax);
            }

            Vector3 newCameraPosition = Vector3.SmoothDamp(transform.position, aheadTargetPos, ref currentVelocity, CameraSpeed);

            transform.position = newCameraPosition;

            lastTargetPosition = Target.position;
        }

        protected virtual void Zoom()
        {
            if (TargetController == null)
            {
                return;
            }

            float characterSpeed = Mathf.Abs(TargetController.Rigidbody.velocity.x);
            float currentVelocity = 0f;

            currentZoom = Mathf.SmoothDamp(currentZoom, (characterSpeed / 10) * (MaximumZoom - MinimumZoom) + MinimumZoom, ref currentVelocity, ZoomSpeed);

            _camera.orthographicSize = currentZoom;
        }

        protected virtual void GetLevelBounds()
        {
            if (_camera == null)
            {
                return;
            }

            levelBounds = LevelManager.Instance.CameraBounds;

            if (levelBounds.size == Vector3.zero)
            {
                return;
            }

            float cameraHeight = _camera.orthographicSize * 2f;
            float cameraWidth = cameraHeight * _camera.aspect;

            xMin = levelBounds.min.x + (cameraWidth / 2);
            xMax = levelBounds.max.x - (cameraWidth / 2);
            yMin = levelBounds.min.y + (cameraHeight / 2);
            yMax = levelBounds.max.y - (cameraHeight / 2);

            if (levelBounds.max.x - levelBounds.min.x <= cameraWidth)
            {
                xMin = levelBounds.center.x;
                xMax = levelBounds.center.x;
            }

            if (levelBounds.max.y - levelBounds.min.y <= cameraHeight)
            {
                yMin = levelBounds.center.y;
                yMax = levelBounds.center.y;
            }
        }
    }
}
