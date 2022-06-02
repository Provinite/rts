using System;
using System.Collections.Generic;
using UnityEngine;

public class PrefabPool {
  private Stack<GameObject> _available;
  private HashSet<GameObject> _inUse;
  protected UnityEngine.Object _resource;

  public int CurrentPoolSize {
    get => _inUse.Count + _available.Count;
  }

  public PrefabPool(UnityEngine.Object resource, int initialPool, int initialPoolSize) {
    _available = new Stack<GameObject>(initialPoolSize);
    _inUse = new HashSet<GameObject>(initialPoolSize);
    _resource = resource;
  }

  public void Instantiate() {
    var turd = (GameObject)GameObject.Instantiate(_resource);
    _afterCreate(turd);
    _available.Push(turd);
  }

  protected virtual void _afterCreate(GameObject gameObject) {
    gameObject.SetActive(false);
  }
  protected virtual void _beforeScoop(GameObject gameObject) { }
  protected virtual void _afterPoop(GameObject gameObject) {
    gameObject.SetActive(false);
  }

  /// <summary>
  /// Drop a kid off at the pool
  /// </summary>
  /// <param name="kid">GameObject to return to the pool</param>
  public void Poop(GameObject kid) {
    if (!_inUse.Remove(kid)) {
      throw new Exception("PrefabPool: Cannot return object, not in use. " + kid.name);
    }
    _available.Push(kid);
    _afterPoop(kid);
  }

  /// <summary>
  /// Scoop some poop from the soup
  /// </summary>
  public GameObject Scoop() {
    GameObject turd;
    try {
      turd = _available.Pop();
    } catch (InvalidOperationException) {
      Instantiate();
      turd = _available.Pop();
    }
    _inUse.Add(turd);
    _beforeScoop(turd);
    return turd;
  }
}
