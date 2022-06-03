using UnityEngine;

public static class SelectionMeshFactory
{
  /**

  */
  private static int[] _frustumTriangles =
  {
    0, // b1
    1, // b1
    3, // b1
    1, // b2
    2, // b2
    3, // b2
    0, // front
    4, // front
    3, // front
    3, // right
    4, // right
    2, // right
    4, // rear
    1, // rear
    2, // rear
    1, // left
    4, // left
    0, // left
  };

  /// <summary>
  /// Generate a square-based pyramid mesh. Can be used to represent
  /// the view frustum of a camera.
  /// </summary>
  /// <remarks>
  /// Tri count: 6
  /// <see href="https://upload.wikimedia.org/wikipedia/commons/a/a8/Variations_of_square_pyramids.png" />
  /// </remarks>
  /// <param name="corners">
  /// 5 Vertex positions of the mesh to create. Corners 0-3 should represent
  /// the corners of the base in clockwise order. Corner 4 is the tip of
  /// the result.
  /// _____________
  /// | 1       2 |  Vertex positions of frustum mesh
  /// |   -----   |  (viewed from behind the camera)
  /// |   | 4 |   |
  /// |   -----   |
  /// | 0       3 |
  /// |-----------|
  /// </param>
  /// <returns></returns>
  public static Mesh GeneratePyramidMesh(Vector3[] corners)
  {
    Mesh selectionMesh = new Mesh();
    selectionMesh.vertices = corners;
    selectionMesh.triangles = _frustumTriangles;
    return selectionMesh;
  }
}
