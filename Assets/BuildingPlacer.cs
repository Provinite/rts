using UnityEngine;
class BuildingPlacer : MonoBehaviour {
  private Building _placedBuilding = null;
  private Ray _ray;
  private RaycastHit _raycastHit;
  private Vector3 _lastPlacementPosition;
  void Start() {
  }

  void Update() {
    if (_placedBuilding != null) {
      if (Input.GetKeyUp(KeyCode.Escape)) {
        _CancelPlacedBuilding();
        return;
      }

      _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      if (Physics.Raycast(_ray, out _raycastHit, 1000f, Globals.TERRAIN_LAYER_MASK)) {
        _placedBuilding.SetPosition(_raycastHit.point);
        if (_lastPlacementPosition != _raycastHit.point) {
          _placedBuilding.CheckValidPlacement();
        }
        _lastPlacementPosition = _raycastHit.point;
      }

      if (_placedBuilding.HasValidPlacement && Input.GetMouseButtonDown(0)) {
        _placedBuilding.Place();
        var dataIndex = _placedBuilding.DataIndex;
        _placedBuilding = null;
        PreparePlacedBuilding(dataIndex);
      }
    }
  }

  public void PreparePlacedBuilding(int buildingDataIndex) {
    if (_placedBuilding != null) {
      Destroy(_placedBuilding.Transform.gameObject);
    }
    Building building = new Building(Globals.BUILDING_DATA[buildingDataIndex]);
    building.Transform.GetComponent<BuildingManager>().Initialize(building);
    _placedBuilding = building;
    _lastPlacementPosition = Vector3.zero;
  }

  void _CancelPlacedBuilding() {
    Destroy(_placedBuilding.Transform.gameObject);
    _placedBuilding = null;
  }
}