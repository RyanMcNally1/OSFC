using System.Collections;
using UnityEngine;

public class DamageFlash : MonoBehaviour {

    [Header("References")]
    [SerializeField] private Renderer[] targetRenderers;

    [Header("Flash Settings")]
    [SerializeField] private Color flashColor = Color.white;
    [SerializeField] private float flashDuration = 0.1f;
    [SerializeField] private float flashIntensity = 2f;

    private MaterialPropertyBlock propertyBlock;
    private Coroutine flashCoroutine;

    private static readonly int BaseColorID =
        Shader.PropertyToID("_BaseColor");

    private static readonly int ColorID =
        Shader.PropertyToID("_Color");

    private Color[][] originalColors;

    void Awake() {
        if (
            targetRenderers == null ||
            targetRenderers.Length == 0
        ) {
            targetRenderers =
                GetComponentsInChildren<Renderer>(true);
        }

        propertyBlock = new MaterialPropertyBlock();

        StoreOriginalColors();
    }

    void StoreOriginalColors() {
        originalColors =
            new Color[targetRenderers.Length][];

        for (
            int rendererIndex = 0;
            rendererIndex < targetRenderers.Length;
            rendererIndex++
        ) {
            Renderer currentRenderer =
                targetRenderers[rendererIndex];

            Material[] materials =
                currentRenderer.sharedMaterials;

            originalColors[rendererIndex] =
                new Color[materials.Length];

            for (
                int materialIndex = 0;
                materialIndex < materials.Length;
                materialIndex++
            ) {
                Material material =
                    materials[materialIndex];

                if (
                    material != null &&
                    material.HasProperty(BaseColorID)
                ) {
                    originalColors[rendererIndex][materialIndex] =
                        material.GetColor(BaseColorID);
                }
                else if (
                    material != null &&
                    material.HasProperty(ColorID)
                ) {
                    originalColors[rendererIndex][materialIndex] =
                        material.GetColor(ColorID);
                }
                else {
                    originalColors[rendererIndex][materialIndex] =
                        Color.white;
                }
            }
        }
    }

    public void Flash() {
        if (flashCoroutine != null) {
            StopCoroutine(flashCoroutine);
        }

        flashCoroutine =
            StartCoroutine(FlashRoutine());
    }

    IEnumerator FlashRoutine() {
        SetFlashColor(
            flashColor * flashIntensity
        );

        yield return new WaitForSeconds(
            flashDuration
        );

        RestoreOriginalColors();

        flashCoroutine = null;
    }

    void SetFlashColor(Color color) {
        for (
            int rendererIndex = 0;
            rendererIndex < targetRenderers.Length;
            rendererIndex++
        ) {
            Renderer currentRenderer =
                targetRenderers[rendererIndex];

            int materialCount =
                currentRenderer.sharedMaterials.Length;

            for (
                int materialIndex = 0;
                materialIndex < materialCount;
                materialIndex++
            ) {
                currentRenderer.GetPropertyBlock(
                    propertyBlock,
                    materialIndex
                );

                propertyBlock.SetColor(
                    BaseColorID,
                    color
                );

                propertyBlock.SetColor(
                    ColorID,
                    color
                );

                currentRenderer.SetPropertyBlock(
                    propertyBlock,
                    materialIndex
                );
            }
        }
    }

    void RestoreOriginalColors() {
        for (
            int rendererIndex = 0;
            rendererIndex < targetRenderers.Length;
            rendererIndex++
        ) {
            Renderer currentRenderer =
                targetRenderers[rendererIndex];

            for (
                int materialIndex = 0;
                materialIndex <
                originalColors[rendererIndex].Length;
                materialIndex++
            ) {
                currentRenderer.GetPropertyBlock(
                    propertyBlock,
                    materialIndex
                );

                propertyBlock.SetColor(
                    BaseColorID,
                    originalColors[rendererIndex][materialIndex]
                );

                propertyBlock.SetColor(
                    ColorID,
                    originalColors[rendererIndex][materialIndex]
                );

                currentRenderer.SetPropertyBlock(
                    propertyBlock,
                    materialIndex
                );
            }
        }
    }

    void OnDisable() {
        if (targetRenderers == null) {
            return;
        }

        RestoreOriginalColors();
    }
}
