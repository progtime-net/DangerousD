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

float4 MainPS(VertexShaderOutput input) : COLOR
{
    float blurDistanceX = 0.001;//1 / 1366.0;
    float blurDistanceY = blurDistanceX; //1 / 768.0;
    float3 color = tex2D(SpriteTextureSampler, input.TextureCoordinates.xy).rgb;
    
    color =  tex2D(SpriteTextureSampler, float2(input.TextureCoordinates.x, input.TextureCoordinates.y));
    int k = 2;
    for (int i = -k; i < k+1; i++)
    {
        for (int j = -k; j < k + 1; j++)
        {
            color += tex2D(SpriteTextureSampler, float2(input.TextureCoordinates.x + i * blurDistanceX, input.TextureCoordinates.y + j * blurDistanceY));
        }
    }
    //color = tex2D(SpriteTextureSampler, float2(input.TextureCoordinates.x + blurDistance, input.TextureCoordinates.y + blurDistance));
    //color += tex2D(SpriteTextureSampler, float2(input.TextureCoordinates.x - blurDistance, input.TextureCoordinates.y - blurDistance));
    //color += tex2D(SpriteTextureSampler, float2(input.TextureCoordinates.x + blurDistance, input.TextureCoordinates.y - blurDistance));
    //color += tex2D(SpriteTextureSampler, float2(input.TextureCoordinates.x - blurDistance, input.TextureCoordinates.y + blurDistance)); 
    color = color / ((2 * k + 1) * (2 * k + 1) + 1); // - tex2D(SpriteTextureSampler, float2(input.TextureCoordinates.x , input.TextureCoordinates.y )).xyz;
    color -= tex2D(SpriteTextureSampler, float2(input.TextureCoordinates.x, input.TextureCoordinates.y));
    color *= 1.52;
    color.r += tex2D(SpriteTextureSampler, float2(input.TextureCoordinates.x + 5 * blurDistanceX, input.TextureCoordinates.y)); 
    
    float bord = 0.95;
    if (color.r < bord)
        color.r = tex2D(SpriteTextureSampler, input.TextureCoordinates.xy).r;
    if (color.g < bord)
        color.g = tex2D(SpriteTextureSampler, input.TextureCoordinates.xy).g;
    if (color.b < bord)
        color.b = tex2D(SpriteTextureSampler, input.TextureCoordinates.xy).b;
        //color += 0.5 * tex2D(SpriteTextureSampler, input.TextureCoordinates.xy).rgb;
    return float4(color, 1);  
}
technique Blur
{
    pass P0
    {
        PixelShader = compile PS_SHADERMODEL MainPS();
    }
};