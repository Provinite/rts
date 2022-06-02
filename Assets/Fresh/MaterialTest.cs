using UnityEngine;

public class MaterialTest : MonoBehaviour {
  private Material _material;
  // Start is called before the first frame update
  void Start() {
    _material = GetComponent<Renderer>().sharedMaterial;
  }

  // Update is called once per frame
  void Update() {
    Vector4[] bars = new Vector4[11];
    for (var i = 0; i < 11; i++) {
      bars[i] = new Vector4(0, Mathf.Lerp(.3f, .7f, Mathf.InverseLerp(0, 10, i)), .2f + 0.05f * i, Mathf.InverseLerp(0, 10, i));
    }
    _material.SetVectorArray("_Bars", bars);
    _material.SetInt("_NumBars", bars.Length);
  }
}
