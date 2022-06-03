using UnityEngine;

public class Unit : MonoBehaviour
{
  public int Health { set; get; }
  public UnitDefinition UnitDefinition { get; protected set; }

  public Unit(UnitDefinition unitDefinition)
  {
    UnitDefinition = unitDefinition;
  }
}
