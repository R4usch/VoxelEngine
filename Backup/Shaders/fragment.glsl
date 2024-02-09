#version 330 core

// Recebe a cor de entrada
in vec3 ourColor;

// Cor de saida
out vec4 FragColor;

void main()
{                
    FragColor = vec4(ourColor,1);
}