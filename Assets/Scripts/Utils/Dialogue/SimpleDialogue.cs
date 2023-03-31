using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace TopDownPlate
{
    [AddComponentMenu("TopDownPlate/Utils/SimpleDialogue")]
    public class SimpleDialogue : MonoBehaviour
    {
        [Tooltip("对话集合")]
        public List<string> dialogues;
        [Tooltip("显示对话的控件")]
        public Text dialogueText;
        [Tooltip("是否启用文字逐字播放")]
        public bool IsTextPlaying = true;
        [ConditionHiden("IsTextPlaying", true)]
        [Tooltip("文字播放多久播放一个字符")]
        public float TextPlayingDelay = 0.1f;

        [Space(10)]
        [Header("Event")]
        public UnityEvent PlayingEvent;
        public UnityEvent FinishedEvent;
        public UnityEvent ResetDialogueEvent;

        private int index = 0;
        private bool playing = false;

        public void NextDialogue()
        {
            if (dialogues == null || dialogues.Count <= 0)
            {
                Debug.LogError("对话集合为空");
                return;
            }
            if (index >= dialogues.Count)
            {
                FinishedEvent?.Invoke();
                return;
            }
            // 如果正在播放，则直接显示全部对话
            if (playing)
            {
                playing = false;
                return;
            }
            ConductDialogue();
        }

        private void ConductDialogue()
        {
            if (IsTextPlaying)
            {
                playing = true;
                StartCoroutine(PlayDialogue());
            }
            else
                dialogueText.text = dialogues[index++];
        }

        /// <summary>
        /// 重置对话
        /// </summary>
        public void ResetDialogue()
        {
            index = 0;
            playing = false;
            ResetDialogueEvent?.Invoke();
            NextDialogue();
        }

        /// <summary>
        /// 逐字播放对话
        /// </summary>
        /// <returns></returns>
        IEnumerator PlayDialogue()
        {
            if (playing)
            {
                int len = dialogues[index].Length;
                string dialogue = dialogues[index];
                for (int i = 0; i < len; i++)
                {
                    if (!playing)
                    {
                        DialoguePlayed(dialogue);
                        yield break;
                    }
                    dialogueText.text = dialogue.Substring(0, i);
                    PlayingEvent?.Invoke();
                    yield return new WaitForSeconds(TextPlayingDelay);
                }
                DialoguePlayed(dialogue);
                PlayingEvent?.Invoke();
                yield return new WaitForSeconds(TextPlayingDelay);
            }
            else
            {
                yield break;
            }
        }

        private void DialoguePlayed(string dialogue)
        {
            playing = false;
            dialogueText.text = dialogue;
            index++;
        }
    }
}
