#version 330 core

struct Material {
    
    sampler2D diffuse;
    sampler2D specular;
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
    vec3 ambient = light.ambient * vec3(texture(material.diffuse, texCoord));
    
    //difuse
    vec3 norm = normalize(normal);
    vec3 lightDir = normalize(light.position - fragPos);
    float diff = max(dot(norm, lightDir), 0.0);
    vec3 diffuse = light.diffuse * (diff * vec3(texture(material.diffuse, texCoord)));
    
    //specular
    vec3 viewDir = normalize(viewPos - fragPos);
    vec3 reflectionDir = reflect(-lightDir, norm);
    float spec = pow(max(dot(viewDir, reflectionDir), 0.0), material.shininess);
    vec3 specular = light.specular * (spec * vec3(texture(material.specular, texCoord)));
    
    vec3 result = ambient + diffuse + specular;
    
    outputColor = vec4(result, 1) * objectColor;
}