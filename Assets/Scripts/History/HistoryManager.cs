using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;

namespace HISTORY {

    [RequireComponent(typeof(HistoryNavigation))]
    [RequireComponent(typeof(HistoryLogManager))]
    public class HistoryManager : MonoBehaviour
    {
        public const int HISTORY_CACHE_LIMIT = 50;
        public static HistoryManager instance { get; private set; }
        public List<HistoryState> history = new List<HistoryState>();

        private HistoryNavigation navigation;
        public bool isViewingHistory => navigation.isViewingHistory;
        public HistoryLogManager logManager { get; private set; }

        private void Awake()
        {
            instance = this;
            navigation = GetComponent<HistoryNavigation>();
            logManager = GetComponent<HistoryLogManager>();

        }

        // Start is called before the first frame update
        void Start()
        {
            DialogueSystem.instance.OnClear += LogCurrentState;
        }

        public void LogCurrentState()
        {
            HistoryState state = HistoryState.Capture();
            history.Add(state);
            logManager.AddLog(state);

            if(history.Count > HISTORY_CACHE_LIMIT)
            {
                history.RemoveAt(0);
            }
        }

        public void LoadState(HistoryState state)
        {
            state.Load();
        }

        //links to navigation
        public void GoForward() => navigation.GoForward();
        public void GoBack() => navigation.GoBack();
    }
}