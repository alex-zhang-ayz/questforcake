using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class ScrollRectNoDrag : ScrollRect {
	public override void OnBeginDrag(PointerEventData eventData) { }
	public override void OnDrag(PointerEventData eventData) { }
	public override void OnEndDrag(PointerEventData eventData) { }
}
