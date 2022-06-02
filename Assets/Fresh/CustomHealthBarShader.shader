Shader "HealthBar/HealthBar"
{
  Properties {
    _MainTex("Texture", 2D) = "white" {}
    _Color("HP Color", Color) = (0, 1, 0, 1)
    _BgColor("Background Color", Color) = (1, 0, 0, 1)
    _Width("Width", Float) = .2
    _Height("Height", Float) = .05
  }
  SubShader
  {
    Tags
    {
      "Queue" = "Transparent"
    }
    Pass
    {
      Blend SrcAlpha OneMinusSrcAlpha
      CGPROGRAM
      #pragma vertex vert
      #pragma fragment frag

      #include "UnityCG.cginc"
      #include "2D_SDF.cginc"

      struct appdata {
        float4 vertex: POSITION;
        float2 uv : TEXCOORD0;
      };

      struct v2f {
        float4 vertex : SV_POSITION;
        float2 uv: TEXCOORD0;
      };

      float4 _Bars[1000];
      int _NumBars;

      sampler2D _MainTex;
      fixed4 _Color;
      float _Width;
      float _Height;
      float4 _BgColor;

      v2f vert (appdata v) {
        v2f o;
        o.vertex = UnityObjectToClipPos(v.vertex);
        o.uv = v.uv;
        return o;
      }
      
      float4 frag(v2f i) : SV_TARGET {
        float4 result = float4(0, 0, 0, 0);
        for (int idx = 0; idx < _NumBars; idx++) {
          float4 bar = _Bars[idx];
          float2 offset = -1 * float2(bar.y, bar.z);
          float innerWidth = _Width * bar.w;
          float innerXOffset = (_Width - (innerWidth));
          float outerRectangle = rectangle(i.uv + offset, float2(_Width, _Height));
          float innerRectangle = rectangle(float2(i.uv.x + innerXOffset, i.uv.y) + offset, float2(innerWidth, _Height));

          float hasInnerPx = step(innerRectangle, 0);
          float hasOuterPx = step(outerRectangle, 0);
          result += (_BgColor * (1 - hasInnerPx) * hasOuterPx + _Color * hasInnerPx);
        }
        fixed hasResult = result.a > .5;
        return hasResult * result + (1-hasResult) * tex2D(_MainTex, i.uv);
      }

      ENDCG
    }
  }
}