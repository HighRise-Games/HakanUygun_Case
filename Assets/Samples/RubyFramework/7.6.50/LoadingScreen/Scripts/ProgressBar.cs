using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace RubyGames.Framework.UI
{
    public class ProgressBar : MonoBehaviour
    {
        [Header("Filler")]
        [SerializeField] private Image filler;
        [SerializeField] private float fillDuration = 0.2f;

        private float _currentFillAmount = 0f;
        private Coroutine _coroutine;

        private void Awake()
        {
            _currentFillAmount = 0f;
            filler.fillAmount = _currentFillAmount;
            _coroutine = null;
        }

        public void UpdateProgress(float percentage, float begin, float end)
        {
            _currentFillAmount = Mathf.Lerp(begin, end, percentage);
            _currentFillAmount = _currentFillAmount > 1f
                ? 1f
                : _currentFillAmount < 0f
                ? 0f
                : _currentFillAmount;

            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
            }

            _coroutine = StartCoroutine(Progress());
        }

        private IEnumerator Progress()
        {
            var initialFillAmount = filler.fillAmount;

            var timer = 0f;
            while (timer <= fillDuration)
            {
                var nextFill = Mathf.Lerp(initialFillAmount, _currentFillAmount, timer / fillDuration);
                filler.fillAmount = nextFill;

                yield return null;
                timer += Time.deltaTime;
            }

            filler.fillAmount = _currentFillAmount;
        }
    }
}
