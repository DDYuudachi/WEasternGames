using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(AnimeRenderer), PostProcessEvent.BeforeStack, "Custom/Anime")]
public sealed class Anime : PostProcessEffectSettings
{
    [Range(0f, 1f), Tooltip("Slider")]
    public FloatParameter slider = new FloatParameter { value = 0.5f };
}
 
public sealed class AnimeRenderer : PostProcessEffectRenderer<Anime>
{
    public override void Render(PostProcessRenderContext context)
    {
        var sheet = context.propertySheets.Get(Shader.Find("Anime/AnimePostProcessor"));
        sheet.properties.SetFloat("_Slider", settings.slider);
        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}
