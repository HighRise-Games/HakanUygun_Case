using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace RubyGames.Framework.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class ProgressBarShineEffect : MonoBehaviour
    {
        [Header("Positions")]
        [SerializeField] private float start = -320f;
        [SerializeField] private float end = 320f;

        [Header("Behaviour")] 
        [SerializeField] private float traverseDuration = 0.75f;
        [SerializeField] private float loopDelay = 0.15f; 

        private RectTransform _rectTransform;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        private void Start()
        {
            Shine().AttachExternalCancellation(this.GetCancellationTokenOnDestroy());
        }
        
        private async UniTask Shine()
        {
            var timer = 0f;
            var position = _rectTransform.anchoredPosition;
            while (true)
            {
                timer += Time.deltaTime;
                var currentPoint = Mathf.Lerp(start, end, timer / traverseDuration);
                position.x = currentPoint;
                _rectTransform.anchoredPosition = position;

                await UniTask.Yield();

                if (!(timer >= traverseDuration)) continue;
                timer = 0f;
                await UniTask.Delay(TimeSpan.FromSeconds(loopDelay));
            }
            // ReSharper disable once FunctionNeverReturns
        }
    }
}
