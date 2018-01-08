﻿/*
 * Author: Shon Verch
 * File Name: CardInstance.cs
 * Project Name: TheDungeonMaster
 * Creation Date: 12/26/2017
 * Modified Date: 12/27/2017
 * Description: The interface between the card data and the card visuals.
 */

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <inheritdoc cref="MonoBehaviour" />
/// <summary>
/// The interface between the card data and the card visuals.
/// </summary>
[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(RectTransform))]
public class CardInstance : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    /// <summary>
    /// The card data which this <see cref="CardInstance"/> is initialized from.
    /// </summary>
    public Card Card { get; private set; }

    /// <summary>
    /// The <see cref="CanvasGroup"/> belonging to this <see cref="CardInstance"/>.
    /// </summary>
    public CanvasGroup CanvasGroup => canvasGroup ?? (canvasGroup = GetComponent<CanvasGroup>());

    /// <summary>
    /// The <see cref="RectTransform"/> of this <see cref="CardInstance"/>.
    /// </summary>
    public RectTransform RectTransform => rectTransform ?? (rectTransform = GetComponent<RectTransform>());

    [SerializeField]
    private Text cardNameText;
    [SerializeField]
    private Text cardDescriptionText;
    [SerializeField]
    private Text cardAttackPointsText;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    /// <summary>
    /// Initializes this <see cref="CardInstance"/> from a <see cref="Card"/>.
    /// </summary>
    /// <param name="card">The <see cref="Card"/> data to initialize from.</param>
    public void Initialize(Card card)
    {
        Card = card;
        gameObject.name = $"{card.Name}_instance";
        cardNameText.text = card.Name;
        cardDescriptionText.text = card.Description;
        cardAttackPointsText.text = card.AttackPoints.ToString();
    }

    /// <summary>
    /// Creates a <see cref="CardInstance"/> from a <see cref="global::Card"/>.
    /// </summary>
    /// <param name="card">The <see cref="global::Card"/> to create the <see cref="CardInstance"/> from.</param>
    /// <param name="parent">The parent of this <see cref="CardInstance"/>.</param>
    /// <returns>The card instance's <see cref="GameObject"/>.</returns>
    public static CardInstance Create(Card card, RectTransform parent)
    {
        GameObject cardGameObject = Instantiate(Resources.Load<GameObject>("Prefabs/Card_Front"));
        cardGameObject.transform.SetParent(parent, false);

        CardInstance cardInstance = cardGameObject.GetComponent<CardInstance>();
        cardInstance.Initialize(card);

        return cardInstance;
    }

    /// <summary>
    /// Executed when the pointer is over the <see cref="T:UnityEngine.GameObject" /> pertaining to this <see cref="T:UnityEngine.MonoBehaviour" />.
    /// </summary>
    /// <param name="eventData">The data pertaining to this event.</param>
    public void OnPointerEnter(PointerEventData eventData) => ControllerDatabase.Get<CardInteractionController>().BeginHover(this);

    /// <summary>
    /// Executed when the pointer leaves the <see cref="T:UnityEngine.GameObject" /> pertaining to this <see cref="T:UnityEngine.MonoBehaviour" />.
    /// </summary>
    /// <param name="eventData">The data pertaining to this event.</param>
    public void OnPointerExit(PointerEventData eventData) => ControllerDatabase.Get<CardInteractionController>().EndHover(this);

    /// <summary>
    /// Executed when the pointer is down on the <see cref="T:UnityEngine.GameObject" /> pertaining to this <see cref="T:UnityEngine.MonoBehaviour" />.
    /// </summary>
    /// <param name="eventData">The data pertaining to this event.</param>
    public void OnPointerDown(PointerEventData eventData) => ControllerDatabase.Get<CardInteractionController>().BeginDrag(this);
}
