#version 330 core
layout (location = 0) in vec2 aPosition;
layout (location = 1) in vec2 aTexCoord;
uniform mat4 uMatrix;
uniform int uMode;
out vec2 TexCoord;

void main()
{
    gl_Position = uMatrix * vec4(aPosition, 0.0, 1.0);
    if (uMode == 1)
    {
        TexCoord = aTexCoord;
    }
}
