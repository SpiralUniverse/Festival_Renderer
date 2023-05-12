#version 330 core
#define NR_POINT_LIGHTS 4

struct Material 
{
    vec4 color;
    sampler2D diffuse;
    sampler2D specular;
    float shininess;
};

struct DirLight
{
    vec3 direction;
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
    vec3 color;
};

struct PointLight
{
    vec3 position;
    
    float constant;
    float linear;
    float quadratic;
    
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
    
    vec3 color;
};
  
uniform Material material;
uniform DirLight dirLight;
uniform PointLight pointLights[NR_POINT_LIGHTS];

uniform int isUsingTexture;

in vec2 texCoord;

in vec3 normal;
in vec3 fragPos;

out vec4 outputColor;

uniform sampler2D texture0;
uniform vec3 viewPos;



vec3 CalculateDirLight(DirLight light, vec3 normal, vec3 viewDir);
vec3 CalculatePointLight(PointLight light, vec3 normal, vec3 fragPos, vec3 viewDir);
void main()
{
    vec3 norm = normalize(normal);
    vec3 viewDir = normalize(viewPos - fragPos);
    
    vec3 result = CalculateDirLight(dirLight, norm, viewDir);
    
    for(int i = 0; i < NR_POINT_LIGHTS; i++)
    {
        result += CalculatePointLight(pointLights[i], norm, fragPos, viewDir);
    }
    
    outputColor = vec4(result, 1) * material.color;
}

vec3 CalculateDirLight(DirLight light, vec3 normal, vec3 viewDir)
{
    vec3 lightDir = normalize(-light.direction);
    
    float diff = max(dot(normal, lightDir), 0.0);
    
    vec3 reflectDir = reflect(-lightDir, normal);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
    
    if(isUsingTexture == 1)
    {
        vec3 ambient = light.ambient * vec3(texture(material.diffuse, texCoord));
        vec3 diffuse = light.diffuse * diff * vec3(texture(material.diffuse, texCoord));
        vec3 specular = light.specular * spec * vec3(texture(material.specular, texCoord));
        return (ambient + diffuse + specular) * light.color;
    }
    else
    {
        vec3 ambient = light.ambient * material.color.xyz;
        vec3 diffuse = light.diffuse * diff * material.color.xyz;
        vec3 specular = light.specular * spec * material.color.xyz;
        return (ambient + diffuse + specular) * light.color;
    }
    
    
}

vec3 CalculatePointLight(PointLight light, vec3 normal, vec3 fragPos, vec3 viewDir)
{
    vec3 lightDir = normalize(light.position - fragPos);
    
    float diff = max(dot(normal, lightDir), 0.0);
    
    vec3 reflectDir = reflect(-lightDir, normal);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
    
    float distance = length(light.position - fragPos);
    float attenuation = 1.0 / (light.constant + light.linear * distance + light.quadratic * pow(distance, 2.0));

    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
    
    if(isUsingTexture == 1)
    {
        ambient = light.ambient * vec3(texture(material.diffuse, texCoord));
        diffuse = light.diffuse * diff * vec3(texture(material.diffuse, texCoord));
        specular = light.specular * spec * vec3(texture(material.specular, texCoord));
    }
    else
    {
        ambient = light.ambient * material.color.xyz;
        diffuse = light.diffuse * diff * material.color.xyz;
        specular = light.specular * spec * material.color.xyz;
    }
    
    ambient *= attenuation;
    diffuse *= attenuation;
    specular *= attenuation;
    
    return ( ambient + diffuse + specular) * light.color;
}