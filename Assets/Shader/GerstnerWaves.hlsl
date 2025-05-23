void GerstnerWaves_float(
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
    float phi = k * dot(D, position.xz)
                - speed * k * time
                + phase;

    float cosPhi = cos(phi);
    float sinPhi = sin(phi);

    // desplazamiento sin steepness
    offset.x = amplitude * D.x * cosPhi;
    offset.y = amplitude * sinPhi;
    offset.z = amplitude * D.y * cosPhi;
    
    // tangente
    float3 tangent = float3(
        1.0 - amplitude * k * D.x * D.x * sinPhi,
        amplitude * k * D.x * cosPhi,
       -amplitude * k * D.x * D.y * sinPhi
    );

    // binormal
    float3 binormal = float3(
       -amplitude * k * D.x * D.y * sinPhi,
        amplitude * k * D.y * cosPhi,
        1.0 - amplitude * k * D.y * D.y * sinPhi
    );

    normal = normalize(cross(binormal, tangent));
}