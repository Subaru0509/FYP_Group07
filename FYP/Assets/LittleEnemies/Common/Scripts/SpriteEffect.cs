using UnityEngine;

namespace Assets.PixelFantasy.Common.Scripts
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Animator))]
    public class SpriteEffect : MonoBehaviour
    {
        public void Play(string clipName, int direction = 1)
        {
            transform.localScale = new Vector3(direction, 1, 1);
            GetComponent<Animator>().Play(clipName);
            Destroy(gameObject, 0.25f);
        }
    }
}