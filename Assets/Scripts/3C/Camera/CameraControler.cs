using UnityEngine;

// TopDownPlate 中的代码是可复用的，其他的可能只有该项目能用所有未进行命名空间设定
namespace TopDownPlate
{
    [RequireComponent(typeof(Camera))]
    [AddComponentMenu("TopDownPlate/Camera/Camera Controller")]
    public class CameraControler : MonoBehaviour
    {
        [Space(10)]
        [Tooltip("摄像机的跟随速度")]
        public float CameraSpeed = 0.3f;

        [Space(10)]
        [Header("Camera Zoom")]
        [Range(1, 20)]
        [Tooltip("正交摄像机最小size")]
        public float MinimumZoom = 5f;
        [Range(1, 20)]
        [Tooltip("正交摄像机最大size")]
        public float MaximumZoom = 10f;
        [Tooltip("摄像机大小缩放速度")]
        public float ZoomSpeed = 0.4f;

        [Space(10)]
        [Header("Info")]
        [ReadOnly]
        [Tooltip("摄像机当前跟随的目标")]
        public Transform Target;
        [ReadOnly]
        public CharacterController TargetController;

        protected Bounds levelBounds;
        protected float xMin;
        protected float xMax;
        protected float yMin;
        protected float yMax;

        protected float offsetZ; // 使相机和Target z轴相对位置不变
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
                Debug.LogWarning("LevelManager.Instance.Player为空");
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
                Debug.LogWarning("主角未包含组件：CharacterController");
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
