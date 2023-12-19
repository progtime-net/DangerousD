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
    float3 myPosition : TEXCOORD1;
};

float4x4 MatrixTransform;
matrix World;

float4 MainPS(VertexShaderOutput input) : COLOR
{
    float blurDistanceX = 1 / 1366.0; //1 / 1366.0;
    float blurDistanceY = blurDistanceX; //1 / 768.0;
    float3 color = float3(input.myPosition.xy, 0); //tex2D(SpriteTextureSampler, input.myPosition.xy).rgb;
    //float2 ps = input.Position.xy;
    //float3 color23 = tex2D(SpriteTextureSampler, ps).rgb;
    //color = tex2D(SpriteTextureSampler, float2(input.TextureCoordinates.r + 0 * 1 / 367.0, input.TextureCoordinates.y));
    
    float n = tex2D(SpriteTextureSampler, input.TextureCoordinates.xy).a;
        int k = 0;
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
    color = color / ((2 * k + 1) * (2 * k + 1) + 1); // - tex2D(SpriteTextureSampler, float2(input.TextureCoordinates.x , input.TextureCoordinates.y )).xyz;
    //color *= 0.8;
    //color += 0.5 * tex2D(SpriteTextureSampler, float2(input.TextureCoordinates.x, input.TextureCoordinates.y));
    //color.r = tex2D(SpriteTextureSampler, float2(input.TextureCoordinates.x + 1 * blurDistanceX, input.TextureCoordinates.y)).r - color.r; 
    //if (n < 0.001 && color.r < 0.6)
    //    return float4(0, 0, 0, 0);
    //color.r += tex2D(SpriteTextureSampler, float2(input.TextureCoordinates.x + 5 * blurDistanceX, input.TextureCoordinates.y)); 
    
    //float bord = 0.95;
    //if (color.r < bord)
    //    color.r = tex2D(SpriteTextureSampler, input.TextureCoordinates.xy).r;
    //if (color.g < bord)
    //    color.g = tex2D(SpriteTextureSampler, input.TextureCoordinates.xy).g;
    //if (color.b < bord)
    //    color.b = tex2D(SpriteTextureSampler, input.TextureCoordinates.xy).b;
        //color += 0.5 * tex2D(SpriteTextureSampler, input.TextureCoordinates.xy).rgb;
    
    if (n == 0)
    {
        if (round((input.TextureCoordinates.x - 2 * 1 / 367.0) * 367.0) % 25 != 0
            &&
            round((input.TextureCoordinates.x - 1 * 1 / 367.0) * 367.0) % 25 != 0
            &&
            round((input.TextureCoordinates.x - 3 * 1 / 367.0) * 367.0) % 25 != 0
            &&
            tex2D(SpriteTextureSampler, float2(input.TextureCoordinates.x - 2 * 1 / 367.0, input.TextureCoordinates.y)).a != 0)
        {
            color = float3(0, 1, 0);
            n = tex2D(SpriteTextureSampler, float2(input.TextureCoordinates.x - 2 * 1 / 367.0, input.TextureCoordinates.y)).a;
        }
    }
    float4 color4 = tex2D(SpriteTextureSampler, input.TextureCoordinates.xy);
    //float3(input.myPosition.xy, 0);
    color4 *= 1 - pow(length(input.myPosition.x - float2(0.0, 0.0)), 4);
    return color4;
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
    float3 color = tex2D(SpriteTextureSampler, input.TextureCoordinates.xy).rgb;
    
    
    float pw = 0.5;
    color *= float3(pw, pw, pw);
    
    //float4 color4 = tex2D(SpriteTextureSampler, input.TextureCoordinates.xy);
    color *=  1 - pow(length(input.myPosition.x - float2(0.0, 0.0)), 4);
    return float4(color, tex2D(SpriteTextureSampler, input.TextureCoordinates.xy).a);
}
float4 LightedObject_PS(VertexShaderOutput input) : COLOR
{
    float blurDistanceX = 0.05; //1 / 1366.0;
    float blurDistanceY = blurDistanceX; //1 / 768.0;
     
    
    //tex2D(SpriteTextureSampler, input.TextureCoordinates.xy);
    //color.r = 1;
    //color.g = 1;
    //if (color.a == 0)
    //{
        
    //    blurDistanceX = 1 / 367.0; //1 / 1366.0;
    //    blurDistanceY = blurDistanceX;
    //    int k = 4;
    //    for (int i = -k; i < k + 1; i++)
    //    {
    //        for (int j = -k; j < k + 1; j++)
    //        {
    //            color += tex2D(SpriteTextureSampler, float2(input.TextureCoordinates.x + i * blurDistanceX, input.TextureCoordinates.y + j * blurDistanceY));
    //        }
    //    }
    //    color = color / ((2 * k + 1) * (2 * k + 1) + 1);
    //}
        //if (color.a == 0)
        //    return float4(0, 0, 0, 0); 
    
    float4 color4 = tex2D(SpriteTextureSampler, input.TextureCoordinates.xy);
    //float3(input.myPosition.xy, 0);
    color4 *= 1 - pow(length(input.myPosition.x - float2(0.0, 0.0)), 4);
    return color4;
}


