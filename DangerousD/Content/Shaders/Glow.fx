#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif


Texture2D SpriteTexture;
uniform float time;

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

float4 Distortion(VertexShaderOutput input) : COLOR
{
    float blurDistanceX = 0.01 * (sin(time) + sin(2 * time + 1.3) + 0.7 * sin(7 * time + 33.3) + 0.3 * sin(9 * time + 33.3)); //1 / 1366.0;
    blurDistanceX = pow(blurDistanceX, 1.2);
    float blurDistanceY = blurDistanceX ; //1 / 768.0;
    float3 color = tex2D(SpriteTextureSampler, input.TextureCoordinates.xy).rgb;
    
    color = 3*tex2D(SpriteTextureSampler, float2(input.TextureCoordinates.x, input.TextureCoordinates.y));
    
    float n = tex2D(SpriteTextureSampler, input.TextureCoordinates.xy).a;
    
    int k = 2;
    for (int i = -k; i < k + 1; i++)
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
    color = color / ((2 * k + 1) * (2 * k + 1) + 3); // - tex2D(SpriteTextureSampler, float2(input.TextureCoordinates.x , input.TextureCoordinates.y )).xyz;
    color *= 0.8;
    color += 0.5 * tex2D(SpriteTextureSampler, float2(input.TextureCoordinates.x, input.TextureCoordinates.y));
    color.r = tex2D(SpriteTextureSampler, float2(input.TextureCoordinates.x + 1 * blurDistanceX, input.TextureCoordinates.y)).r - color.r; 
    if (n < 0.001 && color.r < 0.6)
        return float4(0, 0, 0, 0);
    //color.r += tex2D(SpriteTextureSampler, float2(input.TextureCoordinates.x + 5 * blurDistanceX, input.TextureCoordinates.y)); 
    
    //float bord = 0.95;
    //if (color.r < bord)
    //    color.r = tex2D(SpriteTextureSampler, input.TextureCoordinates.xy).r;
    //if (color.g < bord)
    //    color.g = tex2D(SpriteTextureSampler, input.TextureCoordinates.xy).g;
    //if (color.b < bord)
    //    color.b = tex2D(SpriteTextureSampler, input.TextureCoordinates.xy).b;
        //color += 0.5 * tex2D(SpriteTextureSampler, input.TextureCoordinates.xy).rgb;
    
    float3 color2 = tex2D(SpriteTextureSampler, input.TextureCoordinates.xy).rgb;
    color = color2 + (color - color2) * pow((sin(time * 0.1) + sin(2 * time * 0.1 + 1.3) + 0.7 * sin(7 * time * 0.1 + 33.3) + 0.3 * sin(9 * time * 0.1 + 33.3)), 2);
    color = color2 * 0.7 + color * 0.3;
    
    color *= 0.9 + 0.1 * round(sin(input.TextureCoordinates.y * 3.14 * 64 - 4 * time));
    
    color *= 1 - pow(1.5 * 2 * abs(input.TextureCoordinates.x - 0.5), 4);
    
    
    return float4(color, n * 0.95);
}

