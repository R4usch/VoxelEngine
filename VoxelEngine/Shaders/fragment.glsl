#version 330 core
out vec4 FragColor;

// Recebe a cor do vertex
// in vec4 vertexColor;

uniform vec4 ourColor; // Valor global onde recebe a cor e passa para o FragColor

void main()
{                
    FragColor = ourColor;
}