#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

Texture2D SpriteTexture;

sampler2D SpriteTextureSampler = sampler_state
{
    Texture = <SpriteTexture>;
};

struct VertexShaderOutput
{
    float4 Position : SV_POSITION;
    float4 Color : COLOR0;
    float2 TextureCoordinates : TEXCOORD0;
};

// Bloom settings
float BloomThreshold = 0.8f; // Adjust this threshold to control bloom intensity
float BloomIntensity = 1.5f; // Adjust this value to control bloom intensity

float4 MainPS(VertexShaderOutput input) : COLOR
{
    // Sample the original texture
    float4 col = tex2D(SpriteTextureSampler, input.TextureCoordinates) * input.Color;

    // Apply bloom effect
    float4 bloom = 0;

    // Check if the pixel's brightness is above the threshold
    if (col.r > BloomThreshold)
    {
        // Calculate bloom intensity based on brightness
        bloom = (col - BloomThreshold) * BloomIntensity;

        // Clamp the bloom to prevent overblowing
        bloom = saturate(bloom);
    }

    // Add the bloom to the original color
    col += bloom;

    return col;
}

technique SpriteDrawing
{
    pass P0
    {
        PixelShader = compile PS_SHADERMODEL MainPS();
    }
};
