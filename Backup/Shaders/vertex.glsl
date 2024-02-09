#version 330 core
layout (location = 0) in vec3 aPosition;
vec3 aColor = vec3(1f, 1f,1f);

out vec3 ourColor;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;


void main()
{
    gl_Position = vec4(aPosition, 1.0) * model * view ;

    // Enviando a cor do vertex para o fragment shader
    ourColor = aColor;
}