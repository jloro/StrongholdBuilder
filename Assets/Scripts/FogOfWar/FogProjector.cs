using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogProjector : MonoBehaviour
{
	public Material projectorMaterial;
	public float blendSpeed;
	public int textureScale;

	public CustomRenderTexture fogTexture;

	private RenderTexture prevTexture;
	private RenderTexture currTexture;
	private Projector projector;

	private float blendAmount;

	private void Awake()
	{
		projector = GetComponent<Projector>();
		projector.enabled = true;

		prevTexture = GenerateTexture();
		currTexture = GenerateTexture();

		// Projector materials aren't instanced, resulting in the material asset getting changed.
		// Instance it here to prevent us from having to check in or discard these changes manually.
		projector.material = new Material(projectorMaterial);

		projector.material.SetTexture("_PrevTexture", prevTexture);
		projector.material.SetTexture("_CurrTexture", currTexture);

		StartNewBlend();
	}

	CustomRenderTexture GenerateTexture()
	{
		CustomRenderTexture crt = new CustomRenderTexture(
			fogTexture.width * textureScale,
			fogTexture.height * textureScale,
			fogTexture.format
		)
		{ antiAliasing = 8, depth = 0, filterMode = FilterMode.Bilinear };
		crt.antiAliasing = 8;
		crt.anisoLevel = fogTexture.anisoLevel;

		return crt;
	}

	public void StartNewBlend()
	{
		StopCoroutine(BlendFog());
		blendAmount = 0;
		// Swap the textures
		Graphics.Blit(currTexture, prevTexture);
		Graphics.Blit(fogTexture, currTexture);

		StartCoroutine(BlendFog());
	}

	IEnumerator BlendFog()
	{
		while (blendAmount < 1)
		{
			// increase the interpolation amount
			blendAmount += Time.deltaTime * blendSpeed;
			// Set the blend property so the shader knows how much to lerp
			// by when checking the alpha value
			projector.material.SetFloat("_Blend", blendAmount);
			yield return null;
		}
		// once finished blending, swap the textures and start a new blend
		StartNewBlend();
	}
}