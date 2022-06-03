using UnityEngine;

public class PointAtCamera : MonoBehaviour
{
  void Update()
  {
    transform.LookAt(Camera.main.transform);
  }
}
