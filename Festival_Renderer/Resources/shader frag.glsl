#version 330 core

struct Material {
    
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;

    float shininess;
};

struct Light{
    vec3 position;
    
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};
  
uniform Material material;
uniform Light light;

in vec2 texCoord;

in vec3 normal;
in vec3 fragPos;

out vec4 outputColor;

uniform sampler2D texture0;

uniform vec3 viewPos;

uniform vec4 objectColor;
uniform vec4 lightColor;

void main()
{
    //ambient
    vec4 ambient = vec4(light.ambient, 1) * vec4(material.ambient, 1);
    
    //difuse
    vec3 norm = normalize(normal);
    vec3 lightDir = normalize(light.position - fragPos);
    float diff = max(dot(norm, lightDir), 0.0);
    vec4 diffuse = vec4(light.diffuse, 1) * (diff * vec4(material.diffuse, 1));
    
    //specular
    vec3 viewDir = normalize(viewPos - fragPos);
    vec3 reflectionDir = reflect(-lightDir, norm);
    float spec = pow(max(dot(viewDir, reflectionDir), 0.0), material.shininess);
    vec4 specular = vec4(light.specular, 1) * (spec * vec4(material.specular, 1));
    
    vec4 result = ambient + diffuse + specular;
    
    //outputColor = result * objectColor * texture(texture0, texCoord); //use to enable textures
    outputColor = result * objectColor;
}