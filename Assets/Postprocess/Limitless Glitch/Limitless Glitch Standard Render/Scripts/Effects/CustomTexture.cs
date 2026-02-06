using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(LimitlessGlitch_GlitchCustomTexture), PostProcessEvent.BeforeStack, "Limitless Glitch/CustomTexture", false)]
public sealed class CustomTexture : PostProcessEffectSettings
{
    public TextureParameter texture = new TextureParameter();
    [Range(0f, 1f), Tooltip("Effect alpha.")]
    public FloatParameter threshold = new FloatParameter { value = 0.55f };

}

public sealed class LimitlessGlitch_GlitchCustomTexture : PostProcessEffectRenderer<CustomTexture>
{
    public override void Render(PostProcessRenderContext context)
    {
        var sheet = context.propertySheets.Get(Shader.Find("Limitless/CustomTexture"));
        if (settings.texture.value != null)
            sheet.properties.SetTexture("_CustomTex", settings.texture.value);
        sheet.properties.SetFloat("_thresh", settings.threshold.value);
        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}