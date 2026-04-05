#version 330 core
uniform vec4 uColor;
uniform int uMode;
uniform sampler2D textTexture;
in vec2 TexCoord;
out vec4 fragColor;

void main()
{
    if (uMode == 0)
    {
        fragColor = uColor;
    } 
    else
    {
        float sampled = texture(textTexture, TexCoord).r * uColor.w;
        fragColor = vec4(uColor.x, uColor.y, uColor.z, sampled);
    }
}
