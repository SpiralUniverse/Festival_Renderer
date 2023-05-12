#version 330 core

uniform vec4 color;

uniform vec3 FogColor;
uniform float FogDensity;


out vec4 fragColor;

void main()
{
    float fogFactor = exp(-0.1f * 0.1f * 50 * 50);
    fogFactor = clamp(fogFactor, 0.0, 1.0);
    vec3 fogColor = mix(vec3(0.5), fragColor.rgb, fogFactor);
    fragColor = vec4(fogColor, 1.0);
    fragColor = color;
}
