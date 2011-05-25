struct VertexShaderInput
{
    float4 Position : POSITION0;
	float3 Normal : NORMAL;
	float2 TexCoords : TEXCOORD0;

    // TODO: add input channels such as texture
    // coordinates and vertex colors here.
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
    float4 Color		: COLOR0;
    float LightingFactor: TEXCOORD0;
    float2 TextureCoords: TEXCOORD1;

    // TODO: add vertex shader outputs such as colors and texture
    // coordinates here. These values will automatically be interpolated
    // over the triangle, and provided as input to your pixel shader.
};

//------- Constants --------
float4x4 World;
float4x4 View;
float4x4 Projection;
float3 LightDirection;
float Ambient;
bool EnableLighting;

//------- Texture Samplers --------
Texture Texture;
sampler TextureSampler = sampler_state { texture = <Texture>; magfilter = LINEAR; minfilter = LINEAR; mipfilter=LINEAR; AddressU = mirror; AddressV = mirror;};

//------- Technique: Textured --------

VertexShaderOutput TexturedVS(VertexShaderInput input)
{
    VertexShaderOutput output = (VertexShaderOutput)0;

	float4x4 preViewProjection = mul(View, Projection);
	float4x4 preWorldViewProjection = mul(World, preViewProjection);

	output.Position = mul(input.Position, preWorldViewProjection);
	output.TextureCoords = input.TexCoords;

	float3 Normal = normalize(mul(normalize(input.Normal), World));
	output.LightingFactor = 1;
	if (EnableLighting)
		output.LightingFactor = dot(input.Normal, -LightDirection);

	return output;
}

float4 TexturedPS(VertexShaderOutput input) : COLOR0
{
    float4 output = tex2D(TextureSampler, input.TextureCoords);
	output.rgb *= saturate(input.LightingFactor) + Ambient;

	return output;
}

technique Textured
{
    pass Pass1
    {
        // TODO: set renderstates here.

        VertexShader = compile vs_2_0 TexturedVS();
        PixelShader = compile ps_2_0 TexturedPS();
    }
}
