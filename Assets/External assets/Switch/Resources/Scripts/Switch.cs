using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tween;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
    /// <summary>
    /// Taken largely from toggle.cs (Unity GUI source)
    /// </summary>
    [AddComponentMenu("UI/Switch", 35)]
    [RequireComponent(typeof(RectTransform))]
    public class Switch : Selectable, IPointerClickHandler, ISubmitHandler, ICanvasElement
    {
        [Serializable]
        public class ToggleEvent : UnityEvent<bool>
        { }
        
        /// <summary>
        /// Is a tweening animation playing at the moment?
        /// </summary>
        public bool isTweening = false;

        /// <summary>
        /// Dictionary containing references to On and Off Text elements.
        /// </summary>
        [SerializeField]
        private Dictionary<string, Text> m_OnOffText = new Dictionary<string, Text>(){
            {"ON", null}, {"OFF", null}
        };
        public Dictionary<string, Text> OnOffText
        {
            get
            {
                if (m_OnOffText.Values.Contains(null))
                {
                    foreach (Text t in GetComponentsInChildren<Text>(true))
                    {
                        if (t.name == "On") m_OnOffText["ON"] = t;
                        else if (t.name == "Off") m_OnOffText["OFF"] = t;
                    }
                }
                return m_OnOffText;
            }
        }

        /// <summary>
        /// Total duration of a state transition in seconds.
        /// </summary>
        [Range(0.01f,5)]
        public float TransitionDuration = 0.2f;

        /// <summary>
        /// [0] -> Label's colour when selected
        /// [1] -> Label's colour when unselected
        /// </summary>
        public Color[] m_textColor = new Color[2]{
            new Color(.863f, .863f, .863f, 1f), //Selected
            new Color(.392f, .392f, .392f, 1f)  //Unselected
        };
        /// <summary>
        /// [0] -> "Selected" panel's colour when position is On
        /// [1] -> "Selected" panel's colour when position is Off
        /// </summary>
        public Color[] PanelColor = new Color[2]{
            new Color(.188f, .729f, .172f, 1f), //On
            new Color( .78f,  .38f, .333f, 1f)  //Off
        };

        /// <summary>
        /// Transition type.
        /// </summary>
        public Ease toggleTransition = Ease.Linear;

        /// <summary>
        /// Graphic the toggle should be working with.
        /// </summary>
        public Image graphic;

        /// <summary>
        /// The graphic element panel changing colour positionned under the selected On or Off label.
        /// </summary>
        private RectTransform m_selectPanel;
        public RectTransform selectedPanel
        {
            get
            {
                if (!m_selectPanel || m_selectPanel.name != name)
                {
                    try
                    {
                        m_selectPanel = graphic.GetComponent<RectTransform>();
                    }
                    catch
                    {
                        return null;
                    }
                }
                return m_selectPanel;
            }
        }

        /// <summary>
        /// Allow for delegate-based subscriptions for faster events than 'eventReceiver', and allowing for multiple receivers.
        /// </summary>
        public ToggleEvent onValueChanged = new ToggleEvent();

        // Whether the toggle is on
        [Tooltip("Is the toggle currently on or off?")]
        [SerializeField]
        private bool m_IsOn;

        protected Switch()
	        { }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            Set(m_IsOn, false);
            if (!IsActive()) return;

            PlayEffect(toggleTransition == Ease.None);

            var prefabType = UnityEditor.PrefabUtility.GetPrefabType(this);
            if (prefabType != UnityEditor.PrefabType.Prefab && !Application.isPlaying)
                CanvasUpdateRegistry.RegisterCanvasElementForLayoutRebuild(this);
        }
