Shader "HealthBar/HealthBar"
{
  Properties {
    _Color("HP Color", Color) = (0, 1, 0, 1)
    _BgColor("Background Color", Color) = (1, 0, 0, 1)
    _Width("Width", Float) = .2
    _Health("HP", Float) = .5
    _Height("Height", Float) = .05
    _Offset("Offset", Vector) = (0.5, 0.5, 0, 0)
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

      fixed4 _Color;
      float _Width;
      float _Height;
      float _Health;
      float2 _Offset;
      float4 _BgColor;

      v2f vert (appdata v) {
        v2f o;
        o.vertex = UnityObjectToClipPos(v.vertex);
        o.uv = v.uv - float2(_Offset.r, _Offset.g);
        return o;
      }
      
      float4 frag(v2f i) : SV_TARGET {
        float outerRectangle = rectangle(i.uv, float2(_Width, _Height));
        float innerWidth = _Width * _Health;
        float offsetX = (_Width - (innerWidth));
        float innerRectangle = rectangle(float2(i.uv.x + offsetX, i.uv.y), float2(innerWidth, _Height));
        // fixed4 textureColor = tex2D(_MainTex, i.uv);

        float hasInnerPx = step(innerRectangle, 0);
        float hasOuterPx = step(outerRectangle, 0);
        return _BgColor * (1 - hasInnerPx) * hasOuterPx + _Color * hasInnerPx;
      }
      ENDCG
    }
  }
}