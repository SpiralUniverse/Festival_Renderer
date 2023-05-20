#version 330 core

uniform mat4 model;
uniform mat4 view;
uniform mat4 proj;

layout(location = 0) in vec3 in_position;
layout(location = 1) in vec3 in_normal;
layout(location = 2) in vec2 in_texCoord;

out vec2 texCoord;
out vec3 normal;
out vec3 fragPos;

void main()
{
    gl_Position = vec4(in_position, 1)  * model * view * proj;
    fragPos = vec3(model * vec4(in_position, 1));
    normal = in_normal * mat3(transpose(inverse(model)));
    texCoord = in_texCoord;
}