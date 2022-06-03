// using UnityEngine;

// [RequireComponent(typeof(BoxCollider))]
// public class BuildingManager : MonoBehaviour {
//   private BoxCollider _collider;
//   private Building _building = null;

//   private int _numCollisions = 0;
//   public void Initialize(Building building) {
//     _collider = GetComponent<BoxCollider>();
//     _building = building;
//     CheckPlacement();
//   }

//   private int _lastLoggedCollisions = 0;

//   public void Update() {
//     if (_lastLoggedCollisions == _numCollisions) return;
//     _lastLoggedCollisions = _numCollisions;
//   }

//   private void OnTriggerEnter(Collider other) {
//     if (other.tag == "Terrain") return;

//     _numCollisions++;
//     CheckPlacement();
//   }

//   private void OnTriggerExit(Collider other) {
//     if (other.tag == "Terrain") return;
//     _numCollisions--;
//     CheckPlacement();
//   }

//   public bool CheckPlacement() {
//     if (_building == null || _building.IsFixed) return false;
//     bool validPlacement = HasValidPlacement;
//     if (!validPlacement) {
//       _building.SetMaterials(BuildingPlacement.INVALID);
//     } else {
//       _building.SetMaterials(BuildingPlacement.VALID);
//     }
//     return validPlacement;
//   }

//   public bool HasValidPlacement {
//     get => _numCollisions == 0;
//   }
// }
