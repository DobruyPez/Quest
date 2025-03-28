using UnityEngine;
using Zenject;

public class CameraMovementController : MonoBehaviour
{
    [SerializeField] private Transform _player; // Ссылка на игрока
    [SerializeField] private Transform _cameraTransform; // Ссылка на камеру
    [SerializeField] private Transform _centerLocationObj; // Центр локации с границами движения камеры
    [SerializeField] private float followDelay = 0.3f; // Запаздывание камеры
    [SerializeField] private Vector3 offset = new Vector3(0, 10, -10); // Смещение камеры (Y и Z)
    [SerializeField] private float tiltAngle = 50f; // Угол наклона камеры (в градусах)
    [SerializeField] private float rotationSpeed = 2f; // Скорость вращения камеры вокруг игрока
    [SerializeField] private Vector2 boundarySize = new Vector2(5f, 5f);

    [Header("Zoom Settings")]
    [SerializeField] private float minZoom = 5f; // Минимальный зум
    [SerializeField] private float maxZoom = 15f; // Максимальный зум
    [SerializeField] private float zoomSpeed = 2f; // Скорость зума

    private Vector3 velocity = Vector3.zero; // Для плавного перемещения
    [SerializeField] private Vector3 squareCenter;
    private float currentRotationAngle = 0f; // Текущий угол вращения вокруг игрока

    [Inject]
    private void Construct(PlayerMoveController player)
    {
        Debug.Log("Player injected: " + player);
        _player = player.gameObject.transform;
    }

    void Start()
    {
        if (_player == null)
        {
            Debug.LogError("Player is not assigned!");
            enabled = false;
            return;
            //_player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        if (_centerLocationObj == null)
        {
            Debug.LogError("Center Location Object is not assigned!");
            enabled = false;
            return;
        }

        Camera.main.orthographic = false;
        squareCenter = _player.position;
        HandleZoom(true);
    }

    void LateUpdate()
    {
        UpdateSquarePosition();
        //HandleRotation(); 
        FollowPlayer();
        HandleZoom();
    }

    void UpdateSquarePosition()
    {
        Vector3 playerPosition = _player.position;
        Vector3 boundarySize3d = new Vector3(boundarySize.x, 0, boundarySize.y) / 2;

        float minX = squareCenter.x - boundarySize3d.x;
        float maxX = squareCenter.x + boundarySize3d.x;
        float minZ = squareCenter.z - boundarySize3d.z;
        float maxZ = squareCenter.z + boundarySize3d.z;

        if (playerPosition.x < minX || playerPosition.x > maxX ||
            playerPosition.z < minZ || playerPosition.z > maxZ)
        {
            squareCenter = Vector3.SmoothDamp(squareCenter, playerPosition, ref velocity, followDelay);
        }
    }

    void HandleRotation()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            currentRotationAngle -= rotationSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.E))
        {
            currentRotationAngle += rotationSpeed * Time.deltaTime;
        }
    }

    void FollowPlayer()
    {
        if (_player == null || _cameraTransform == null) return;

        // 1. Рассчитываем направление камеры с учетом tiltAngle
        // Теперь offset.y влияет на высоту, а offset.z - на расстояние
        float horizontalDistance = offset.z;  // Полное расстояние в XZ плоскости
        float verticalDistance = offset.y;    // Высота камеры

        // 2. Вращаем смещение вокруг игрока с учетом currentRotationAngle
        Vector3 rotatedOffset = new Vector3(
            Mathf.Sin(currentRotationAngle * Mathf.Deg2Rad) * horizontalDistance,
            verticalDistance,
            Mathf.Cos(currentRotationAngle * Mathf.Deg2Rad) * horizontalDistance
        );

        // 3. Наклоняем вектор смещения вниз на tiltAngle градусов
        Quaternion tiltRotation = Quaternion.Euler(0, tiltAngle, 0);
        Vector3 finalOffset = tiltRotation * rotatedOffset;

        // 4. Позиция камеры = позиция игрока + смещение
        Vector3 targetPosition = squareCenter + finalOffset;

        // 5. Ограничение позиции камеры в пределах _centerLocationObj
        Vector3 minBounds = _centerLocationObj.position - _centerLocationObj.localScale / 2;
        Vector3 maxBounds = _centerLocationObj.position + _centerLocationObj.localScale / 2;

        targetPosition.x = Mathf.Clamp(targetPosition.x, minBounds.x, maxBounds.x);
        targetPosition.y = Mathf.Clamp(targetPosition.y, minBounds.y, maxBounds.y);
        targetPosition.z = Mathf.Clamp(targetPosition.z, minBounds.z, maxBounds.z);

        // 6. Плавное перемещение камеры
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, followDelay);

        // 7. Камера всегда смотрит на игрока
        transform.LookAt(squareCenter);
    }

    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            float newSize = offset.z - scroll * zoomSpeed;
            newSize = Mathf.Clamp(newSize, minZoom, maxZoom);
            offset.z = newSize;
        }
    }
    void HandleZoom(bool isAxis = false)
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0 || isAxis)
        {
            float newSize = offset.z - scroll * zoomSpeed;
            newSize = Mathf.Clamp(newSize, minZoom, maxZoom);
            offset.z = newSize;
        }
    }

    void OnDrawGizmos()
    {
        if (squareCenter == null || _centerLocationObj == null) return;

        // Границы локации (синий)
        Gizmos.color = Color.blue;
        Vector3 halfScale = _centerLocationObj.localScale / 2;
        Vector3 minBounds = _centerLocationObj.position - halfScale;
        Vector3 maxBounds = _centerLocationObj.position + halfScale;

        Gizmos.DrawLine(new Vector3(minBounds.x, _centerLocationObj.position.y, minBounds.z), new Vector3(maxBounds.x, _centerLocationObj.position.y, minBounds.z));
        Gizmos.DrawLine(new Vector3(maxBounds.x, _centerLocationObj.position.y, minBounds.z), new Vector3(maxBounds.x, _centerLocationObj.position.y, maxBounds.z));
        Gizmos.DrawLine(new Vector3(maxBounds.x, _centerLocationObj.position.y, maxBounds.z), new Vector3(minBounds.x, _centerLocationObj.position.y, maxBounds.z));
        Gizmos.DrawLine(new Vector3(minBounds.x, _centerLocationObj.position.y, maxBounds.z), new Vector3(minBounds.x, _centerLocationObj.position.y, minBounds.z));

        // Границы квадрата следования (зелёный)
        Gizmos.color = Color.green;
        Vector3 boundarySize3d = new Vector3(boundarySize.x, 0, boundarySize.y) / 2;
        minBounds = squareCenter - boundarySize3d;
        maxBounds = squareCenter + boundarySize3d;

        Gizmos.DrawLine(new Vector3(minBounds.x, squareCenter.y, minBounds.z), new Vector3(maxBounds.x, squareCenter.y, minBounds.z));
        Gizmos.DrawLine(new Vector3(maxBounds.x, squareCenter.y, minBounds.z), new Vector3(maxBounds.x, squareCenter.y, maxBounds.z));
        Gizmos.DrawLine(new Vector3(maxBounds.x, squareCenter.y, maxBounds.z), new Vector3(minBounds.x, squareCenter.y, maxBounds.z));
        Gizmos.DrawLine(new Vector3(minBounds.x, squareCenter.y, maxBounds.z), new Vector3(minBounds.x, squareCenter.y, minBounds.z));

        // Линия от камеры к игроку (красная)
        if (_player != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, squareCenter);
        }
    }
}