using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum IconState {
    SELECTED,
    STANDBY
}

public struct SelectionIcon {
    public RawImage icon;
    public IconState state;

    public SelectionIcon(RawImage img, IconState s) { this.icon = img; this.state = s; }

    public void SetSelectionAndAnimate(float ANIM_ICON_SCALE, float ANIM_DURATION) {
        if (this.state == IconState.SELECTED) {
                LeanTween.scale(this.icon.gameObject, Vector3.one * ANIM_ICON_SCALE, ANIM_DURATION);
                this.state = IconState.STANDBY;
        }
    }
}

public class SelectionBarManager : MonoBehaviour
{
    [SerializeField] private PartsPlacementManager ppm;

    [Header("UI Reference")]
    [SerializeField] private Transform part_ui_container;
    [SerializeField] private RawImage part_ui_icon;
    [SerializeField] private Text selected_part_text;
    [SerializeField] private RawImage debug_image;

    [Header("UI settings")]
    [SerializeField] private float ANIM_TEXT_SCALE = 0.7f;
    [SerializeField] private float ANIM_TEXT_OPACITY = 0.7f;
    [SerializeField] private float ANIM_ICON_SCALE = 0.7f;
    [SerializeField] private float ANIM_DURATION = 0.2f;
    

    [Header("Scripts references")]
    [SerializeField] private IconRenderer iconRenderer;
    [SerializeField] private LoadingScreenManager loadingScreenManager;

    [HideInInspector] private SelectionIcon[] icons;
    [HideInInspector] private int currently_selected_ui;


    private void Awake() {
        Bar_Populate();
    }

    public void SelectCurrentPart() {
        Part part = ppm.parts[ppm.currently_selected];
        UpdateAndAnimatePartNameText(part);

        for (int i = 0; i < icons.Length; i++) {
            SelectionIcon icon = icons[i];
            icon.SetSelectionAndAnimate(ANIM_ICON_SCALE, ANIM_DURATION);
        }

        SelectionIcon currentIcon = GetCurrentIcon();
        LeanTween.scale(currentIcon.icon.gameObject, Vector3.one * 1f, ANIM_DURATION);
        SetCurrentIconState(IconState.SELECTED);
    }

    public void UpdateAndAnimatePartNameText(Part part) {
        // c'est totalement illisible
        // désole pour ça futur moi
        selected_part_text.text = part.name;

        LeanTween.cancel(selected_part_text.gameObject);

        LeanTween.value(selected_part_text.gameObject, 0f, 1f, 0.1f).setOnUpdate((value) => {
            selected_part_text.color = new Color(1f, 1f, 1f, ANIM_TEXT_OPACITY * value + (1f - ANIM_TEXT_OPACITY));
            selected_part_text.GetComponent<RectTransform>().localScale = Vector3.one * (ANIM_TEXT_SCALE + (1 - ANIM_TEXT_SCALE) * value);
        });

        LeanTween.value(selected_part_text.gameObject, 1f, 0f, 0.3f).setDelay(1f).setOnUpdate((value) => {
            selected_part_text.color = new Color(1f, 1f, 1f, ANIM_TEXT_OPACITY * value + (1f - ANIM_TEXT_OPACITY));
            selected_part_text.GetComponent<RectTransform>().localScale = Vector3.one * (ANIM_TEXT_SCALE + (1 - ANIM_TEXT_SCALE) * value);
        });
    }

    public void Bar_Populate() {
        icons = new SelectionIcon[ppm.parts.Length];
        for (int i = 0; i < ppm.parts.Length; i++) {
            Part part = ppm.parts[i];
            RawImage icon = Instantiate(part_ui_icon) as RawImage;
            icon.transform.SetParent(part_ui_container);
            icon.texture = iconRenderer.render_icon(ppm.parts[i], 512);

            icons[i] = new SelectionIcon(icon, IconState.SELECTED);
        }

        SelectCurrentPart();
        iconRenderer.clean();
        loadingScreenManager.hide_loading_screen();
    }

    public void SetBuildMode(bool build_mode) {
        float start = (build_mode ? 1f : 0f);
        float end = 1f - start;
        LeanTween.value(this.gameObject, start, end, 0.2f).setOnUpdate((value) => {
            Vector2 pos = this.GetComponent<RectTransform>().anchoredPosition;
            this.GetComponent<RectTransform>().anchoredPosition = new Vector2(pos.x, Mathf.Lerp(10f, -80f, value));
        });
    }

    private SelectionIcon GetCurrentIcon() { return icons[ppm.currently_selected]; }
    private void SetCurrentIconState(IconState state) {icons[ppm.currently_selected].state = state; }
}
