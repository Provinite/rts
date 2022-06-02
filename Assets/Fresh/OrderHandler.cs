using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Component responsible for:
/// <list type="bullet">
///   <item>
///     <description>Tracking selection state</description>
///   </item>
///   <item>
///     <description>Handling selection from clicks</description>
///   </item>
///   <item>
///     <description>Issuing unit orders in response to clicks</description>
///   </item>
/// </list>
/// </summary>
public class OrderHandler : MonoBehaviour {
  /// <summary>
  /// Start dragging after this many seconds of holding the mouse button.
  /// </summary>
  /// <remarks>
  /// Works in an OR fashion with  <see cref="DraggingDistance" />
  /// </remarks>
  public float DraggingDelay = 0.05f;
  /// <summary>
  /// Start dragging after moving this far from the original position while
  /// holding the mouse button.
  /// </summary>
  /// <remarks>
  /// Works in an OR fashion with <see cref="DraggingDelay" />
  /// </remarks>
  public float DraggingDistance = 5;

  /// <summary>
  /// The currently selected game objects
  /// </summary>
  private List<GameObject> _selectedGameObjects = new List<GameObject>();

  /// <summary>
  /// The MovableUnit scripts associated with the currently selected units;
  /// read-only
  /// </summary>
  private List<MovableUnit> _selectedMovableUnits = new List<MovableUnit>();

  /// <summary>
  /// <c>true</c> when the primary mouse butotn is held down.
  /// </summary>
  private bool _primaryMouseDown = false;

  /// <summary>
  /// The screen space coordinates of the last mouse down action.
  /// </summary>
  private Vector3 _primaryMouseDownPosition;

  /// <summary>
  /// The time at which the primary mouse button last went down
  /// </summary>
  float _primaryMouseDownTime;

  /// <summary>
  /// <c>true</c> when the user is currently dragging a box
  /// </summary>
  private bool _isDragging = false;

  /// <summary>
  /// GUI Style for selection box
  /// </summary>
  private GUIStyle _selectionBoxStyle;

  /// <summary>
  /// Top left of current selection box screen-space coordinates
  /// </summary>
  private Vector2 _topLeft;

  /// <summary>
  /// Bottom right of the current selection box in screen-space coordinates
  /// </summary>
  private Vector2 _bottomRight;


  /// <summary>
  /// Set up the order handler before the first frame
  /// </summary>
  /// <remarks>
  /// Initializes textures and GUI styles needed for box selection.
  /// </remarks>
  void Start() {
    var selectionBoxTexture = new Texture2D(1, 1);
    selectionBoxTexture.SetPixel(0, 0, Color.green);
    selectionBoxTexture.Apply();
    _selectionBoxStyle = new GUIStyle();
    _selectionBoxStyle.normal.background = selectionBoxTexture;
  }

  /// <summary>
  /// Convert a screen-space position to a GUI-space position
  /// </summary>
  /// <param name="screenPosition">Screen-space coordinates to convert</param>
  /// <returns>The same coordinates in GUI-space</returns>
  private Vector2 _screenPosToGuiPos(Vector3 screenPosition) {
    return new Vector2(screenPosition.x, Screen.height - screenPosition.y);
  }

  /// <summary>
  /// Convert GUI-space position to a screen-space position
  /// </summary>
  /// <param name="guiPosition">GUI-space position</param>
  /// <returns>The same coordinates in screen-space</returns>
  private Vector2 _guiPosToScreenPos(Vector2 guiPosition) {
    return new Vector2(guiPosition.x, Screen.height - guiPosition.y);
  }

  /// <summary>
  /// Cast a ray from the camera to terrain and return the hit point.
  /// </summary>
  /// <param name="screenPosition">
  /// Position on the screen (screen-spacecoordinates)
  /// </param>
  /// <returns>
  /// <para>
  /// A tuple. The first element (<c>valid</c>) being a boolean indicating
  /// whether the operationsucceeded.
  /// </para>
  /// <para>
  /// The second element being (<c>worldPosition</c>) the position on the
  /// terrain aligned with <c>screenPosition</c>.
  /// </para>
  /// </returns>
  private (bool valid, Vector3 worldPosition) _screenPosToTerrainPos(Vector3 screenPosition) {
    RaycastHit raycastHit;
    Ray ray = Camera.main.ScreenPointToRay(screenPosition);
    // check if click is a unit
    if (Physics.Raycast(ray, out raycastHit, 1000f, Globals.TERRAIN_LAYER_MASK)) {
      return (true, raycastHit.point);
    }
    return (false, Vector3.zero);
  }

  /// <summary>
  /// Draw box selection GUI as-needed
  /// </summary>
  void OnGUI() {
    if (_isDragging) {
      var mousePosition = _screenPosToGuiPos(Input.mousePosition);
      var startPosition = _screenPosToGuiPos(_primaryMouseDownPosition);

      var (topLeft, bottomRight) = GuiUtils.Square(mousePosition, startPosition, 2, _selectionBoxStyle);

      _topLeft = _guiPosToScreenPos(topLeft);
      _bottomRight = _guiPosToScreenPos(bottomRight);
    }
  }