float4 MainPS2(VertexShaderOutput input) : COLOR
{
    //float blurDistanceX = 0.001; //1 / 1366.0;
    //float blurDistanceY = blurDistanceX; //1 / 768.0;
    float3 color = tex2D(SpriteTextureSampler, input.TextureCoordinates.xy).rgb;
    color *= 0.25;
    //color = tex2D(SpriteTextureSampler, float2(input.TextureCoordinates.x, input.TextureCoordinates.y));
    //int k = 2;
    //for (int i = -k; i < k + 1; i++)
    //{
    //    for (int j = -k; j < k + 1; j++)
    //    {
    //        color += tex2D(SpriteTextureSampler, float2(input.TextureCoordinates.x + i * blurDistanceX, input.TextureCoordinates.y + j * blurDistanceY));
    //    }
    //} 
    //color += tex2D(SpriteTextureSampler, float2(input.TextureCoordinates.x, input.TextureCoordinates.y));
    //color *= 2;
    return float4(color, 0.05);
}
float4 MainPS3(VertexShaderOutput input) : COLOR
{
    float blurDistanceX = 0.0005; //1 / 1366.0;
    float blurDistanceY = blurDistanceX; //1 / 768.0;
    float3 color = tex2D(SpriteTextureSampler, input.TextureCoordinates.xy).rgb;
    int k = 15;
    for (int i = -k; i < k + 1; i++)
    {
        for (int j = -k; j < k + 1; j++)
        {
            color += tex2D(SpriteTextureSampler, float2(input.TextureCoordinates.x + i * blurDistanceX, input.TextureCoordinates.y + j * blurDistanceY));
        }
    }
    color = color / ((2 * k + 1) * (2 * k + 1) + 1);
    //color -= 1*tex2D(SpriteTextureSampler, float2(input.TextureCoordinates.xy));
    int pw = 15;
    color = pow(color, float3(pw, pw, pw));
    float boundry = 0.1;
    if (color.r < boundry)
        color.r = 0;
    if (color.g < boundry)
        color.g = 0;
    if (color.b < boundry)
        color.b = 0;
    return float4(color, 0.21);
}

float4 Dark(VertexShaderOutput input) : COLOR
{
    float blurDistanceX = 0.05; //1 / 1366.0;
    float blurDistanceY = blurDistanceX; //1 / 768.0;
    float3 color = tex2D(SpriteTextureSampler, input.TextureCoordinates.xy).rgb;
    int k = 0;
    for (int i = -k; i < k + 1; i++)
    {
        for (int j = -k; j < k + 1; j++)
        {
            color += tex2D(SpriteTextureSampler, float2(input.TextureCoordinates.x + i * blurDistanceX, input.TextureCoordinates.y + j * blurDistanceY));
        }
    }
    //color = color / ((2 * k + 1) * (2 * k + 1) + 2);
    //color -= 1 * tex2D(SpriteTextureSampler, float2(input.TextureCoordinates.xy));
    int pw = 10;
    color = pow(color, float3(pw /2, pw / 2, pw / 2));
    return float4(color, tex2D(SpriteTextureSampler, input.TextureCoordinates.xy).a);
}
float4 MainPS_HighLightPlayer(VertexShaderOutput input) : COLOR
{
    float blurDistanceX = 0.05; //1 / 1366.0;
    float blurDistanceY = blurDistanceX; //1 / 768.0;
    float4 color = tex2D(SpriteTextureSampler, input.TextureCoordinates.xy);
    color.r = time;
    color.g = 1;
    if (color.a == 0)
        return float4(0, 0, 0, 0);
    return color;
}
float4 Red(VertexShaderOutput input) : COLOR
{
    float blurDistanceX = 0.05; //1 / 1366.0;
    float blurDistanceY = blurDistanceX; //1 / 768.0;
    float3 color = tex2D(SpriteTextureSampler, input.TextureCoordinates.xy);
    color.r *= 2; 
    if (tex2D(SpriteTextureSampler, input.TextureCoordinates.xy).a == 0)
        return float4(0, 0, 0, 0);
    int pw = 10;
    color = pow(color, float3(pw / 2, pw / 2, pw / 2));
    return float4(color, tex2D(SpriteTextureSampler, input.TextureCoordinates.xy).a);
}

technique Distortion
{
    pass P0
    {
        PixelShader = compile PS_SHADERMODEL Distortion();
    }                                        
};                                           
technique Blur2                              
{                                            
    pass P0                                  
    {                                        
        PixelShader = compile PS_SHADERMODEL MainPS2();
    }                                        
};                                           
technique Blur3                              
{                                         
    pass P0                                 
    {                                        
        PixelShader = compile PS_SHADERMODEL MainPS3();
    }
};                                       
technique Dark
{
    pass P0
    {
        PixelShader = compile PS_SHADERMODEL Dark();
    }
};                               
technique Yellow
{
    pass P0
    {
        PixelShader = compile PS_SHADERMODEL MainPS_HighLightPlayer();
    }
};                         
technique Red
{
    pass P0
    {
        PixelShader = compile PS_SHADERMODEL Red();
    }
};