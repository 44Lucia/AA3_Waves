void SinusoidalWaves_float(
    float3 position,
    float2 direction,
    float amplitude,
    float wavelength,
    float speed,
    float time,
    float phase,
    out float3 offset,
    out float3 normal)
{
    float2 D = normalize(direction);
    float k = 2.0 * 3.14159265 / wavelength;
    float phi = k * dot(D, position.xz) - speed * k * time + phase;

    offset = float3(0.0, amplitude * sin(phi), 0.0);

    float dY = amplitude * k * cos(phi);
    float3 tangent = float3(1.0, dY * D.x, 0.0);
    float3 binormal = float3(0.0, dY * D.y, 1.0);

    normal = normalize(cross(binormal, tangent));
}