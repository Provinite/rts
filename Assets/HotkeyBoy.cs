using UnityEngine;

[RequireComponent(typeof(BuildingPlacer))]
public class HotkeyBoy : MonoBehaviour {
  private BuildingPlacer _buildingPlacer;
  private GameObject _selectedUnit;

  void Start() {
    _buildingPlacer = GetComponent<BuildingPlacer>();
  }

  void Update() {
    if (Input.GetKeyDown(KeyCode.A)) {
      _buildingPlacer.PreparePlacedBuilding(0);
    }
    if (Input.GetKeyDown(KeyCode.B)) {
      var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      RaycastHit raycastHit;
      if (Physics.Raycast(ray, out raycastHit, 1000f, Globals.TERRAIN_LAYER_MASK)) {
        var unit = new Unit(Globals.UNIT_DATA[0]);
        unit.GameObject.transform.position = raycastHit.point;
      }
    }
    if (Input.GetMouseButtonDown(0)) {
      var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      RaycastHit raycastHit;
      if (Physics.Raycast(ray, out raycastHit)) {
        Debug.Log(raycastHit.transform.gameObject.name);
        if (raycastHit.rigidbody != null && raycastHit.rigidbody.gameObject.name.StartsWith("Unit")) {
          _selectedUnit = raycastHit.rigidbody.gameObject;
        }
      }
    }
    if (Input.GetMouseButtonDown(1)) {
      if (_selectedUnit != null) {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit raycastHit;
        if (Physics.Raycast(ray, out raycastHit, 1000f, Globals.TERRAIN_LAYER_MASK)) {
          _selectedUnit.GetComponent<UnitManager>().initiateMoveTo(raycastHit.point);
        }
      }
    }
  }
}