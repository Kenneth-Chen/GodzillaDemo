Shader "Custom/InsideOut" {
    
    Properties {
    _Color("Main Color", Color) = (1,1,1,1)
    _MainTex ("Base (RGB) Trans(A)", 2D)="white"{}
}
    SubShader {
 
      Tags { "RenderType" = "Opaque" }
 
      Cull Off
 
      CGPROGRAM
      #pragma surface surf Lambert vertex:vert
      
      sampler2D _MainTex;
      fixed4 _Color;
      //TextureScale = new Vector2(-1,1)
 
      void vert(inout appdata_full v)
      {
        v.normal.xyz = v.normal * -1;
      }
 
      struct Input {
          float2 uv_MainTex;
          
      };
 
      void surf (Input IN, inout SurfaceOutput o) {
          fixed4 c = tex2D(_MainTex, IN.uv_MainTex)* _Color;
          o.Albedo = c.rgb;
          o.Alpha = c.a;          
      }
 
      ENDCG
 
    }
 
    Fallback "Diffuse"
 
  }
