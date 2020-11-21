float remap (float input, float2 inMinMax, float2 outMinMax)
{
    return outMinMax.x + (input - inMinMax.x) * (outMinMax.y - outMinMax.x) / (inMinMax.y - inMinMax.x);
}

float fresnelEffect (float3 normal, float3 viewDir, float featherStrength, float featherSize)
{
    return pow((1.0 - pow(saturate(dot(normalize(normal), normalize(viewDir))), featherSize)), featherStrength);
}

float3 blendOverlay (float3 Base, float3 Blend, float Opacity)
{
    float3 result1 = 1.0 - 2.0 * (1.0 - Base) * (1.0 - Blend);
    float3 result2 = 2.0 * Base * Blend;
    float3 zeroOrOne = step(Base, 0.5);
    float3 Out = result2 * zeroOrOne + (1 - zeroOrOne) * result1;
    Out = lerp(Base, Out, Opacity);
    return Out;
}

float3 blendMultiply (float3 Base, float3 Blend, float Opacity)
{
    return lerp(Base, Base * Blend, Opacity);
}

float atmosphereParameters (float alpha, float power, float multiply, float sub)
{
    return (pow(alpha, power) - 1.0) * multiply + 1.0 - sub;
}
