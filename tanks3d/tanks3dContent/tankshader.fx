//-----------------------------------------------------------------------------
// EnvironmentMap.fx
//
// Microsoft Game Technology Group
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------


float4x4 World;
float4x4 View;
float4x4 Projection;

float3 LightDirection;
float3 LightColor;
float3 AmbientColor;

texture Texture;
texture EnvironmentMap;

bool TextureEnabled;


struct VS_INPUT
{
    float4 Position : POSITION0;
    float3 Normal : NORMAL0;
    float2 TexCoord : TEXCOORD0;
};


struct VS_OUTPUT
{
    float4 Position : POSITION0;
    float2 TexCoord : TEXCOORD0;
    float3 Fresnel : COLOR0;
    float3 Lighting : COLOR1;
};


VS_OUTPUT VertexShaderFunction(VS_INPUT input)
{
    VS_OUTPUT output;

    // Transform the input values.
    float4 worldPosition = mul(input.Position, World);
    float3 worldNormal = mul(input.Normal, World);

    output.Position = mul(mul(worldPosition, View), Projection);

    output.TexCoord = input.TexCoord;
    
    // Compute a reflection vector for the environment map.
    float3 eyePosition = mul(-View._m30_m31_m32, transpose(View));

    float3 viewVector = worldPosition - eyePosition;
    
    // Approximate a Fresnel coefficient for the environment map.
    // This makes the surface less reflective when you are looking
    // straight at it, and more reflective when it is viewed edge-on.
    output.Fresnel = saturate(1 + dot(normalize(viewVector), worldNormal));
    
    // Compute lighting.
    float lightAmount = max(dot(worldNormal, LightDirection), 0);

    output.Lighting = AmbientColor + lightAmount * LightColor;

    return output;
}


struct PS_INPUT
{
    float2 TexCoord : TEXCOORD0;
    float3 Fresnel : COLOR0;
    float3 Lighting : COLOR1;
};


sampler TextureSampler = sampler_state
{
    Texture = (Texture);

    MinFilter = Linear;
    MagFilter = Linear;
    MipFilter = Linear;
    
    AddressU = Wrap;
    AddressV = Wrap;
};


float4 PixelShaderFunction(PS_INPUT input) : COLOR0
{
    // Sample the texture and environment map.
    float3 color;
    
    if (TextureEnabled)
        color = tex2D(TextureSampler, input.TexCoord);
    else
        color = float3(0, 0, 0);

    // Apply lighting.
    color *= input.Lighting;
    
    return float4(color, 1);
}


technique EnvironmentMapTechnique
{
    pass Pass1
    {
        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