float4 Blur(VertexShaderOutput input) : COLOR
{ 
    
    float4 color = tex2D(SpriteTextureSampler, input.TextureCoordinates.xy);
    //color.r = 1;
    //color.g = 1;
    if (color.a == 0)
    {
        
        float blurDistanceX = 5 / 1366.0;
        float blurDistanceY = blurDistanceX;
        int k = 10;
        int cel = 1;
        for (int i = -k; i < k + 1; i++)
        {
            for (int j = -k; j < k + 1; j++)
            {
                if (i * i + j * j < 100)
                {
                    cel ++;
                    color += tex2D(SpriteTextureSampler, float2(input.TextureCoordinates.x + i * blurDistanceX, input.TextureCoordinates.y + j * blurDistanceY));
                }
            }
        }
        color = color / cel; 
        
        //color = pow(color * 2, 2);
        color.r = color.a;
        color.g = color.a;
        color.b = color.a/5;

    }
    if (color.a == 0)
        return float4(0, 0, 0, 0); 
    return color;
}

float4 MainPS_HighLightPlayer(VertexShaderOutput input) : COLOR
{
    float blurDistanceX = 0.05; //1 / 1366.0;
    float blurDistanceY = blurDistanceX; //1 / 768.0;
    
    float4 color = input.Color;
    
    //tex2D(SpriteTextureSampler, input.TextureCoordinates.xy);
    //color.r = 1;
    //color.g = 1;
    if (color.a == 0)
    {
        
        blurDistanceX = 1 / 367.0; //1 / 1366.0;
        blurDistanceY = blurDistanceX;
        int k = 4;
        for (int i = -k; i < k + 1; i++)
        {
            for (int j = -k; j < k + 1; j++)
            {
                color += tex2D(SpriteTextureSampler, float2(input.TextureCoordinates.x + i * blurDistanceX, input.TextureCoordinates.y + j * blurDistanceY));
            }
        }
        color = color / ((2 * k + 1) * (2 * k + 1) + 1);
    }
        //if (color.a == 0)
        //    return float4(0, 0, 0, 0);
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


VertexShaderOutput MainVS(float4 position : SV_POSITION, float4 color : COLOR0, float2 texCoord : TEXCOORD0)
{
    VertexShaderOutput output;
    output.Position = mul(position, MatrixTransform);
    output.myPosition = mul(position, MatrixTransform);
    output.Color = color;
    output.TextureCoordinates = texCoord;
    return output;
} 
                                          
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
        VertexShader = compile VS_SHADERMODEL MainVS();
        PixelShader = compile PS_SHADERMODEL Dark();
    }
};                               
technique Yellow
{
    pass P0
    {
        PixelShader = compile PS_SHADERMODEL LightedObject_PS();
    }
};                            
technique Blur
{
    pass P0
    {
        PixelShader = compile PS_SHADERMODEL Blur();
    }
};                         
technique Red
{
    pass P0
    {
        PixelShader = compile PS_SHADERMODEL Red();
    }
};