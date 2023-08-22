using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RubyGames.Framework.UI
{
    public class LoadingSceneHandler : MonoBehaviour
    {
        private CanvasGroup _canvasGroup;
        private CanvasGroup canvasGroup => _canvasGroup ? _canvasGroup : _canvasGroup = GetComponent<CanvasGroup>();

        private ProgressBar _progressBar;
        private ProgressBar progressBar =>
            _progressBar ? _progressBar : _progressBar = GetComponentInChildren<ProgressBar>();

        [SerializeField] private string sceneToLoad;

        private async void Start()
        {
            RubyFramework.frameworkHandler.LoadingProgress.ProgressChanged += UpdateRubyFrameworkProgress;

            await UniTask.WaitUntil(() => RubyFramework.frameworkHandler.IsInitialized);
            await LoadScene();
        }

        private void OnDestroy()
        {
            if (RubyFramework.frameworkHandler == null) return;

            RubyFramework.frameworkHandler.LoadingProgress.ProgressChanged -= UpdateRubyFrameworkProgress;
        }

        private async UniTask LoadScene()
        {
            var loadMode =  LoadSceneMode.Additive;
            var sceneLoadProgression = SceneManager.LoadSceneAsync(sceneToLoad, loadMode);

            var loadingLeft = .3f;
            while (!sceneLoadProgression.isDone)
            {
                progressBar.UpdateProgress(sceneLoadProgression.progress, .5f, .8f);
                await UniTask.Yield();
            }

            //Wait for additional scenes
            await UniTask.DelayFrame(10);

            var isLoadingScene = false;
            do
            {
                isLoadingScene = false;
                var sceneCount = SceneManager.sceneCount;

                for (int i = 0; i < sceneCount; i++)
                {
                    var scene = SceneManager.GetSceneAt(i);
                    if (!scene.isLoaded)
                    {
                        isLoadingScene = true;
                        break;
                    }
                }

                if (isLoadingScene)
                    await UniTask.Yield();
            }
            while (isLoadingScene);

            await Hide();

            SceneManager.UnloadSceneAsync(gameObject.scene);
        }

        private async UniTask Hide()
        {
            while (canvasGroup.alpha > 0)
            {
                canvasGroup.alpha -= Time.unscaledDeltaTime * 2f;
                await UniTask.Yield();
            }
        }

        private void UpdateRubyFrameworkProgress(object sender, float percentage)
        {
            progressBar.UpdateProgress(percentage, 0f, .5f);
        }
    }
}
