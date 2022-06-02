using UnityEngine;
public static class SelectionMeshFactory {
  private static int[] _cubeTriangles = { 0, 1, 2, 2, 1, 3, 4, 6, 0, 0, 6, 2, 6, 7, 2, 2, 7, 3, 7, 5, 3, 3, 5, 1, 5, 0, 1, 1, 4, 0, 4, 5, 6, 6, 5, 7 };
  private static int[] _frustumTriangles = {
    0, 1, 3, // b1
    1, 2, 3, // b2
    0, 4, 3, // front
    3, 4, 2, // right
    4, 1, 2, // rear
    1, 4, 0, // left
    };
  // topLeft, topRight, bottomLeft, bottomRight (bottom rectangle)
  // topLeft, topRight, bottomLeft, bottomRight (top rectangle)
  public static Mesh GenerateQuadPrismSelectionMesh(Vector3[] corners) {
    Vector3[] vertices = new Vector3[8];
    for (int i = 0; i < 4; i++) {
      vertices[i] = corners[i];
      vertices[i + 4] = corners[i] + (Vector3.up * 100f);
    }

    Mesh selectionMesh = new Mesh();
    selectionMesh.vertices = vertices;
    selectionMesh.triangles = _cubeTriangles;
    return selectionMesh;
  }

  /// <summary>
  /// Remove this.
  /// </summary>
  /// <param name="corners"></param>
  /// <returns></returns>
  public static Mesh GenerateFrustumSelectionMesh(Vector3[] corners) {
    Mesh selectionMesh = new Mesh();
    selectionMesh.vertices = corners;
    selectionMesh.triangles = _cubeTriangles;
    return selectionMesh;
  }

  public static Mesh GenerateTrueFrustumSelectionMesh(Vector3[] corners) {
    Mesh selectionMesh = new Mesh();
    selectionMesh.vertices = corners;
    selectionMesh.triangles = _frustumTriangles;
    return selectionMesh;
  }

}