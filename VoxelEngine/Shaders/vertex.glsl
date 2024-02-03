#version 330 core
layout (location = 0) in vec3 aPosition;

out vec4 vertexColor;

void main()
{
    gl_Position = vec4(aPosition, 1.0);
    // Enviando a cor do vertex para o fragment shader
    vertexColor = vec4(0f, 0.5f, 0.2f, 1.0f);
}