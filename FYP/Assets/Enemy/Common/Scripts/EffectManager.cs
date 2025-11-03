using System;
using System.Collections;
using UnityEngine;

namespace Assets.PixelFantasy.Common.Scripts
{
    [CreateAssetMenu(fileName = "EffectManager", menuName = "Pixel Heroes/EffectManager")]
    public class EffectManager : ScriptableObject
    {
        public SpriteEffect SpriteEffectPrefab;
        public AudioClip FireAudioClip;

        private static Material _baseMaterial;
        private static Material _blinkMaterial;

        public static EffectManager Instance;

        [RuntimeInitializeOnLoadMethod]
        static void Initialize()
        {
            Instance = Resources.Load<EffectManager>("EffectManager");
        }

        public void Blink(Creature creature)
        {
            if (_baseMaterial == null) _baseMaterial = creature.Body.sharedMaterial;
            if (_blinkMaterial == null) _blinkMaterial = new Material(Shader.Find("GUI/Text Shader"));

            creature.StartCoroutine(BlinkCoroutine(creature));
        }

        private IEnumerator BlinkCoroutine(Creature creature)
        {
            creature.Body.material = _blinkMaterial;

            yield return new WaitForSeconds(0.1f);

            creature.Body.material = _baseMaterial;
        }

        public SpriteEffect CreateSpriteEffect(Creature creature, string clipName, int direction = 0, Transform parent = null)
        {
            var instance = Instantiate(SpriteEffectPrefab, creature.transform.position, Quaternion.identity, parent);

            instance.name = clipName;
            instance.transform.position = parent == null ? creature.transform.position : parent.transform.position;
            instance.GetComponent<SpriteRenderer>().sortingOrder = creature.Body.sortingOrder + 1;
            instance.Play(clipName, direction == 0 ? Math.Sign(creature.transform.localScale.x) : direction);

            return instance;
        }
    }
}