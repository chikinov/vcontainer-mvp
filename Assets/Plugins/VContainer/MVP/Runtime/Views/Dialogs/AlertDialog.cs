using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VContainer.Unity.MVP.Async;

namespace VContainer.Unity.MVP
{
    public class AlertDialog : Dialog
    {
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text messageText;
        [SerializeField] private Button positiveButton;
        [SerializeField] private Button negativeButton;
        [SerializeField] private Transform buttonGroup;

        public string Title { set { if (titleText) titleText.text = value; } }

        public string Message { set { if (messageText) messageText.text = value; } }

        public string PositiveText
        {
            set
            {
                SetButtonText(positiveButton, value);

                UpdateButtonGroup();
            }
        }

        public string NegativeText
        {
            set
            {
                SetButtonText(negativeButton, value);

                UpdateButtonGroup();
            }
        }

        private void SetButtonText(Button button, string text)
        {
            if (!button) return;

            if (string.IsNullOrEmpty(text))
                button.gameObject.SetActive(false);
            else
            {
                var textComponent = button.GetComponentInChildren<TMP_Text>();
                if (textComponent) textComponent.SetText(text);

                button.gameObject.SetActive(true);
            }
        }

        private void UpdateButtonGroup()
        {
            if (!buttonGroup) return;

            buttonGroup.gameObject.SetActive(
                positiveButton && positiveButton.gameObject.activeSelf ||
                negativeButton && negativeButton.gameObject.activeSelf);
        }

        public async Task<bool> ShowAndWaitAsync(
            CancellationToken cancellationToken = default)
        {
            await Show();

            try
            {
                if (!buttonGroup || !buttonGroup.gameObject.activeSelf ||
                    !positiveButton || !negativeButton)
                    return false;

                var tcs = new TaskCompletionSource<bool>();

                using var registration = cancellationToken.Register(
                    () => tcs.TrySetResult(false));

                var positiveAction = new UnityAction(
                    () => tcs.TrySetResult(true));
                var negativeAction = new UnityAction(
                    () => tcs.TrySetResult(false));

                positiveButton.onClick.AddListener(positiveAction);
                negativeButton.onClick.AddListener(negativeAction);

                var result = await tcs.Task;

                positiveButton.onClick.RemoveListener(positiveAction);
                negativeButton.onClick.RemoveListener(negativeAction);

                return result;
            }
            finally
            {
                _ = Hide();
            }
        }
    }
}
