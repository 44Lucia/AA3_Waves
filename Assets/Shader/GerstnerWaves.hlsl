void GerstnerWaves_float(
    float3 position,
    float2 direction,
    float amplitude,
    float wavelength,
    float speed,
    float steepness,
    float time,
    float phase,
    out float3 offset,
    out float3 normal)
{
    float2 D = normalize(direction);
    float k = 2.0 * 3.14159265 / wavelength;
    float phi = k * dot(D, position.xz) - speed * k * time + phase;

    float cosPhi = cos(phi);
    float sinPhi = sin(phi);

    offset.x = steepness * amplitude * D.x * cosPhi;
    offset.z = steepness * amplitude * D.y * cosPhi;
    offset.y = amplitude * sinPhi;

    float3 tangent = float3(
        1.0 - steepness * amplitude * k * D.x * D.x * sinPhi,
        steepness * amplitude * k * D.x * cosPhi,
        -steepness * amplitude * k * D.x * D.y * sinPhi
    );

    float3 binormal = float3(
        -steepness * amplitude * k * D.x * D.y * sinPhi,
        steepness * amplitude * k * D.y * cosPhi,
        1.0 - steepness * amplitude * k * D.y * D.y * sinPhi
    );

    normal = normalize(cross(binormal, tangent));
}