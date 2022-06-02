using UnityEngine;
public class CameraController : MonoBehaviour {

  [Header("Camera Settings")]
  [Tooltip("How far from the edge of the screen the mouse can be to initiate camera movement")]
  public uint MoveBoundary = 10;

  [Tooltip("Camera acceleration in world units/sec^2")]
  public float Acceleration = 0.5f;

  [Tooltip("Maximum speed of the camera in world units/sec")]
  public float MaxSpeed = 5f;

  [Tooltip("Camera deceleration in world units/sec^2")]
  public float Deceleration = 0.5f;

  [Tooltip("The camera to manage")]
  public GameObject TargetCamera;

  private Vector2 _currentDirection;

  private Splider _splider {
    get => this.TargetCamera.GetComponent<Splider>();
  }

  private ushort _zoomLevel = 0;


  private static ArrayByEnum<Vector2, CameraDirection> _unitVectors = new ArrayByEnum<Vector2, CameraDirection>(
      new Vector2(-1, 0), // left
      new Vector2(0, -1), // down
      new Vector2(1, 0), // right
      new Vector2(0, 1) // up
  );

  void Update() {
    _handleLateralMovement();
    _handleZoom();
  }

  private void _handleLateralMovement() {
    // Calculate direction
    var position = Input.mousePosition;
    var offsets = new ArrayByEnum<float, CameraDirection>(
        position.x,
        position.y,
        Screen.width - position.x,
        Screen.height - position.y
    );
    Vector2 newDirection = Vector2.zero;
    offsets.ForEach((offset, direction) => {
      if (offset > 0 && offset < MoveBoundary) {
        newDirection += _unitVectors[direction];
      }
    });

    if (newDirection == Vector2.zero) {
      return;
    }
    gameObject.transform.position = gameObject.transform.position + new Vector3(newDirection.x, 0, newDirection.y) * Time.deltaTime * 40;
  }
  private void _handleZoom() {
    var previousZoomLevel = _zoomLevel;
    if (Input.mouseScrollDelta.y > 0) {
      _zoomLevel++;
    } else if (Input.mouseScrollDelta.y < 0 && _zoomLevel > 0) {
      _zoomLevel--;
    }
    if (previousZoomLevel != _zoomLevel) {
      var (valid, position, tangent) = _splider.GetPositionAtTime(_zoomLevel);
      if (!valid) {
        _zoomLevel = previousZoomLevel;
      } else {
        var level = Mathf.Max(_zoomLevel, previousZoomLevel);
        int direction = _zoomLevel < previousZoomLevel ? 1 : -1;
        TargetCamera.transform.position = position;
        TargetCamera.transform.Rotate(new Vector3(direction * _getZoomRotation(level), 0, 0));
      }
    }
  }

  private float _getZoomRotation(int level) {
    return Mathf.Max(level - 15, 0) / 60f * 0.25f;
  }
}

public enum CameraDirection : ushort {
  Left = 0,
  Down = 1,
  Right = 2,
  Up = 3
}