#endif

        public virtual void Rebuild(CanvasUpdate executing)
        {
#if UNITY_EDITOR
            if (executing == CanvasUpdate.Prelayout)
                onValueChanged.Invoke(m_IsOn);
#endif
        }

	    public virtual void LayoutComplete()
	    {}

	    public virtual void GraphicUpdateComplete()
	    {}

        protected override void OnEnable()
        {
            base.OnEnable();
            ToggleState(1);
        }

        /// <summary>
        /// Whether the toggle is currently active.
        /// </summary>
        public bool isOn
        {
            get { return m_IsOn; }
            set
            {
                Set(value);
            }
        }

        void Set(bool value)
        {
            Set(value, true);
        }

        void Set(bool value, bool sendCallback)
        {
            if (m_IsOn == value)
                return;

            m_IsOn = value;

            // Always send event when toggle is clicked, even if value didn't change
            // due to already active toggle in a toggle group being clicked.
            // Controls like SelectionList rely on this.
            // It's up to the user to ignore a selection being set to the same value it already was, if desired.
            PlayEffect(toggleTransition == Ease.None);
            if (sendCallback)
                onValueChanged.Invoke(m_IsOn);
        }

        /// <summary>
        /// Play the appropriate effect.
        /// </summary>
        private void PlayEffect(bool instant)
        {
            if (graphic == null)
                return;
            if (instant)
                ToggleState(1);

            StopCoroutine("Tweening");
            StartCoroutine("Tweening");
        }

        /// <summary>
        /// Assume the correct visual state.
        /// </summary>
        protected override void Start()
        {
            ToggleState(1);
        }

        private void InternalToggle()
        {
            if (!IsActive() || !IsInteractable())
                return;

            isOn = !isOn;
        }

        /// <summary>
        /// React to clicks.
        /// </summary>
        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            InternalToggle();
        }

        public virtual void OnSubmit(BaseEventData eventData)
        {
            InternalToggle();
        }

        /// <summary>
        /// Sets all animated elements with their new tweening values.
        /// </summary>
        /// <param name="CompletionRatio"></param>
        private void ToggleState (float CompletionRatio)
        {
            //If no "selected" image exist or this element is not interactable, we stop there.
            if (!graphic || !interactable)
                return;

            //If the transition is not none we need to get the tweening value.
            if (toggleTransition != Ease.None)
            {
                CompletionRatio = Tween.Equations.methods[(int)toggleTransition](CompletionRatio * TransitionDuration, 0, 1, TransitionDuration);
            }

            selectedPanel.SetInsetAndSizeFromParentEdge(isOn ? RectTransform.Edge.Left : RectTransform.Edge.Right,
                                                        selectedPanel.rect.width * (1 - CompletionRatio) + 3f,
                                                        selectedPanel.rect.width);
            graphic.color = Color.Lerp(PanelColor[isOn ? 1 : 0],
                                       PanelColor[isOn ? 0 : 1],
                                       CompletionRatio);

            //If one of the two labels is missing we forget this last part
            if (!OnOffText.Values.Contains(null))
            {
                OnOffText["ON"].color = Color.Lerp(m_textColor[isOn ? 1 : 0], m_textColor[isOn ? 0 : 1], CompletionRatio);
                OnOffText["OFF"].color = Color.Lerp(m_textColor[isOn ? 0 : 1], m_textColor[isOn ? 1 : 0], CompletionRatio);
            }
        }
        
        /// <summary>
        /// Inbetweening IEnumerator
        /// </summary>
        /// <returns></returns>
        private IEnumerator Tweening()
        {
            //If no "selected" image exist or this element is not active, we stop there.
            if (!graphic || !IsActive())
                yield break;
#if UNITY_EDITOR
            //If we are toggling the switch within the editor custom inspector we cannot use Time.timeSinceLevelLoad
            if (!Application.isPlaying)
            {
                //Show directly the value of the finished animation.
                ToggleState(1);
                yield break;
            }
#endif
            float time = Time.timeSinceLevelLoad;
            float elapsedTime = 0; //Elapsed time  as a ratio of passed time divided by the total time.

            isTweening = true;
            while (elapsedTime < 1)
            {
                elapsedTime = (Time.timeSinceLevelLoad - time) / TransitionDuration;
                //If None, there is no animation.
                if (toggleTransition == Ease.None)
                    break;
                else
                    ToggleState(elapsedTime);
                yield return null;
            }
            ToggleState(1);
            isTweening = false;
        }

        /// <summary>
        /// DoStateTransition is overidden to apply disabled effect on the "selected" image.
        /// </summary>
        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            base.DoStateTransition(state, instant);

            if (!graphic)
                return;

            if (state == SelectionState.Disabled)
            {
                Color nColor = graphic.color;
                nColor.a = colors.disabledColor.a;
                graphic.color = nColor;
            }
            else if (graphic.material.color.a != colors.disabledColor.a)
            {
                Color nColor = graphic.color;
                nColor.a = colors.normalColor.a;
                graphic.color = nColor;
            }
        }
    }
}
