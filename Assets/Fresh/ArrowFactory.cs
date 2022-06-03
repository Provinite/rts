using System;
using UnityEngine;
using UnityEngine.Splines;

public static class ArrowFactory
{
  public static GameObject Spawn(GameObject assailant, GameObject victim)
  {
    // ArrowBehavior cleans up arrows when they arrive.
    // This will change later, but whatever
    var arrowObject = Globals.ArrowPool.Scoop();
    arrowObject.transform.position = assailant.transform.position + Vector3.up;

    var behavior = arrowObject.GetComponent<ArrowBehavior>();
    behavior.Target = victim;
    behavior.TraversalSpline = GetFiringArc(assailant, victim);
    behavior.Fire();

    arrowObject.SetActive(true);

    return arrowObject;
  }

  public static Spline GetFiringArc(GameObject assailant, GameObject victim)
  {
    var origin = assailant.transform.position;
    origin.y += 1;
    var destination = victim.transform.position;
    // boom headshot
    destination.y += 1;

    var middle = (origin + destination) / 2f;
    // raise up by 1/3 of the total distance
    var distance = (destination - origin).magnitude;
    middle.y += Mathf.Max(distance / 3f, 5f);
    return SplineTweener.SplineBetween(origin, middle, destination);
  }
}
