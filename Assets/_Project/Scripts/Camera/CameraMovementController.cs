using UnityEngine;
using Zenject;

public class CameraMovementController : MonoBehaviour
{
    [SerializeField] private Transform _player; // ������ �� ������
    [SerializeField] private Transform _cameraTransform; // ������ �� ������
    [SerializeField] private Transform _centerLocationObj; // ����� ������� � ��������� �������� ������
    [SerializeField] private float followDelay = 0.3f; // ������������ ������
    [SerializeField] private Vector3 offset = new Vector3(0, 10, -10); // �������� ������ (Y � Z)
    [SerializeField] private float tiltAngle = 50f; // ���� ������� ������ (� ��������)
    [SerializeField] private float rotationSpeed = 2f; // �������� �������� ������ ������ ������
    [SerializeField] private Vector2 boundarySize = new Vector2(5f, 5f);

    [Header("Zoom Settings")]
    [SerializeField] private float minZoom = 5f; // ����������� ���
    [SerializeField] private float maxZoom = 15f; // ������������ ���
    [SerializeField] private float zoomSpeed = 2f; // �������� ����

    private Vector3 velocity = Vector3.zero; // ��� �������� �����������
    [SerializeField] private Vector3 squareCenter;
    private float currentRotationAngle = 0f; // ������� ���� �������� ������ ������

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

        // 1. ������������ ����������� ������ � ������ tiltAngle
        // ������ offset.y ������ �� ������, � offset.z - �� ����������
        float horizontalDistance = offset.z;  // ������ ���������� � XZ ���������
        float verticalDistance = offset.y;    // ������ ������

        // 2. ������� �������� ������ ������ � ������ currentRotationAngle
        Vector3 rotatedOffset = new Vector3(
            Mathf.Sin(currentRotationAngle * Mathf.Deg2Rad) * horizontalDistance,
            verticalDistance,
            Mathf.Cos(currentRotationAngle * Mathf.Deg2Rad) * horizontalDistance
        );

        // 3. ��������� ������ �������� ���� �� tiltAngle ��������
        Quaternion tiltRotation = Quaternion.Euler(0, tiltAngle, 0);
        Vector3 finalOffset = tiltRotation * rotatedOffset;

        // 4. ������� ������ = ������� ������ + ��������
        Vector3 targetPosition = squareCenter + finalOffset;

        // 5. ����������� ������� ������ � �������� _centerLocationObj
        Vector3 minBounds = _centerLocationObj.position - _centerLocationObj.localScale / 2;
        Vector3 maxBounds = _centerLocationObj.position + _centerLocationObj.localScale / 2;

        targetPosition.x = Mathf.Clamp(targetPosition.x, minBounds.x, maxBounds.x);
        targetPosition.y = Mathf.Clamp(targetPosition.y, minBounds.y, maxBounds.y);
        targetPosition.z = Mathf.Clamp(targetPosition.z, minBounds.z, maxBounds.z);

        // 6. ������� ����������� ������
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, followDelay);

        // 7. ������ ������ ������� �� ������
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

        // ������� ������� (�����)
        Gizmos.color = Color.blue;
        Vector3 halfScale = _centerLocationObj.localScale / 2;
        Vector3 minBounds = _centerLocationObj.position - halfScale;
        Vector3 maxBounds = _centerLocationObj.position + halfScale;

        Gizmos.DrawLine(new Vector3(minBounds.x, _centerLocationObj.position.y, minBounds.z), new Vector3(maxBounds.x, _centerLocationObj.position.y, minBounds.z));
        Gizmos.DrawLine(new Vector3(maxBounds.x, _centerLocationObj.position.y, minBounds.z), new Vector3(maxBounds.x, _centerLocationObj.position.y, maxBounds.z));
        Gizmos.DrawLine(new Vector3(maxBounds.x, _centerLocationObj.position.y, maxBounds.z), new Vector3(minBounds.x, _centerLocationObj.position.y, maxBounds.z));
        Gizmos.DrawLine(new Vector3(minBounds.x, _centerLocationObj.position.y, maxBounds.z), new Vector3(minBounds.x, _centerLocationObj.position.y, minBounds.z));

        // ������� �������� ���������� (������)
        Gizmos.color = Color.green;
        Vector3 boundarySize3d = new Vector3(boundarySize.x, 0, boundarySize.y) / 2;
        minBounds = squareCenter - boundarySize3d;
        maxBounds = squareCenter + boundarySize3d;

        Gizmos.DrawLine(new Vector3(minBounds.x, squareCenter.y, minBounds.z), new Vector3(maxBounds.x, squareCenter.y, minBounds.z));
        Gizmos.DrawLine(new Vector3(maxBounds.x, squareCenter.y, minBounds.z), new Vector3(maxBounds.x, squareCenter.y, maxBounds.z));
        Gizmos.DrawLine(new Vector3(maxBounds.x, squareCenter.y, maxBounds.z), new Vector3(minBounds.x, squareCenter.y, maxBounds.z));
        Gizmos.DrawLine(new Vector3(minBounds.x, squareCenter.y, maxBounds.z), new Vector3(minBounds.x, squareCenter.y, minBounds.z));

        // ����� �� ������ � ������ (�������)
        if (_player != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, squareCenter);
        }
    }
}