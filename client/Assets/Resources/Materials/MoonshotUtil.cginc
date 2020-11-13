float remap (float input, float2 inMinMax, float2 outMinMax)
{
    return outMinMax.x + (input - inMinMax.x) * (outMinMax.y - outMinMax.x) / (inMinMax.y - inMinMax.x);
}

float fresnelEffect (float3 normal, float3 viewDir, float power)
{
    return pow((1 - saturate(dot(normalize(normal), normalize(viewDir)))), power);
}

float atmosphereParameters (float alpha, float power, float multiply, float sub)
{
    return (pow(alpha, power) - 1) * multiply + 1 - sub;
}
