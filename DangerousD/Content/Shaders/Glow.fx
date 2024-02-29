#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif


Texture2D SpriteTexture;
float3 dominantColor;
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
    color *= float3(pw, pw, pw) + dominantColor;
    
    //float4 color4 = tex2D(SpriteTextureSampler, input.TextureCoordinates.xy);
    color *= 1 - pow(length(input.myPosition.x - float2(0.0, 0.0)), 4);
    return float4(color, tex2D(SpriteTextureSampler, input.TextureCoordinates.xy).a);
}
float4 LightedObject_PS(VertexShaderOutput input) : COLOR
{
    float blurDistanceX = 0.05; //1 / 1366.0;
    float blurDistanceY = blurDistanceX; //1 / 768.0;
     
    float4 color4 = tex2D(SpriteTextureSampler, input.TextureCoordinates.xy);
    //float3(input.myPosition.xy, 0);
    color4 *= 1 - pow(length(input.myPosition.x - float2(0.0, 0.0)), 4);
    return color4;
}


float3 ColorGlowColor;
float4 ColorGlow(VertexShaderOutput input) : COLOR
{
    
    float4 color = tex2D(SpriteTextureSampler, input.TextureCoordinates.xy);
    //color.r = 1;
    //color.g = 1;
    if (color.a == 0)
    {
        
        float blurDistanceX = 2 / 1366.0;
        float blurDistanceY = blurDistanceX;
        int k = 10;
        int cel = 1;
        for (int i = -k; i < k + 1; i++)
        {
            for (int j = -k; j < k + 1; j++)
            {
                if (i * i + j * j < (k + 0.5) * (k + 0.5))
                {
                    cel++;
                    color += tex2D(SpriteTextureSampler, float2(input.TextureCoordinates.x + i * blurDistanceX, input.TextureCoordinates.y + j * blurDistanceY));
                }
            }
        }
        color = color / cel;
        
        //color = pow(color * 2, 2);
        color = float4(ColorGlowColor * color.a, color.a);

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
        PixelShader = compile PS_SHADERMODEL LightedObject_PS();
    }
};                            
technique Blur
{
    pass P0
    {
        PixelShader = compile PS_SHADERMODEL ColorGlow();
    }
};                         
technique Red
{
    pass P0
    {
        PixelShader = compile PS_SHADERMODEL Red();
    }
};               




float totalSeconds;
float4x4 MainMatrixTransform;
float mix(float a, float b, float c)
{
    return a + (b - a) * c;
}

VertexShaderOutput MainScreen_VS(float4 position : SV_POSITION, float4 color : COLOR0, float2 texCoord : TEXCOORD0)
{
    VertexShaderOutput output;
    output.Position = mul(position, MainMatrixTransform);
    output.myPosition = mul(position, MainMatrixTransform);
    output.Color = color;
    
    float2 a = 2 * 3.1415 * (texCoord - 0.5);
    output.TextureCoordinates = texCoord;
    return output;
}
float4 MainScreen_PS(VertexShaderOutput input) : COLOR
{ 
    //distortion
    //float distortion = 0.6;
    //float2 ndc_pos = input.TextureCoordinates.xy - 0.5;
    //float2 testVec = ndc_pos.xy / max(abs(ndc_pos.x), abs(ndc_pos.y));
    //float len = max(1.0, length(testVec));
    //ndc_pos *= mix(1.0, mix(1.0, len, max(abs(ndc_pos.x), abs(ndc_pos.y))), distortion);
    //float2 texCoordinate_Screened = float2(ndc_pos.x, ndc_pos.y) * 1 + 0.5;  
    //removed
    float2 texCoordinate_Screened = input.TextureCoordinates.xy;
    
    //set color by coord
    if (texCoordinate_Screened.x > 1 || texCoordinate_Screened.x < 0 || texCoordinate_Screened.y > 1 || texCoordinate_Screened.y < 0)
        return float4(0, 0, 0, 0);
    
    float4 color = tex2D(SpriteTextureSampler, texCoordinate_Screened);
    
    
    //// stripes
    //float u_stripe = 0.5;
    //float stripTile = texCoordinate_Screened.x * mix(10.0, 100.0, u_stripe);
    //float stripFac = 1.0 + 0.25 * u_stripe * (step(0.5, stripTile - float(int(stripTile))) - 0.5);
    
    
    //brightness
    float am = 0.03;
    color = (1 - am) * color + (am) * (sin(3 * totalSeconds));
    
    //my stripes
    float am2 = 0.1;
    color *= (1 - am2) + (am2) * round(sin(texCoordinate_Screened.y * 100 - 3 * totalSeconds) * 1);
    return color;
}

technique MainScreen
{
    pass P0
    {
        VertexShader = compile VS_SHADERMODEL MainScreen_VS();
        PixelShader = compile PS_SHADERMODEL MainScreen_PS();
    }
};
 


VertexShaderOutput Default_VS(float4 position : SV_POSITION, float4 color : COLOR0, float2 texCoord : TEXCOORD0)
{
    VertexShaderOutput output;
    output.Position = mul(position, MatrixTransform);
    output.myPosition = mul(position, MatrixTransform);
    output.Color = color; 
    output.TextureCoordinates = texCoord;
    return output;
}
float4 Default_PS(VertexShaderOutput input) : COLOR
{ 
    return tex2D(SpriteTextureSampler, input.TextureCoordinates.xy);
}
technique Default
{
    pass P0
    { 
        PixelShader = compile PS_SHADERMODEL Default_PS();
    }
};  