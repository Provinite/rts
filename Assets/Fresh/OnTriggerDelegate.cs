using System;
using UnityEngine;
public class OnTriggerDelegate : MonoBehaviour {
  public Action<Collider> EnterDelegate = (_) => { };
  public Action<Collider> ExitDelegate = (_) => { };
  void OnTriggerEnter(Collider other) {
    EnterDelegate(other);
  }
  void OnTriggerExit(Collider other) {
    ExitDelegate(other);
  }
}