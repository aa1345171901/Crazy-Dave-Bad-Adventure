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
        [Tooltip("�Ի�����")]
        public List<string> dialogues;
        [Tooltip("��ʾ�Ի��Ŀؼ�")]
        public Text dialogueText;
        [Tooltip("�Ƿ������������ֲ���")]
        public bool IsTextPlaying = true;
        [ConditionHiden("IsTextPlaying", true)]
        [Tooltip("���ֲ��Ŷ�ò���һ���ַ�")]
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
                Debug.LogError("�Ի�����Ϊ��");
                return;
            }
            if (index >= dialogues.Count)
            {
                FinishedEvent?.Invoke();
                return;
            }
            // ������ڲ��ţ���ֱ����ʾȫ���Ի�
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
        /// ���öԻ�
        /// </summary>
        public void ResetDialogue()
        {
            index = 0;
            playing = false;
            ResetDialogueEvent?.Invoke();
            NextDialogue();
        }

        /// <summary>
        /// ���ֲ��ŶԻ�
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