  /// <summary>
  /// Handle frame-to-frame operations for orders.
  /// </summary>
  /// <remarks>
  /// Detects &amp; handles user interactions and maintains component state.
  /// <para>
  /// TODOS:
  /// <list type="bullet">
  ///   <item>
  ///     <description>Break this function up, its handling a lot</description>
  ///   </item>
  ///   <item>
  ///     <description>Multi-select</description>
  ///   </item>
  /// </list>
  /// </para>
  /// </remarks>
  void Update() {
    _cullSelectedUnits();

    bool isMouseUpFrame = false;
    if (Input.GetMouseButtonDown(0)) {
      _primaryMouseDown = true;
      _primaryMouseDownPosition = Input.mousePosition;
      _primaryMouseDownTime = Time.time;
    }
    if (Input.GetMouseButtonUp(0) && _primaryMouseDown) {
      isMouseUpFrame = true;
      _primaryMouseDown = false;
    }
    float timeSinceMouseDown = Time.time - _primaryMouseDownTime;
    if (!_isDragging && _primaryMouseDown) {
      _isDragging =
        (timeSinceMouseDown >= DraggingDelay) ||
        ((Input.mousePosition - _primaryMouseDownPosition).magnitude >= DraggingDistance);
    }

    if (_isDragging) {
      var topRightMousePos = new Vector2(_bottomRight.x, _topLeft.y);
      var bottomLeftMousePos = new Vector2(_topLeft.x, _bottomRight.y);
      var (validTopLeft, topLeft) = _screenPosToTerrainPos(_topLeft);
      var (validBottomRight, bottomRight) = _screenPosToTerrainPos(_bottomRight);
      var (validTopRight, topRight) = _screenPosToTerrainPos(topRightMousePos);
      var (validBottomLeft, bottomLeft) = _screenPosToTerrainPos(bottomLeftMousePos);

      Debug.DrawLine(topLeft, topRight, Color.red);
      Debug.DrawLine(topRight, bottomRight, Color.red);
      Debug.DrawLine(bottomRight, bottomLeft, Color.red);
      Debug.DrawLine(bottomLeft, topLeft, Color.red);
      Debug.DrawLine(topLeft, Camera.main.ScreenToWorldPoint(_topLeft));
      Debug.DrawLine(topRight, Camera.main.ScreenToWorldPoint(_topLeft));

      Debug.DrawLine(bottomRight, Camera.main.ScreenToWorldPoint(_bottomRight));
      Debug.DrawLine(bottomLeft, Camera.main.ScreenToWorldPoint(_bottomRight));

      if (isMouseUpFrame) {

        // TODO: create pyramid instead of point-face cuboidish thing
        var selectionMesh = SelectionMeshFactory.GenerateFrustumSelectionMesh(new Vector3[] {
          topLeft,
          topRight,
          bottomLeft,
          bottomRight,
          Camera.main.ScreenToWorldPoint(_topLeft),
          Camera.main.ScreenToWorldPoint(topRightMousePos),
          Camera.main.ScreenToWorldPoint(bottomLeftMousePos),
          Camera.main.ScreenToWorldPoint(_bottomRight)
      });
        var selectionObj = new GameObject();
        var del = selectionObj.AddComponent<OnTriggerDelegate>();
        bool cleared = false;
        del.EnterDelegate = (other) => {
          if (!cleared) {
            _deselectAll();
            cleared = true;
          }
          var attachedRigidbody = other.attachedRigidbody;
          _addSelection(attachedRigidbody.gameObject);
        };

        var selectionCollider = selectionObj.AddComponent<MeshCollider>();
        selectionCollider.sharedMesh = selectionMesh;
        selectionCollider.convex = true;
        selectionCollider.isTrigger = true;

        Destroy(selectionObj, 0.02f);
      }
    }



    if (isMouseUpFrame && _isDragging) {
      // onDragEnd
      // TODO
    } else if (isMouseUpFrame && !_isDragging) {
      // onClick
      RaycastHit raycastHit;
      Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      if (Physics.Raycast(ray, out raycastHit)) {
        _deselectAll();
        _addSelection(raycastHit.rigidbody.gameObject);
      }
    }

    if (isMouseUpFrame) {
      _isDragging = false;
    }

    if (Input.GetMouseButtonUp(1) && _selectedGameObjects.Count != 0) {
      RaycastHit raycastHit;
      Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      // check if click is a unit
      if (Physics.Raycast(ray, out raycastHit)) {
        if (raycastHit.collider?.transform?.parent?.GetComponent<MovableUnit>() != null) {
          var target = raycastHit.rigidbody.gameObject;
          _selectedGameObjects.ForEach(gameObject => ArrowFactory.Spawn(gameObject, target));
        } else if (Physics.Raycast(ray, out raycastHit, 1000f, Globals.TERRAIN_LAYER_MASK)) {
          _selectedMovableUnits.ForEach(movableUnit => {
            if (movableUnit != null) {
              movableUnit.MoveTo(raycastHit.point);
            }
          });
        }
      }
    }
  }

  private void _addSelection(GameObject gameObject) {
    _selectedGameObjects.Add(gameObject);
    var movableUnit = gameObject.GetComponent<MovableUnit>();
    if (movableUnit != null) movableUnit.SelectionEnabled = true;
    _selectedMovableUnits.Add(movableUnit);
  }

  private void _deselectAll() {
    _selectedMovableUnits.ForEach(movableUnit => { if (movableUnit != null) movableUnit.SelectionEnabled = false; });
    _selectedGameObjects.Clear();
    _selectedMovableUnits.Clear();
  }

  private void _cullSelectedUnits() {
    _selectedGameObjects.RemoveAll(go => go == null);
    _selectedMovableUnits.RemoveAll(go => go == null);
  }
}

