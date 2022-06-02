using System;
using System.Collections.Generic;
using UnityEngine;

public class Building {
  public BuildingData Data;
  public Transform Transform;
  public int CurrentHealth;

  public BuildingPlacement Placement;
  private List<Material> Materials;
  private BuildingManager _buildingManager;

  public Building(BuildingData data) {
    Data = data;
    CurrentHealth = data.HealthPoints;
    var buildingObj = GameObject.Instantiate(
      Resources.Load($"Prefabs/Buildings/{Data.Code}")
    ) as GameObject;
    Transform = buildingObj.transform;
    Placement = BuildingPlacement.VALID;
    Materials = new List<Material>();
    _buildingManager = buildingObj.GetComponent<BuildingManager>();

    foreach (Material material in Transform.Find("Mesh").GetComponent<Renderer>().materials) {
      Materials.Add(new Material(material));
    }
  }

  public void SetPosition(Vector3 position) {
    Transform.position = position;
  }

  public int MaxHP {
    get => Data.HealthPoints;
  }

  public int DataIndex {
    get {
      return Array.FindIndex(
        Globals.BUILDING_DATA,
        (e) => e.Code == Data.Code
      );
    }
  }

  public void SetMaterials() {
    SetMaterials(Placement);
  }

  public void SetMaterials(BuildingPlacement placement) {
    List<Material> materials;
    if (placement == BuildingPlacement.VALID || placement == BuildingPlacement.INVALID) {
      string materialName = placement == BuildingPlacement.VALID ? "valid" : "invalid";
      Material refMaterial = Resources.Load($"{materialName}") as Material;
      materials = new List<Material>();
      Materials.ForEach(m => materials.Add(refMaterial));
    } else if (placement == BuildingPlacement.FIXED) {
      materials = Materials;
    } else {
      return;
    }
    Transform.Find("Mesh").GetComponent<Renderer>().materials = materials.ToArray();
  }
  public void CheckValidPlacement() {
    if (Placement == BuildingPlacement.FIXED) return;

    Placement = _buildingManager.CheckPlacement() ? BuildingPlacement.VALID : BuildingPlacement.INVALID;
  }

  public bool HasValidPlacement {
    get => Placement == BuildingPlacement.VALID;
  }

  public void Place() {
    Placement = BuildingPlacement.FIXED;
    SetMaterials();
    // enable collision
    Transform.GetComponent<BoxCollider>().isTrigger = false;
  }

  public bool IsFixed { get => Placement == BuildingPlacement.FIXED; }
}

public enum BuildingPlacement {
  VALID,
  FIXED,
  INVALID
}