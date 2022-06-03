using UnityEngine;

public class ArrowPool : PrefabPool
{
  public ArrowPool(int initialPoolSize, int initialPool)
    : base(Resources.Load("Prefabs/Arrow"), initialPoolSize, initialPool) { }

  protected override void _afterCreate(GameObject gameObject)
  {
    base._afterCreate(gameObject);
    gameObject.GetComponent<ArrowBehavior>().ArrowPool = this;
    gameObject.layer = Globals.PROJECTILE_LAYER_INDEX;
  }
}
