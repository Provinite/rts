using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Camera))]
public class HealthBarRenderer : MonoBehaviour {
  public Material HealthBarMaterial;
  private Mesh _frustumMesh;
  private Vector3[] _corners;
  private MeshCollider _collider;

  /// <summary>
  /// 0: unused
  /// 1: x offset
  /// 2: y offset
  /// 3: hp %
  /// </summary>
  private Vector4[] _bars = new Vector4[1000];
  private int _numBars = 0;

  public float CastDistance = 50;

  private HashSet<MovableUnit> _visibleUnits = new HashSet<MovableUnit>();

  private Camera _camera;

  HealthBarRenderer() : base() {
  }

  void Start() {
    Physics.IgnoreLayerCollision(Globals.CAMERA_FRUSTUM_COLLIDER_LAYER_INDEX, Globals.PROJECTILE_LAYER_INDEX);

    _camera = GetComponent<Camera>();
    var obj = new GameObject("HealthbarCameraViewCollider", new System.Type[] { typeof(MeshCollider), typeof(OnTriggerDelegate) });
    obj.layer = Globals.CAMERA_FRUSTUM_COLLIDER_LAYER_INDEX;
    _collider = obj.GetComponent<MeshCollider>();
    _collider.convex = true;
    _collider.isTrigger = true;
    _collider.enabled = false;

    var triggerDelegate = obj.GetComponent<OnTriggerDelegate>();
    triggerDelegate.EnterDelegate = handleEnter;
    triggerDelegate.ExitDelegate = handleLeave;
  }

  private void handleEnter(Collider other) {
    if (other.attachedRigidbody == null) {
      return;
    }
    var movableUnit = other.attachedRigidbody.gameObject.GetComponent<MovableUnit>();
    if (movableUnit != null) {
      _visibleUnits.Add(movableUnit);
    }
  }

  private void handleLeave(Collider other) {
    var movableUnit = other.attachedRigidbody?.gameObject.GetComponent<MovableUnit>();

    if (movableUnit != null) {
      _visibleUnits.Remove(movableUnit);
    }
  }

  void LateUpdate() {
    if (_frustumMesh == null) {
      var corners = new Vector3[5];
      var bottomLeft = _screenPosToFarClipPos(new Vector3(0, 0, 0));
      var topLeft = _screenPosToFarClipPos(new Vector3(0, Screen.height, 0));
      var topRight = _screenPosToFarClipPos(new Vector3(Screen.width, Screen.height, 0));
      var bottomRight = _screenPosToFarClipPos(new Vector3(Screen.width, 0, 0));
      var tip = transform.position;
      var offset = tip;

      corners[0] = bottomLeft - offset;
      corners[1] = topLeft - offset;
      corners[2] = topRight - offset;
      corners[3] = bottomRight - offset;
      corners[4] = tip - offset;

      _collider.enabled = true;

      _frustumMesh = SelectionMeshFactory.GenerateTrueFrustumSelectionMesh(corners);
      _collider.sharedMesh = _frustumMesh;
      _collider.transform.position = transform.position;
      _collider.transform.parent = transform.parent;
    } else {
      _collider.transform.position = transform.position;
      int idx = 0;
      List<MovableUnit> cull = new List<MovableUnit>();
      foreach (var movableUnit in _visibleUnits) {
        if (movableUnit == null) {
          cull.Add(movableUnit);
          continue;
        }
        var worldPosition = movableUnit.transform.position + 3 * Vector3.up;
        var cameraPosition = _camera.WorldToViewportPoint(worldPosition);
        if (cameraPosition.x > 1 || cameraPosition.x < 0 || cameraPosition.y > 1 || cameraPosition.y < 0) {
          continue;
        }
        Debug.DrawLine(worldPosition, worldPosition + Vector3.up * 4);

        var bar = new Vector4(0, cameraPosition.x, cameraPosition.y, movableUnit.HealthPercent);
        _bars[idx++] = bar;
      }

      _numBars = idx;
      HealthBarMaterial.SetVectorArray("_Bars", _bars);
      HealthBarMaterial.SetInt("_NumBars", _numBars);

      foreach (var objToCull in cull) {
        _visibleUnits.Remove(objToCull);
      }
    }
  }

  void OnRenderImage(RenderTexture src, RenderTexture dst) {
    Graphics.Blit(src, dst, HealthBarMaterial);
  }

  private Vector3 _screenPosToFarClipPos(Vector3 screenPosition) {
    var farPosition = new Vector3(screenPosition.x, screenPosition.y, CastDistance);
    return _camera.ScreenToWorldPoint(farPosition);
  }
}
